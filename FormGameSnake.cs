using NeuralNet.Games;
using NeuralNet.Network;
using NeuralNet.NetworkGraphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NeuralNet
{
	public partial class FormGameSnake : Form
	{
		GameSnake mGame = new GameSnake(8);
		NetworkVisualizer mVisual;
		Timer mTimer;
		int mMaxLength = 0;
		public FormGameSnake()
		{
			InitializeComponent();

			mGame.Trainer.OnTrainingIteration += Trainer_OnTrainingIteration;
			mTimer = new Timer();
			mTimer.Tick += MTimer_Tick;
			mVisual = new NetworkVisualizer(mGame.Brain, panel1);
			mVisual.LayerDistance = 150;
			mVisual.InpOutVisible = false;
			mVisual.FrameMargin = 25;
			mVisual.NeuronMarginX = 75;
			mVisual.ShowWeights(false);

			pictureBox1.Image = mGame.Image;
		}

		private void Trainer_OnTrainingIteration(double value, string msg)
		{
			labelMeanErr.Text = "Mean Error : " + value.ToString("#.00000");
		}

		private void MTimer_Tick(object sender, EventArgs e)
		{
			UpdateGame();
		}

		private void btnMove_Click(object sender, EventArgs e)
		{
			UpdateGame();
		}

		private void btnAuto_Click(object sender, EventArgs e)
		{
			mTimer.Interval = 100;
			if (mTimer.Enabled)
				mTimer.Stop();
			else
				mTimer.Start();
		}

		private void UpdateGame()
		{
			mGame.Move();
			mMaxLength = Math.Max(mMaxLength, mGame.Length);
			UpdateVisuals();
		}

		private void UpdateVisuals()
		{
			labelMaxL.Text = "Max Length : " + mMaxLength.ToString();
			labelDataL.Text = "Data Length : " + mGame.TrainingData.Pairs.Count.ToString();
			labelMoveCount.Text = "Moves : " + mGame.MoveCount.ToString();
			pictureBox1.Image = mGame.Image;
			pictureBox1.Update();
			mVisual.DrawAll();
			mVisual.Update();
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			SaveFileDialog dlg = new SaveFileDialog();
			dlg.Filter = "neural network file (*.nnt)|*.nnt";

			if (dlg.ShowDialog() == DialogResult.OK)
			{
				mGame.Brain.SaveToFile(dlg.FileName);
			}
		}

		private void btnLoad_Click(object sender, EventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Filter = "neural network file (*.nnt)|*.nnt";

			if (dlg.ShowDialog() == DialogResult.OK)
			{
				mGame.Brain = NeuralNetwork.LoadFromFile(dlg.FileName);
				mVisual.Network = mGame.Brain;
				mVisual.DrawAll();
				mVisual.Update();
			}
		}

		private void btnLoadTrainData_Click(object sender, EventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();

			//	dlg.InitialDirectory = "c:\\";
			dlg.Filter = "training data file (*.tda)|*.tda";

			if (dlg.ShowDialog() == DialogResult.OK)
			{
				mGame.TrainingData = TrainingSet.LoadFromFile(dlg.FileName);
			}
		}

		private void btnSaveTrainData_Click(object sender, EventArgs e)
		{
			SaveFileDialog dlg = new SaveFileDialog();
			dlg.Filter = "training data file (*.tda)|*.tda";

			if (dlg.ShowDialog() == DialogResult.OK)
			{
				mGame.TrainingData.SaveToFile(dlg.FileName);
			}
		}
	}
}
