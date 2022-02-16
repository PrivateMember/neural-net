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
	public partial class ConvolutionForm : Form
	{
		Bitmap mImageMain;
		Bitmap mImageKernel;
		Bitmap mImageFeature;
		Bitmap mImageKernelR;
		Bitmap mImageKernelG;
		Bitmap mImageKernelB;


		public ConvolutionForm()
		{
			InitializeComponent();

			mImageMain = new Bitmap(314, 314, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
			mImageFeature = new Bitmap(314, 314, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
			mImageKernel = new Bitmap(62, 62, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
			mImageKernelR = new Bitmap(62, 62, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
			mImageKernelG = new Bitmap(62, 62, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
			mImageKernelB = new Bitmap(62, 62, System.Drawing.Imaging.PixelFormat.Format24bppRgb);


			pBoxImage.Image = mImageMain;
			pBoxKernel.Image = mImageKernel;
			pBoxKernelR.Image = mImageKernelR;
			pBoxKernelG.Image = mImageKernelG;
			pBoxKernelB.Image = mImageKernelB;
			pBoxFeatureMap.Image = mImageFeature;

			DarwLines();
		}

		private void DarwLines()
		{
			Graphics gfx = Graphics.FromImage(mImageMain);
			Pen penLine = new Pen(Color.White, 1);

			int x1, y1, x2, y2;

			y1 = 0;
			y2 = mImageMain.Height - 1;
			for (int i = 0; i < 15; i++)
			{
				x1 = x2 = 21 * i;
				gfx.DrawLine(penLine, x1, y1, x2, y2);
			}

			x1 = 0;
			x2 = mImageMain.Width - 1;
			for (int i = 0; i < 15; i++)
			{
				y1 = y2 = 21 * i;
				gfx.DrawLine(penLine, x1, y1, x2, y2);
			}

			pBoxImage.Update();
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Filter = "neural network file (*.jpg)|*.jpg";

			if (dlg.ShowDialog() == DialogResult.OK)
			{
				mImageMain = new Bitmap(dlg.FileName);
				pBoxImage.Image = mImageMain;
			}
		}

		private void btnReset_Click(object sender, EventArgs e)
		{

		}

		private void btnAuto_Click(object sender, EventArgs e)
		{

		}

		private void btnNext_Click(object sender, EventArgs e)
		{

		}
	}
}
