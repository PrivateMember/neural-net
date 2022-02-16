using NeuralNet.Network;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NeuralNet.NetworkGraphics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;

namespace NeuralNet
{
	public partial class MainForm : Form
	{
		NeuralNetwork mNetwork;
		NetworkTrainer mTrainer;
		TrainingSet mDataSet;
		NetworkVisualizer mVisual;
		SimpleChart mErrChart;
		private GameWindow gameWindow;

		public MainForm()
		{
			InitializeComponent();

			cbxHidLayerActF.DataSource = Enum.GetValues(typeof(ActivationMethod));
			cbxOutLayerActF.DataSource = Enum.GetValues(typeof(ActivationMethod));
			this.DoubleBuffered = true;

			tbxDimensions.Text = "2,2,1";

			mVisual = new NetworkVisualizer(splitContainer1.Panel1);
			mVisual.LayerDistance = (int)nud_LayerD.Value;
			mVisual.FrameMargin = (int)nud_FrameM.Value;
			mVisual.NeuronMarginX = (int)nud_NrnXmargin.Value;
			mVisual.NeuronMarginY = (int)nud_NrnYmargin.Value;
			mVisual.NeuronMaxHieght = (int)nud_NrnMaxSize.Value;

			mErrChart = new SimpleChart(splitContainer1.Panel2);


			mVisual.NeuronVerticalSpacing = (int)nud_Vspace.Value;

			mDataSet = new TrainingSet();

			mTrainer = new NetworkTrainer();
			mTrainer.OnTrainingIteration += MTrainer_OnTrainingIteration;
			mTrainer.OnBatchComplete += MTrainer_OnBatchComplete;
			mTrainer.OnFinish += MTrainer_OnFinish;

			CreateNetwork(new uint[] { 2, 2, 1 }, ActivationMethod.ReLU, ActivationMethod.Identity);

			cbx_chartFW.Checked = mErrChart.Mode == SimpleChart.ChartMode.FullWidth;
			cbx_chartFW.CheckedChanged += cbx_chartFW_CheckedChanged;
		}

		private void MTrainer_OnFinish(string msg)
		{
			if (this.InvokeRequired)
			{
				// Call this same method but append THREAD2 to the text
				Action safeWrite = delegate { MTrainer_OnFinish(msg); };
				this.Invoke(safeWrite);
			}
			else
			{
				btnTrain1.Enabled = true;
			}

			UpdateNetworkVisual();
		}

		private void MTrainer_OnBatchComplete(double[] value)
		{
			//	rtxbLog.AppendText("error = " + string.Join(",", value) + "\r\n");
			//	rtxbLog.SelectionStart = rtxbLog.Text.Length;
			//	rtxbLog.ScrollToCaret();
		}

		private void MTrainer_OnTrainingIteration(double value, string msg)
		{
			UpdateErrorVisualSafe(value);
		}

		public void UpdateNetworkVisual()
		{
			if (this.InvokeRequired)
			{
				// Call this same method but append THREAD2 to the text
				Action safeWrite = delegate { UpdateNetworkVisual(); };
				this.Invoke(safeWrite);
			}
			else
			{
				mVisual.DrawAll();
				mVisual.Update();
			}
		}

		public void UpdateErrorVisualSafe(double value)
		{
			if (this.InvokeRequired)
			{
				// Call this same method but append THREAD2 to the text
				Action safeWrite = delegate { UpdateErrorVisualSafe(value); };
				this.Invoke(safeWrite);
			}
			else
			{
				tbxError.Text = value.ToString("0.#####");
				mErrChart.AddValue(value);
				mErrChart.Draw();

				//	rtxbLog.AppendText("error = " + value.ToString() + "\r\n");
				//	rtxbLog.SelectionStart = rtxbLog.Text.Length;
				//	rtxbLog.ScrollToCaret();
			}
		}

		private void CreateNetwork(uint[] layerSizes, ActivationMethod hiddenActF, ActivationMethod outputActF)
		{
			int seed = int.Parse(tbxSeed.Text);

			mNetwork = new NeuralNetwork(layerSizes, hiddenActF, outputActF, seed);
			mVisual.Network = mNetwork;

			UpdateNetworkVisual();

			mErrChart.Clear();
			mErrChart.Draw();
		}

