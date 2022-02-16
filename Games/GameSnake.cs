using NeuralNet.Network;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNet.Games
{
	public class GameSnake
	{
		class SensorData
		{
			public double WallDistance { set; get; }
			public double BodyDistance { set; get; }
			public double FoodDistance { set; get; }
			public double TailDistance { set; get; }
		}

		public enum Directions
		{
			None,
			Up,
			Down,
			Left,
			Right
		}

		Coordinate[] mDelta = new Coordinate[]
		{
			new Coordinate(){ X = 0, Y = 0},
			new Coordinate(){ X = 0, Y = -1},
			new Coordinate(){ X = 0, Y = 1},
			new Coordinate(){ X = -1, Y = 0},
			new Coordinate(){ X = 1, Y = 0}
		};

		Directions mLastDirection = Directions.None;
		int mMoveCount = 0;
		int mMoveCountFoodless = 0;
		const int MaxMoves = 75;
		int mCellSize = 8;
		bool mAlive = true;
		int mWorldSize;
		double mMaxDistance;
		double[] mInputs;
		LinkedList<Coordinate> mSnakeBody = new LinkedList<Coordinate>();
		Coordinate mFoodLoc = new Coordinate();
		Coordinate mHeadLocOld = new Coordinate();
		Coordinate mTailLocOld = new Coordinate();
		NeuralNetwork mBrain;
		NetworkTrainer mTrainer;
		TrainingSet mTrainData;
		Bitmap mImage;
		Graphics mGraphics;


		public bool IsAlive { get { return mAlive; } }
		public Bitmap Image { get { return mImage; } }
		public NeuralNetwork Brain {get { return mBrain; }  set { mBrain = value; } }
		public TrainingSet TrainingData { get { return mTrainData; } set { mTrainData = value; } }
		public int Length { get { return mSnakeBody.Count(); } }
		public int MoveCount { get { return mMoveCount; } }
		public NetworkTrainer Trainer { get { return mTrainer; } }

		public GameSnake(int worldSize)
		{
			mWorldSize = worldSize;
			//	mCellSize = 512 / worldSize;
			//	mImage = new Bitmap(512, 512);
			mImage = new Bitmap(worldSize * mCellSize, mWorldSize * mCellSize);
			mMaxDistance = Math.Sqrt(2 * worldSize * worldSize);
			mBrain = new NeuralNetwork(new uint[] { 16, 12,12, 4 }, ActivationMethod.TanH, ActivationMethod.LeakyReLU);
			mInputs = new double[16];
			mTrainer = new NetworkTrainer();
			mTrainData = new TrainingSet();
			
			mGraphics = Graphics.FromImage(mImage);
			mGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			mGraphics.Clear(Color.Black);
			InitSnake();
			GenerateFood();
			Draw();
		}

		private void Die()
		{
			mMoveCount = 0;
			mAlive = false;
		}

		private void GenerateFood()
		{
			bool collision = false;
			do
			{
				mFoodLoc = GetRandomLocation();

				var pos = mSnakeBody.First;
				collision = false;
				while (pos != null)
				{
					if (pos.Value.IsEqual(mFoodLoc))
					{
						collision = true;
						break;
					}
					pos = pos.Next;
				}
			} while (collision);
		}

		private void InitSnake()
		{
			mSnakeBody.Clear();
			var cor = GetRandomLocation();
			mSnakeBody.AddFirst(cor);
			mHeadLocOld.X = cor.X;
			mHeadLocOld.Y = cor.Y;
			mTailLocOld.X = cor.X;
			mTailLocOld.Y = cor.Y;

			mAlive = true;
		}

		private Directions DecideDirection(double[] netOutput)
		{
			Directions movement = Directions.Up;
			var max = netOutput.Max();
			if (max == netOutput[0])
			{
				movement = Directions.Up;
			}
			else if (max == netOutput[1])
			{
				movement = Directions.Down;
			}
			else if (max == netOutput[2])
			{
				movement = Directions.Left;
			}
			else
			{
				movement = Directions.Right;
			}

			return movement;
		}

		private double[] GenerateTrainingOutput(Directions mt)
		{
			double[] output = new double[4];
			if (mt == Directions.Up)
			{
				output[0] = 1;
				output[1] = 0;
				output[2] = 0;
				output[3] = 0;
			}
			else if (mt == Directions.Down)
			{
				output[0] = 0;
				output[1] = 1;
				output[2] = 0;
				output[3] = 0;
			}
			else if (mt == Directions.Left)
			{
				output[0] = 0;
				output[1] = 0;
				output[2] = 1;
				output[3] = 0;
			}
			else
			{
				output[0] = 0;
				output[1] = 0;
				output[2] = 0;
				output[3] = 1;
			}

			return output;
		}

		private double[] GenerateAntiWallTrainingOutput(Directions direction)
		{
			double[] trOutput = new double[4];
			var headLoc = mSnakeBody.First.Value;

			switch (direction)
			{
				// the last calculated move was UP and we hit the top border and failed
				// all the possible moves should be investigated based on the last direction and the the current failed direction
				// UU, RU, LU, 
				case Directions.Up:
				case Directions.Down:
					trOutput[0] = 0;
					trOutput[1] = 0;
					if (headLoc.X == mWorldSize - 1) // at right there is wall
					{
						trOutput[2] = 1; // left is the better choice
						trOutput[3] = 0;
					}
					else if (headLoc.X == 0) // at left there is wall
					{
						trOutput[2] = 0;
						trOutput[3] = 1; // right is the better choice
					}
					else
					{
						// both left and right are good choices
						trOutput[2] = 1;
						trOutput[3] = 1;
					}
					break;
				case Directions.Left:
				case Directions.Right:
					trOutput[2] = 0;
					trOutput[3] = 0;
					if (headLoc.Y == mWorldSize - 1)
					{
						trOutput[0] = 1; // up is the better choice
						trOutput[1] = 0;
					}
					else if (headLoc.Y == 0) // at up there is wall
					{
						trOutput[0] = 0;
						trOutput[1] = 1; // down is the better choice
					}
					else
					{
						// both up and down are good choices
						trOutput[0] = 1;
						trOutput[1] = 1;
					}
					break;
			}
			return trOutput;
		}
		
		private double[] GenerateAntiBodyTrainingOutput(Directions direction)
		{
			double[] trOutput = new double[4];
			var headLoc = mSnakeBody.First.Value;
			switch (direction)
			{
				case Directions.Up:
				case Directions.Down:
					trOutput[0] = 0;
					trOutput[1] = 0;
					if (headLoc.X == mWorldSize - 1) // at right there is wall
					{
						trOutput[2] = 1; // left is the better choice
						trOutput[3] = 0;
					}
					else if (headLoc.X == 0) // at left there is wall
					{
						trOutput[2] = 0;
						trOutput[3] = 1; // right is the better choice
					}
					else
					{
						// both left and right are good choices
						trOutput[2] = 1;
						trOutput[3] = 1;
					}
					break;
				case Directions.Left:
				case Directions.Right:
					trOutput[2] = 0;
					trOutput[3] = 0;
					if (headLoc.Y == mWorldSize - 1)
					{
						trOutput[0] = 1; // up is the better choice
						trOutput[1] = 0;
					}
					else if (headLoc.Y == 0) // at up there is wall
					{
						trOutput[0] = 0;
						trOutput[1] = 1; // down is the better choice
					}
					else
					{
						// both up and down are good choices
						trOutput[0] = 1;
						trOutput[1] = 1;
					}
					break;
			}
			return trOutput;
		}

		private SensorData GetSensorData(Directions dir)
		{
			SensorData sd = new SensorData();
			var headLoc = mSnakeBody.First.Value;
			var tailLoc = mSnakeBody.Last.Value;

			switch(dir)
			{
				case Directions.Up:
					if (mFoodLoc.X == headLoc.X && mFoodLoc.Y < headLoc.Y) // it is in the same column
						sd.FoodDistance = Math.Abs(mFoodLoc.Y - headLoc.Y) / mMaxDistance;
					else
						sd.FoodDistance = 1;

					if (tailLoc.X == headLoc.X && tailLoc.Y < headLoc.Y) // it is in the same column
						sd.TailDistance = Math.Abs(tailLoc.Y - headLoc.Y) / mMaxDistance;
					else
						sd.TailDistance = 1;

					sd.WallDistance = headLoc.Y / mMaxDistance;
					sd.BodyDistance = 1;
					
					var matches = mSnakeBody.Where(p => p.X == headLoc.X && p.Y < headLoc.Y);
					if (matches.Count() > 0)
					{
						var body = matches.Max(p => p.Y);
						sd.BodyDistance = Math.Abs(body - headLoc.Y) / mMaxDistance;
					}
					break;
				case Directions.Down:
					if (mFoodLoc.X == headLoc.X && mFoodLoc.Y > headLoc.Y) // it is in the same column
						sd.FoodDistance = Math.Abs(mFoodLoc.Y - headLoc.Y) / mMaxDistance;
					else
						sd.FoodDistance = 1;

					if (tailLoc.X == headLoc.X && tailLoc.Y > headLoc.Y) // it is in the same column
						sd.TailDistance = Math.Abs(tailLoc.Y - headLoc.Y) / mMaxDistance;
					else
						sd.TailDistance = 1;

					sd.WallDistance = (mWorldSize - 1 - headLoc.Y) / mMaxDistance;
					sd.BodyDistance = 1;
					matches = mSnakeBody.Where(p => p.X == headLoc.X && p.Y > headLoc.Y);
					if (matches.Count() > 0)
					{
						var body = matches.Min(p => p.Y);
						sd.BodyDistance = Math.Abs(body - headLoc.Y) / mMaxDistance;
					}
					break;
				case Directions.Left:
					if (mFoodLoc.Y == headLoc.Y && mFoodLoc.X < headLoc.X) // it is in the same row
						sd.FoodDistance = Math.Abs(mFoodLoc.X - headLoc.X) / mMaxDistance;
					else
						sd.FoodDistance = 1;

					if (tailLoc.Y == headLoc.Y && tailLoc.X < headLoc.X) // it is in the same row
						sd.TailDistance = Math.Abs(tailLoc.X - headLoc.X) / mMaxDistance;
					else
						sd.TailDistance = 1;

					sd.WallDistance = headLoc.X / mMaxDistance;
					sd.BodyDistance = 1;
					
					matches = mSnakeBody.Where(p => p.Y == headLoc.Y && p.X < headLoc.X);
					if (matches.Count() > 0)
					{
						var body = matches.Max(p => p.X);
						sd.BodyDistance = Math.Abs(body - headLoc.X) / mMaxDistance;
					}
					break;
				case Directions.Right:
					if (mFoodLoc.Y == headLoc.Y && mFoodLoc.X > headLoc.X) // it is in the same row
						sd.FoodDistance = Math.Abs(mFoodLoc.X - headLoc.X) / mMaxDistance;
					else
						sd.FoodDistance = 1;

					if (tailLoc.Y == headLoc.Y && tailLoc.X > headLoc.X) // it is in the same row
						sd.TailDistance = Math.Abs(tailLoc.X - headLoc.X) / mMaxDistance;
					else
						sd.TailDistance = 1;

					sd.WallDistance = (mWorldSize - 1 - headLoc.X) / mMaxDistance;
					sd.BodyDistance = 1;
					
					matches = mSnakeBody.Where(p => p.Y == headLoc.Y && p.X > headLoc.X);
					if (matches.Count() > 0)
					{
						var body = matches.Min(p => p.X);
						sd.BodyDistance = Math.Abs(body - headLoc.X) / mMaxDistance;
					}
					break;
			}
			
			return sd;
		}

		private double[] CheckSensors()
		{
			// look in 8 directions and calculate distances from food, wall and tail

			var sd = GetSensorData(Directions.Up);
			mInputs[0] = sd.FoodDistance;
			mInputs[1] = sd.TailDistance;
			mInputs[2] = sd.WallDistance;
			mInputs[3] = sd.BodyDistance;

			sd = GetSensorData(Directions.Down);
			mInputs[5] = sd.FoodDistance;
			mInputs[6] = sd.TailDistance;
			mInputs[7] = sd.WallDistance;
			mInputs[8] = sd.BodyDistance;

			sd = GetSensorData(Directions.Left);
			mInputs[8] = sd.FoodDistance;
			mInputs[9] = sd.TailDistance;
			mInputs[10] = sd.WallDistance;
			mInputs[11] = sd.BodyDistance;

			sd = GetSensorData(Directions.Right);
			mInputs[12] = sd.FoodDistance;
			mInputs[13] = sd.TailDistance;
			mInputs[14] = sd.WallDistance;
			mInputs[15] = sd.BodyDistance;

			return mInputs;
		}

		private bool IsWall(Coordinate cord)
		{
			return (cord.X == mWorldSize || cord.Y == mWorldSize || cord.X == -1 || cord.Y == -1);	
		}

		private bool IsFood(Coordinate cord)
		{
			return (cord.X == mFoodLoc.X && cord.Y == mFoodLoc.Y);
		}

		private bool IsBody(Coordinate cord)
		{
			bool result = false;
			var pos = mSnakeBody.First;

			while(pos != null)
			{
				pos = pos.Next;
				if (pos == null) break;

				if(pos.Value.IsEqual(cord))
				{
					result = true;
					break;
				}
			}
			return result;
		}

		private bool IsReverse(Directions direction)
		{
			bool result = false;
			switch(direction)
			{
				case Directions.Down:
					if (mLastDirection == Directions.Up) result = true;
					break;
				case Directions.Up:
					if (mLastDirection == Directions.Down) result = true;
					break;
				case Directions.Left:
					if (mLastDirection == Directions.Right) result = true;
					break;
				case Directions.Right:
					if (mLastDirection == Directions.Left) result = true;
					break;
			}

			return result;
		}

		public void Move()
		{
			if (!mAlive) return;

			var sensorData = CheckSensors();
			var output = mBrain.Process(sensorData);
			var direction = DecideDirection(output);
			var trOutput = output;
			var headLoc = mSnakeBody.First.Value.Clone();
			
			if (IsReverse(direction))
			{
				direction = mLastDirection;
			}

			var temp = GetSensorData(direction);

			headLoc.CopyTo(mHeadLocOld);
			headLoc.Add(mDelta[(int)direction]);

			if (IsWall(headLoc))
			{
				Die();

				trOutput = GenerateAntiWallTrainingOutput(direction);
				mTrainData.Add(new TrainingPair() { Inputs = (double[])sensorData.Clone(), Outputs = trOutput });
				Learn();

				InitSnake();
				GenerateFood();
			}
			else if (IsBody(headLoc))
			{
				Die();

				trOutput = GenerateAntiBodyTrainingOutput(direction);
				mTrainData.Add(new TrainingPair() { Inputs = (double[])sensorData.Clone(), Outputs = trOutput });
				Learn();

				InitSnake();
				GenerateFood();
			}
			else if (IsFood(headLoc))
			{
				mMoveCountFoodless = 0;
				mMoveCount++;
				mSnakeBody.AddFirst(headLoc);
				GenerateFood();

				trOutput = GenerateTrainingOutput(direction);
				mTrainData.Add(new TrainingPair() { Inputs = (double[])sensorData.Clone(), Outputs = trOutput });
			}
			else
			{
				mMoveCountFoodless++;
				mMoveCount++;
				mSnakeBody.RemoveLast();
				mSnakeBody.AddFirst(headLoc);
				if(mMoveCountFoodless == MaxMoves)
				{
					Die();

				//	trOutput = GeneratePrependicularTrainingOutput(direction);
				//	mTrainData.Add(new TrainingPair() { Inputs = (double[])sensorData.Clone(), Outputs = trOutput });
				//	Learn();

					InitSnake();
					GenerateFood();
				}
			//	mTrainData.Add(new TrainingPair() { Inputs = (double[])input.Clone(), Outputs = (double[])output.Clone() });
			}

			mLastDirection = direction;
			
			Draw();
		}

		private void Learn()
		{
			mTrainer.Network = mBrain;
			mTrainer.DataSet = mTrainData;
			mTrainer.LearningRate = 0.1;
			mTrainer.ErrorThreshold = 0.01;
			mTrainer.Train(10, TrainingMode.Batch, ProcessMode.Synch);

		//	mTrainData.Clear();
		}

		private Coordinate GetRandomLocation()
		{
			Random rnd = new Random();
			int x = rnd.Next(0, mWorldSize - 1);
			int y = rnd.Next(0, mWorldSize - 1);
			return new Coordinate() { X = x, Y = y };
		}

		public void Draw()
		{
			mGraphics.Clear(Color.Black);

			float x = mFoodLoc.X * mCellSize;
			float y = mFoodLoc.Y * mCellSize;

			mGraphics.FillEllipse(Brushes.Green, x, y, mCellSize, mCellSize);

			var pos = mSnakeBody.First;

			var bosdyLoc = pos.Value;
			x = bosdyLoc.X * mCellSize;
			y = bosdyLoc.Y * mCellSize;
			mGraphics.FillEllipse(Brushes.Red, x, y, mCellSize, mCellSize);

			while(pos != null)
			{
				pos = pos.Next;
				if (pos == null) break;

				bosdyLoc = pos.Value;
				x = bosdyLoc.X * mCellSize;
				y = bosdyLoc.Y * mCellSize;
				mGraphics.FillEllipse(Brushes.Gray, x, y, mCellSize, mCellSize);
			}
		}
	}
}
