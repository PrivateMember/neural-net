using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using NeuralNet.Utility;
using System.Threading;

namespace NeuralNet.Network
{
	public enum TrainingMode
	{
		Single,
		Batch,
		Full
	}

	public enum ProcessMode
	{
		Synch,
		Asynch
	}

	public class NetworkTrainer
	{
		private Thread mThread = null;
		private TrainingMode mMode = TrainingMode.Single;
		private uint mIteration = 1;
		private Mutex mLock = new Mutex();
		private bool mStop = false;
		private bool mRunning = false;
		private double mMeanError = 0;
		public double LearningRate { set; get; } = 0.1f;
		public double ErrorThreshold { set; get; } = 0.001f;
		public NeuralNetwork Network { set; get; }
		public TrainingSet DataSet { set; get; } = null;
		public double MeanError { get { return mMeanError; } }

		public event MessageValue OnTrainingIteration = null;
		public event MessageErrorVector OnBatchComplete = null;
		public event Message OnFinish = null;

		public NetworkTrainer()
		{
		}

		public NetworkTrainer(NeuralNetwork net)
		{
			Network = net;
		}

		public void Start()
		{
			if(!mRunning)
			{
				mThread = new Thread(new ThreadStart(ThreadFunc));
				mThread.Start();
			}
		}

		private void ThreadFunc()
		{
			mRunning = true;
			
			switch (mMode)
			{
				case TrainingMode.Full: TrainFull(mIteration); break;
				case TrainingMode.Batch: TrainBatch(mIteration); break;
				case TrainingMode.Single: TrainSingle(mIteration); break;
			}

			OnFinish?.Invoke("");

			mRunning = false;
		}

		private void TrainSingle(uint iteration = 1)
		{
			double[][] deltaTerms = new double[Network.Layers.Count][];
			var netErrors = new double[Network.OutputSize];
			var data = DataSet.Pairs;

			for (int iter = 0; iter < iteration; iter++)
			{
				var netOutputs = Network.Process(data[0].Inputs);
				Utility.VectorOps.Subtract(netOutputs, data[0].Outputs, netErrors);
			//	var totalError = Utility.VectorOps.MeanSquared(netErrors);

				//===== for the output layer ======
				var layers = Network.Layers;
				int layerIdx = layers.Count - 1;
				var curLayer = layers[layerIdx];
				var preLayer = layers[layerIdx - 1];
				var neurons = curLayer.Neurons;
				deltaTerms[layerIdx] = new double[curLayer.Size];
				for (int nIdx = 0; nIdx < neurons.Length; nIdx++)
				{
					var weights = neurons[nIdx].Weights;
					var weightsDelta = neurons[nIdx].WeightsDelta;

					deltaTerms[layerIdx][nIdx] = netErrors[nIdx] * curLayer.OutputsD[nIdx];

					for (int wIdx = 0; wIdx < weightsDelta.Length; wIdx++)
					{
						weightsDelta[wIdx] = deltaTerms[layerIdx][nIdx] * preLayer.Outputs[wIdx];
					}
				}
				//=================================

				//===== for the hidden layers =====
				layerIdx--;
				while(layerIdx > 0)
				{
					curLayer = layers[layerIdx];
					preLayer = layers[layerIdx - 1];
					var nxtLayer = layers[layerIdx + 1];
					neurons = curLayer.Neurons;
					deltaTerms[layerIdx] = new double[curLayer.Size];

					for (int nIdx = 0; nIdx < neurons.Length; nIdx++)
					{
						var weights = neurons[nIdx].Weights;
						var weightsDelta = neurons[nIdx].WeightsDelta;

						neurons = nxtLayer.Neurons;
						for (int npIdx = 0; npIdx < neurons.Length; npIdx++)
						{
							deltaTerms[layerIdx][nIdx] += deltaTerms[layerIdx + 1][npIdx] * neurons[npIdx].Weights[nIdx];
						}

						for (int wIdx = 0; wIdx < weightsDelta.Length; wIdx++)
						{
							weightsDelta[wIdx] = deltaTerms[layerIdx][nIdx] * curLayer.OutputsD[nIdx] * preLayer.Outputs[wIdx];
						}
					}
					layerIdx--;
				}
				//=================================

				//======Update the Weights ========
				for (layerIdx = 1; layerIdx < layers.Count; layerIdx++)
				{
					curLayer = layers[layerIdx];
					neurons = curLayer.Neurons;
					for (int nIdx = 0; nIdx < neurons.Length; nIdx++)
					{
						var weights = neurons[nIdx].Weights;
						var delats = neurons[nIdx].WeightsDelta;

						for (int wIdx = 0; wIdx < weights.Length; wIdx++)
						{
							weights[wIdx] -= LearningRate * delats[wIdx];
						}
					}
				}
				//=================================

			}
		}


		private double CalculateMeanError(List<TrainingPair> data)
		{
			double errorSum = 0;
			foreach (var d in data)
			{
				var netOutputs = Network.Process(d.Inputs);
				var netErrors = VectorOps.Subtract(netOutputs, d.Outputs);
				errorSum += VectorOps.Magnitude(netErrors);
			}
		//	OnBatchComplete?.Invoke(errorSum);
			return errorSum * 0.5f / data.Count;
		}

		private void BackPropagate()
		{

		}