		private void btnProcess_Click(object sender, EventArgs e)
		{
			var dataArr = tbxTestData.Text.Split(new char[] { ',' });
			double[] input = new double[dataArr.Length];

			for (int i = 0; i < input.Length; i++)
			{
				input[i] = double.Parse(dataArr[i]);
			}

			var output = mNetwork.Process(input);

			UpdateNetworkVisual();
		}

		private void btnRandomize_Click(object sender, EventArgs e)
		{
			mNetwork.RandomizeWeights();
			UpdateNetworkVisual();
		}

		private void btnTanH_Click(object sender, EventArgs e)
		{
			double input = double.Parse(tbxInputF.Text);
			tbxOutputF.Text = ActivationFunction.TanH(input).ToString();
		}

		private void btnTrain1_Click(object sender, EventArgs e)
		{
			btnTrain1.Enabled = false;
			mTrainer.Network = mNetwork;
			mTrainer.DataSet = mDataSet;
			mTrainer.LearningRate = double.Parse(tbxLearnR.Text);
			mTrainer.ErrorThreshold = double.Parse(tbxErrorTHr.Text);
			mTrainer.Train((uint)nud_Iter.Value, TrainingMode.Full);
		}

		private void btnCreate_Click(object sender, EventArgs e)
		{
			var strArr = tbxDimensions.Text.Split(new char[] { ',' });
			uint[] arr = new uint[strArr.Length];

			for (int i = 0; i < strArr.Length; i++)
			{
				arr[i] = uint.Parse(strArr[i]);
			}



			ActivationMethod actH, actO;
			Enum.TryParse<ActivationMethod>(cbxHidLayerActF.SelectedValue.ToString(), out actH);
			Enum.TryParse<ActivationMethod>(cbxOutLayerActF.SelectedValue.ToString(), out actO);

			CreateNetwork(arr, actH, actO);
		}

		private void aveImageToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveFileDialog dlg = new SaveFileDialog();

			if (dlg.ShowDialog() == DialogResult.OK)
			{
				mVisual.SaveToFile(dlg.FileName);
			}
		}

		private void nud_LayerD_ValueChanged(object sender, EventArgs e)
		{
			mVisual.LayerDistance = (int)nud_LayerD.Value;
			UpdateNetworkVisual();
		}

		private void nud_FrameM_ValueChanged(object sender, EventArgs e)
		{
			mVisual.FrameMargin = (int)nud_FrameM.Value;
			UpdateNetworkVisual();
		}

		private void nud_Vspace_ValueChanged(object sender, EventArgs e)
		{
			mVisual.NeuronVerticalSpacing = (int)nud_Vspace.Value;
			UpdateNetworkVisual();
		}

		private void nud_NrnXoffset_ValueChanged(object sender, EventArgs e)
		{
			mVisual.NeuronMarginX = (int)nud_NrnXmargin.Value;
			UpdateNetworkVisual();
		}

		private void nud_NrnYmargin_ValueChanged(object sender, EventArgs e)
		{
			mVisual.NeuronMarginY = (int)nud_NrnYmargin.Value;
			UpdateNetworkVisual();
		}

		private void nud_NrnMinSize_ValueChanged(object sender, EventArgs e)
		{
			mVisual.NeuronMaxHieght = (int)nud_NrnMaxSize.Value;
			UpdateNetworkVisual();
		}

		private void saveNetworkToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveFileDialog dlg = new SaveFileDialog();
			dlg.Filter = "neural network file (*.nnt)|*.nnt";

