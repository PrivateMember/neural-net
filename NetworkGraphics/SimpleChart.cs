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
	public class SimpleChart
	{
		public enum ChartMode
		{
			Normal,
			FullWidth
		}

		Panel mPanel = null;
		Bitmap mBitmapOrig = null;
		Graphics mGraphics = null;
		double mMaxValue = 0.1;
		double mMinValue = 0;
		Pen mPen1 = new Pen(Brushes.White);
		Pen mPen2 = new Pen(Brushes.Red, 2);
		Pen mPenGuide = new Pen(Brushes.Gray, 2) { DashStyle = DashStyle.Dash };
		float mPreX, mPreY;
		int mFrameHight;
		int mFrameWidth;
		Font mFont = new Font("Arial", 20);
		List<double> mValues = new List<double>();

		public int FrameMarginX { set; get; } = 100;
		public int FrameMarginY { set; get; } = 20;
		public ChartMode Mode { set; get; } = ChartMode.FullWidth;
		// Graphic Objects

		public SimpleChart(Panel panel)
		{
			mPanel = panel;
			mBitmapOrig = new Bitmap(1920, 280, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
			mPanel.BackgroundImage = mBitmapOrig;
			mPanel.BackgroundImageLayout = ImageLayout.Stretch;
			mGraphics = Graphics.FromImage(mBitmapOrig);
			mGraphics.SmoothingMode = SmoothingMode.AntiAlias;
			mGraphics.Clear(Color.Black);

			mPen2.Width = 2;
			mPen1.Width = 2;
		}

		public void AddValue(double value)
		{
			mValues.Add(value);
			if (value > mMaxValue) mMaxValue = value;
			else if (value < mMinValue) mMinValue = value;
		}

		public void AddValue(double[] values)
		{
			mValues.AddRange(values);
			foreach(var v in values)
			{
				if (v > mMaxValue) mMaxValue = v;
				else if (v < mMinValue) mMinValue = v;
			}
		}

		public void Clear()
		{
			mValues.Clear();
			mGraphics.Clear(Color.Black);
			DrawAxis();
			mPanel.Refresh();
		}

		private void DrawAxis()
		{
			mFrameHight = mBitmapOrig.Height - 2 * FrameMarginY;
			mFrameWidth = mBitmapOrig.Width - 2 * FrameMarginX;
			float X0 = FrameMarginX;
			float Y0 = mBitmapOrig.Height / 2;

			mPreX = X0;
			mPreY = Y0;

			mGraphics.DrawLine(mPen1, FrameMarginX, FrameMarginY, FrameMarginX, FrameMarginY + mFrameHight);
			mGraphics.DrawLine(mPen1, FrameMarginX, Y0, FrameMarginX + mFrameWidth, Y0);
		}

		private void DrawValuesNormal()
		{
			if (mValues.Count == 0) return;
			int sampleRate = 1;
			int frameWidth = (mBitmapOrig.Width - 2 * FrameMarginX);
			float hRatio = (float)(0.5 * (mBitmapOrig.Height - 2 * FrameMarginY) / mMaxValue);
			float Y0 = mBitmapOrig.Height / 2;
			//	float xScale = 1;// (mBitmapOrig.Width - 2 * FrameMarginX) / (float)count;

			while ((mValues.Count / sampleRate) >= frameWidth)
			{
				sampleRate++;
			}


			var x1 = FrameMarginX + 2;
			var y1 = Y0;
			for (int i = 0; i < mValues.Count; i += sampleRate)
			{
				y1 = (float)(Y0 - hRatio * mValues[i]);
				mGraphics.DrawLine(mPen2, mPreX, mPreY, x1, y1);
				mPreX = x1;
				mPreY = y1;
				x1++;
			}

			// draw the guid line
			//var y = (float)(Y0 - hRatio * mMaxValue);
			mGraphics.DrawLine(mPenGuide, FrameMarginX, y1, x1 - 1, y1);
			mGraphics.DrawLine(mPenGuide, x1 - 1, Y0, x1 - 1, y1);

			mGraphics.DrawString(mValues[mValues.Count-1].ToString("0.####"), mFont, Brushes.Red, FrameMarginX - 100, y1);
			mGraphics.DrawString(mValues.Count.ToString(), mFont, Brushes.Red, x1 - 1, Y0 + 10);
		}

		private void DrawValuesFullW()
		{
			if (mValues.Count == 0) return;
			int sampleRate = 1;
			int count = mValues.Count / sampleRate;
			float hRatio = (float)(0.5 * (mBitmapOrig.Height - 2 * FrameMarginY) / mMaxValue);
			float X0 = FrameMarginX + 1;
			float Y0 = mBitmapOrig.Height / 2;
			float xScale = (mBitmapOrig.Width - 2 * FrameMarginX) / (float)count;
			if (xScale <= 1)
			{
				sampleRate = (int)(0.5 + 1 / xScale);
				xScale = (mBitmapOrig.Width - 2 * FrameMarginX) / (float)count;
			}
			Pen pen = new Pen(Brushes.Red);
			pen.Width = 2;
			var x1 = X0;
			for (int i = 0; i < mValues.Count; i += sampleRate)
			{
				var y1 = (float)(Y0 - hRatio * mValues[i]);

				mGraphics.DrawLine(pen, x1, y1, x1 + xScale, y1);

				x1 += xScale;
			}
		}

		public void DrawValues()
		{
			switch(Mode)
			{
				case ChartMode.FullWidth:
					DrawValuesFullW();
					break;
				case ChartMode.Normal:
					DrawValuesNormal();
					break;
			}
		}

		public void Draw()
		{
			mGraphics.Clear(Color.Black);
			DrawAxis();
			DrawValues();
			mPanel.Refresh();
		}
	}
}