		private void CalculateDelatW(TrainingPair data)
		{
			double[][] errorTerms = new double[Network.Layers.Count][];

			// feed forward
			var netOutputs = Network.Process(data.Inputs);
			// claculate errors
			var netErrors = VectorOps.Subtract(netOutputs, data.Outputs);

			var layers = Network.Layers;

			//===== for the output layer ======
			int layerIdx = layers.Count - 1;
			var curLayer = layers[layerIdx];
			var preLayer = layers[layerIdx - 1];
			var neurons = curLayer.Neurons;

			errorTerms[layerIdx] = new double[curLayer.Size];
			
			for (int nIdx = 0; nIdx < neurons.Length; nIdx++)
			{
				var neuron = neurons[nIdx];

				errorTerms[layerIdx][nIdx] = netErrors[nIdx] * curLayer.OutputsD[nIdx];

				for (int wIdx = 0; wIdx < neuron.WeightsDelta.Length; wIdx++)
				{
					// accumulate the wDelta for all the inputs in the training batch 
					neuron.WeightsDelta[wIdx] += errorTerms[layerIdx][nIdx] * preLayer.Outputs[wIdx];
				}

				neuron.BiasDelta += errorTerms[layerIdx][nIdx];
			}

		//	deltaTerms[layerIdx] = VectorOps.Multiply(netErrors, curLayer.OutputsD, LearningRate);
			//=================================

			//===== for the hidden layers =====
			layerIdx--;
			while (layerIdx > 0)
			{
				curLayer = layers[layerIdx];
				preLayer = layers[layerIdx - 1];
				var nxtLayer = layers[layerIdx + 1];
				neurons = curLayer.Neurons;
				errorTerms[layerIdx] = new double[curLayer.Size];

				for (int nIdx = 0; nIdx < neurons.Length; nIdx++)
				{
					var neuron = neurons[nIdx];
					var wDelta = neuron.WeightsDelta;

					errorTerms[layerIdx][nIdx] = 0;
					for (int npIdx = 0; npIdx < nxtLayer.Neurons.Length; npIdx++)
					{
						errorTerms[layerIdx][nIdx] += errorTerms[layerIdx + 1][npIdx] * nxtLayer.Neurons[npIdx].Weights[nIdx];
					}

					errorTerms[layerIdx][nIdx] *= curLayer.OutputsD[nIdx];

					for (int wIdx = 0; wIdx < wDelta.Length; wIdx++)
					{
						wDelta[wIdx] += errorTerms[layerIdx][nIdx] * preLayer.Outputs[wIdx];
					}

					neuron.BiasDelta += errorTerms[layerIdx][nIdx];
				}
				layerIdx--;
			}

			layerIdx = 0; // to catch a break point
		}

		private void CalculateDelatW(List<TrainingPair> batch)
		{
			foreach (var data in batch)
			{
				CalculateDelatW(data);
			}
		}

		private void UpdateWeights(int batchSize)
		{
			double rate = LearningRate / batchSize;
			// ignore the input layer (index = 0)
			for (int layerIdx = 1; layerIdx < Network.Layers.Count; layerIdx++)
			{
				var layer = Network.Layers[layerIdx];
				var neurons = layer.Neurons;

				for (int nIdx = 0; nIdx < neurons.Length; nIdx++)
				{
					var weights = neurons[nIdx].Weights;
					var wDelats = neurons[nIdx].WeightsDelta;

					for (int wIdx = 0; wIdx < weights.Length; wIdx++)
					{
						weights[wIdx] -= wDelats[wIdx] * rate;
						wDelats[wIdx] = 0; // reset the delta values for the next training iteration
					}
					
					neurons[nIdx].Bias -= neurons[nIdx].BiasDelta * rate;
					neurons[nIdx].BiasDelta = 0;
				//	neurons[nIdx].Normalize(); // some times dramatically slow downs the learning
				}
			}
		}

		private void TrainDataPairs(List<TrainingPair> pairs)
		{
			CalculateDelatW(pairs);
			UpdateWeights(pairs.Count);

			// for some activation functions the weight normalization is necessary
			// for example ReLU in hidden layers in deep networks can produce very large values
			// and the output error can easily get out of double range and produce NaN values
			//	Network.Normalize(); 
		}

		private void TrainBatch(uint iteration = 1)
		{
			Random rnd = new Random();

			var allPairs = DataSet.Pairs;

			var indexes = Utility.NumberOps.GenerateRandNonRepetitive(0, allPairs.Count - 1);
			List<TrainingPair> batch = new List<TrainingPair>();
			int index;
			for (int iter = 0; iter < iteration; iter++)
			{
				index = 0;
				while (true)
				{
					batch.Clear();
					for (int i=0; i < 10; i++)
					{
						batch.Add(allPairs[indexes[index]]);
						index++;
						if (index == indexes.Length) break;
					}

					TrainDataPairs(batch);

					var batchError = CalculateMeanError(batch);
					if (batchError <= ErrorThreshold)
						break;
					if (index == indexes.Length) break;
				}

				mMeanError = CalculateMeanError(allPairs);
				OnTrainingIteration?.Invoke(mMeanError, "mean error");
			}
		}

		private void TrainFull(uint iteration = 1)
		{
			var data = DataSet;

			for (int iter = 0; iter < iteration; iter++)
			{
				TrainDataPairs(data.Pairs);

				var error = CalculateMeanError(data.Pairs);
				OnTrainingIteration?.Invoke(error, "");
				if (error <= ErrorThreshold)
					break;
			}
		}

		public void Train(uint iteration = 1, TrainingMode mode = TrainingMode.Full, ProcessMode pMode = ProcessMode.Asynch)
		{
			mMode = mode;
			mIteration = iteration;

			if (pMode == ProcessMode.Asynch)
				Start();
			else
				ThreadFunc();
		}
	}
}