			if (dlg.ShowDialog() == DialogResult.OK)
			{
				mNetwork.SaveToFile(dlg.FileName);
			}
		}

		private void loadNetworkToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Filter = "neural network file (*.nnt)|*.nnt";

			if (dlg.ShowDialog() == DialogResult.OK)
			{
				mNetwork = NeuralNetwork.LoadFromFile(dlg.FileName);

				mVisual.Network = mNetwork;
				UpdateNetworkVisual();
			}
		}

		private void ShowDataSet()
		{
			tbxTrIn.Text = "";
			tbxTrOut.Text = "";

			foreach (var data in mDataSet.Pairs)
			{
				tbxTrIn.Text += string.Join(",", data.Inputs) + "\r\n";
				tbxTrOut.Text += string.Join(",", data.Outputs) + "\r\n";
			}
		}

		private void GenerateDataFromUserInput(string inputData, string outputData)
		{
			mDataSet.Clear();
			var inputLines = inputData.Split(new string[] { "\r\n" }, StringSplitOptions.None);
			var outputLines = outputData.Split(new string[] { "\r\n" }, StringSplitOptions.None);

			if (inputLines.Length != outputLines.Length)
			{
				MessageBox.Show("invalid data size");
				return;
			}

			for (int i = 0; i < inputLines.Length; i++)
			{
				var inputStrArr = inputLines[i].Split(new char[] { ',' });
				var outputStrArr = outputLines[i].Split(new char[] { ',' });

				double[] input = new double[inputStrArr.Length];
				double[] output = new double[outputStrArr.Length];

				for (int j = 0; j < input.Length; j++)
				{
					input[j] = double.Parse(inputStrArr[j]);
				}
				for (int j = 0; j < output.Length; j++)
				{
					output[j] = double.Parse(outputStrArr[j]);
				}

				mDataSet.Add(new TrainingPair() { Inputs = input, Outputs = output });
			}
		}

		private void loadTrainingDataToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();

			//	dlg.InitialDirectory = "c:\\";
			dlg.Filter = "training data file (*.tda)|*.tda";

			if (dlg.ShowDialog() == DialogResult.OK)
			{
				mDataSet = TrainingSet.LoadFromFile(dlg.FileName);
				ShowDataSet();
			}
		}

		private void saveTrainingDataToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveFileDialog dlg = new SaveFileDialog();
			dlg.Filter = "training data file (*.tda)|*.tda";

			if (dlg.ShowDialog() == DialogResult.OK)
			{
				mDataSet.SaveToFile(dlg.FileName);
			}
		}

		private void btnGenerateData_Click(object sender, EventArgs e)
		{
			GenerateDataFromUserInput(tbxTrIn.Text, tbxTrOut.Text);
		}

		private void btnSigmoid_Click(object sender, EventArgs e)
		{
			double input = double.Parse(tbxInputF.Text);
			tbxOutputF.Text = ActivationFunction.Sigmoid(input).ToString();
		}

		private void btnClearChart_Click(object sender, EventArgs e)
		{
			mErrChart.Clear();
		}

		private void nud_ChartXmargin_ValueChanged(object sender, EventArgs e)
		{
			mErrChart.FrameMarginX = (int)nud_ChartXmargin.Value;
			mErrChart.Draw();
		}

		private void nud_ChartYmargin_ValueChanged(object sender, EventArgs e)
		{
			mErrChart.FrameMarginY = (int)nud_ChartYmargin.Value;
			mErrChart.Draw();
		}

		private void cbx_chartFW_CheckedChanged(object sender, EventArgs e)
		{
			mErrChart.Mode = cbx_chartFW.Checked ? NetworkGraphics.SimpleChart.ChartMode.FullWidth : NetworkGraphics.SimpleChart.ChartMode.Normal;
			mErrChart.Draw();
		}

		private void btnNormalize_Click(object sender, EventArgs e)
		{
			mNetwork.Normalize(1);

			UpdateNetworkVisual();
		}

		private void showWeightsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			mVisual.ShowWeights(true);
		}

		private void hideWeightsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			mVisual.ShowWeights(false);
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();

			//	dlg.InitialDirectory = "c:\\";
			dlg.Filter = "training data file (*.idx3-ubyte)|*.idx3-ubyte";

			if (dlg.ShowDialog() == DialogResult.OK)
			{
				var filePath = dlg.FileName.Replace(dlg.SafeFileName, "");
				var labelsFile = filePath + "train-labels.idx1-ubyte";

				FileStream fsImages = new FileStream(dlg.FileName, FileMode.Open);
				BinaryReader brImages = new BinaryReader(fsImages);

				FileStream fsLabels = new FileStream(labelsFile, FileMode.Open);
				BinaryReader brLabels = new BinaryReader(fsLabels);

				Int32 magic = Utility.MemoryOps.Bit2Little(brImages.ReadInt32());
				Int32 count = Utility.MemoryOps.Bit2Little(brImages.ReadInt32());
				Int32 rows = Utility.MemoryOps.Bit2Little(brImages.ReadInt32());
				Int32 cols = Utility.MemoryOps.Bit2Little(brImages.ReadInt32());

				brLabels.ReadInt32();
				brLabels.ReadInt32();

				//	MessageBox.Show(magic.ToString() + "\r\n" + count.ToString() + "\r\n" + rows.ToString() + "\r\n" + cols.ToString());


				var buffer = brImages.ReadBytes(rows * cols);
				var image = Utility.ImageOps.BitmapFromPixelArray(buffer, rows, cols);
				picBoxDataSet0.Image = image;
				lblDataSet0.Text = brLabels.ReadByte().ToString();

				buffer = brImages.ReadBytes(rows * cols);
				image = Utility.ImageOps.BitmapFromPixelArray(buffer, rows, cols);
				picBoxDataSet1.Image = image;
				lblDataSet1.Text = brLabels.ReadByte().ToString();

				buffer = brImages.ReadBytes(rows * cols);
				image = Utility.ImageOps.BitmapFromPixelArray(buffer, rows, cols);
				picBoxDataSet2.Image = image;
				lblDataSet2.Text = brLabels.ReadByte().ToString();

				buffer = brImages.ReadBytes(rows * cols);
				image = Utility.ImageOps.BitmapFromPixelArray(buffer, rows, cols);
				picBoxDataSet3.Image = image;
				lblDataSet3.Text = brLabels.ReadByte().ToString();

				buffer = brImages.ReadBytes(rows * cols);
				image = Utility.ImageOps.BitmapFromPixelArray(buffer, rows, cols);
				picBoxDataSet4.Image = image;
				lblDataSet4.Text = brLabels.ReadByte().ToString();

				buffer = brImages.ReadBytes(rows * cols);
				image = Utility.ImageOps.BitmapFromPixelArray(buffer, rows, cols);
				picBoxDataSet5.Image = image;
				lblDataSet5.Text = brLabels.ReadByte().ToString();

				buffer = brImages.ReadBytes(rows * cols);
				image = Utility.ImageOps.BitmapFromPixelArray(buffer, rows, cols);
				picBoxDataSet6.Image = image;
				lblDataSet6.Text = brLabels.ReadByte().ToString();

				buffer = brImages.ReadBytes(rows * cols);
				image = Utility.ImageOps.BitmapFromPixelArray(buffer, rows, cols);
				picBoxDataSet7.Image = image;
				lblDataSet7.Text = brLabels.ReadByte().ToString();

				buffer = brImages.ReadBytes(rows * cols);
				image = Utility.ImageOps.BitmapFromPixelArray(buffer, rows, cols);
				picBoxDataSet8.Image = image;
				lblDataSet8.Text = brLabels.ReadByte().ToString();

				buffer = brImages.ReadBytes(rows * cols);
				image = Utility.ImageOps.BitmapFromPixelArray(buffer, rows, cols);
				picBoxDataSet9.Image = image;
				lblDataSet9.Text = brLabels.ReadByte().ToString();

				brImages.Close();
				fsImages.Close();
				brLabels.Close();
				fsLabels.Close();
			}
		}

		private void testToolStripMenuItem_Click(object sender, EventArgs e)
		{
			//	ConvolutionForm form = new ConvolutionForm();
			//	form.ShowDialog();

			Bitmap bmp = new Bitmap(100, 100);
			Graphics gf = Graphics.FromImage(bmp);
			Font font = new Font(FontFamily.GenericSansSerif, 10.0f);
			var a = gf.MeasureString("255", font);
			MessageBox.Show(a.ToString());
		}

		private void openTKToolStripMenuItem_Click(object sender, EventArgs e)
		{
			NNGraphics nng = new NNGraphics();
			nng.Run();
		}

		private void snakeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FormGameSnake form = new FormGameSnake();

			form.ShowDialog();
		}
	}
}
