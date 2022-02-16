using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace NeuralNet.NetworkGraphics
{
	public class NetworkVisualizer
	{
		Panel mPanel = null;
		public NeuralNetwork Network { set; get; } = null;
		Bitmap mBitmapOrig = null;
		Graphics mGraphics = null;
		bool mWeightsVisible = true;
		Pen mPenLine = new Pen(Brushes.White) { Width = 2 };
		Pen mPenBold = new Pen(Brushes.Yellow) { Width = 5 };
		Font mDrawFont = new Font("Arial", 20);

		public NetworkVisualOptions mOptions { set; get; }
		public int LayerDistance { set; get; } = 200;
		public int FrameMargin { set; get; } = 50;
		public int NeuronVerticalSpacing { set; get; } = 10;
		public int NeuronMarginX { set; get; } = 100;
		public int NeuronMarginY { set; get; } = 20;
		public int NeuronMaxHieght { set; get; } = 100;
		public bool WeightsVisible { get { return mWeightsVisible; } }
		public bool InpOutVisible { get; set; }

		public NetworkVisualizer(Panel panel)
		{
			mPanel = panel;
			mBitmapOrig = new Bitmap(1920, 1080, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
			mPanel.BackgroundImage = mBitmapOrig;
			mPanel.BackgroundImageLayout = ImageLayout.Stretch;
			mGraphics = Graphics.FromImage(mBitmapOrig);
			mGraphics.SmoothingMode = SmoothingMode.AntiAlias;
			mGraphics.Clear(Color.Black);
		}

		public NetworkVisualizer(NeuralNetwork nn, Panel panel)
		{
			mPanel = panel;
			Network = nn;
			mBitmapOrig = new Bitmap(1920, 1080, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
			mPanel.BackgroundImage = mBitmapOrig;
			mGraphics = Graphics.FromImage(mBitmapOrig);
			mGraphics.SmoothingMode = SmoothingMode.AntiAlias;
			mGraphics.Clear(Color.Black);
		}

		public void Clear()
		{
			mGraphics.Clear(Color.Black);
		}

		public void ShowWeights(bool state)
		{
			if (state != mWeightsVisible)
			{
				mWeightsVisible = state;
				DrawAll();
				Update();
			}
		}

		public void DrawAll()
		{
			if (Network == null) return;

			var layersNeuronCount = Network.LayerSizes;
			int maxlayerNeuronCount = (int)layersNeuronCount.Max();
			int frameHeight = mBitmapOrig.Height - FrameMargin * 2;
			int frameWidth = mBitmapOrig.Width - FrameMargin * 2;
			int neuronH = (frameHeight - (NeuronMarginY * 2) - (maxlayerNeuronCount - 1) * NeuronVerticalSpacing) / maxlayerNeuronCount;
			neuronH = Math.Min(neuronH, NeuronMaxHieght);
			int neuronW = neuronH;
			int neuronUintH = neuronH + NeuronVerticalSpacing;
			int layerWidth = neuronW + NeuronMarginX * 2;
			int maxH = maxlayerNeuronCount * neuronUintH - NeuronVerticalSpacing + NeuronMarginY * 2;
			
			int neuronX, neuronY;
			int neuronX_1, neuronY_1;

			var layers = Network.Layers;
			int[] layersH = new int[layers.Count];
			int[] layerY0 = new int[layers.Count];
			int[] layerX0 = new int[layers.Count];

			string str;
			
			mGraphics.Clear(Color.Black);
			for (int layerIdx = 0; layerIdx < layers.Count; layerIdx++)
			{
				var neurons = layers[layerIdx].Neurons;
				var layerOutputs = layers[layerIdx].Outputs;
			//	var layerActVals = layers[layerIdx].Activations;

				mPenLine.Color = Color.FromArgb(255, 255, 255);

				layersH[layerIdx] = maxH - (maxlayerNeuronCount - layers[layerIdx].Size) * neuronUintH;
				layerY0[layerIdx] = FrameMargin + (maxH - layersH[layerIdx]) / 2;
				layerX0[layerIdx] = FrameMargin + layerIdx * (LayerDistance + layerWidth);

				// draw the area of the current layer 
				mGraphics.DrawRectangle(mPenLine, layerX0[layerIdx], layerY0[layerIdx], layerWidth, layersH[layerIdx]);

				neuronX = layerX0[layerIdx] + NeuronMarginX;

				// draw all neuron of the current layer
				for (int j = 0; j < neurons.Length; j++)
				{
					neuronY = layerY0[layerIdx] + neuronUintH * j + NeuronMarginY;
					//mGraphics.DrawEllipse(pen, new Rectangle(neuronX, neuronY, mNeuronW, mNeuronH));
					mGraphics.DrawRectangle(mPenLine, new Rectangle(neuronX, neuronY, neuronW, neuronH));


					int dY = (int)(layerOutputs[j] * neuronH);
					int mY = neuronY + neuronH - dY;
					mGraphics.FillRectangle(Brushes.White, new Rectangle(neuronX, mY, neuronW, dY));


					// the input activation value green line
					mGraphics.DrawLine(mPenBold, neuronX - NeuronMarginX, neuronY + neuronH / 2, neuronX, neuronY + neuronH / 2);
					

					// the output activation function green line
					mGraphics.DrawLine(mPenBold, neuronX + neuronW, neuronY + neuronH / 2, neuronX + neuronW + NeuronMarginX, neuronY + neuronH / 2);

					if (InpOutVisible)
					{
						mGraphics.DrawString(
										neurons[j].Activation.ToString("0.###"),
										mDrawFont,
										neurons[j].Activation >= 0 ? Brushes.Green : Brushes.Red,
										neuronX - NeuronMarginX + 1, neuronY + neuronH / 2 + 5);

						mGraphics.DrawString(
										neurons[j].Bias.ToString("0.###"),
										mDrawFont,
										neurons[j].Bias >= 0 ? Brushes.Green : Brushes.Red,
										neuronX - NeuronMarginX + 1, neuronY + neuronH / 2 - 40);
						mGraphics.DrawString(
										layerOutputs[j].ToString("0.###"),
										mDrawFont,
										Brushes.Green,
										neuronX + neuronW + 1, neuronY + neuronH / 2 + 5);
					}
				}

				// draw the connections
				int colorInt, colorIntR, colorIntG;
				int deltaX, deltaY;
				if (layerIdx != 0) // not for the input layer
				{
					for (int j = 0; j < neurons.Length; j++)
					{
						neuronX = layerX0[layerIdx];
						neuronY = layerY0[layerIdx] + neuronUintH * j + NeuronMarginY + neuronH / 2;

						neuronX_1 = layerX0[layerIdx - 1] + layerWidth;
						for (int k = 0; k < layers[layerIdx - 1].Neurons.Length; k++)
						{
							if (neurons[j].Weights[k] != 0)
							{
								colorInt = Math.Min((int)(Math.Abs(neurons[j].Weights[k]) * 200) + 55, 255);
								colorInt = colorInt < 0 ? 0 : colorInt;
								if (neurons[j].Weights[k] > 0)
								{
									colorIntR = 0;
									colorIntG = colorInt;
								}
								else
								{
									colorIntR = colorInt;
									colorIntG = 0;
								}

								neuronY_1 = layerY0[layerIdx - 1] + neuronUintH * k + NeuronMarginY + neuronH / 2;
								mPenLine.Color = Color.FromArgb(colorIntR, colorIntG, 0);
							//	mGraphics.DrawLine(mPenLine, neuronX_1, neuronY_1, neuronX, neuronY);
								DrawCurvedConnection(new PointF(neuronX_1, neuronY_1), new PointF(neuronX, neuronY));

								deltaX = neuronX - neuronX_1;
								deltaY = neuronY - neuronY_1;
								str = neurons[j].Weights[k].ToString("0.###");
								SizeF textSize = mGraphics.MeasureString(str, mDrawFont);

								if (mWeightsVisible)
								{
									mGraphics.DrawString(
										str,
										mDrawFont,
										neurons[j].Weights[k] > 0 ? Brushes.Green : Brushes.Red,
										neuronX_1 + deltaX / 4, neuronY_1 + deltaY / 4);
								}
							}
						}
					}
				}
			}
		}

		private void DrawCurvedConnection(PointF p1, PointF p2)
		{
			float deltaY = Math.Abs(p2.Y - p1.Y) * 0.1f;
			if (p1.Y < p2.Y) deltaY *= -1;
			PointF[] points = new PointF[5];
			var p0 = new PointF((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
			points[0] = p1;
			points[4] = p2;
			points[2] = p0;
			points[1] = new PointF((p0.X + p1.X) / 2, (p0.Y + p1.Y) / 2 + deltaY);
			points[3] = new PointF((p0.X + p2.X) / 2, (p0.Y + p2.Y) / 2 - deltaY);

			mGraphics.DrawCurve(mPenLine, points, 0.5f);
		}

		public void ShowConnections()
		{

		}

		public void HideConnections()
		{

		}

		public void Update()
		{
		//	mPanel.BackgroundImage = mBitmapOrig;
			mPanel.BackgroundImageLayout = ImageLayout.Stretch;
			mPanel.Refresh();
		}

		public void SaveToFile()
		{
			SaveFileDialog dlg = new SaveFileDialog();

			if (dlg.ShowDialog() == DialogResult.OK)
			{
				SaveToFile(dlg.FileName);
			}
		}

		public void SaveToFile(string path)
		{
			mBitmapOrig.Save(path);
		}
	}
}
