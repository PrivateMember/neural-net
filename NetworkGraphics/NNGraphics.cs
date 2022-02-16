using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;

namespace NeuralNet.NetworkGraphics
{
	public class NNGraphics : GameWindow
	{
		public NNGraphics() :
			base(GameWindowSettings.Default, NativeWindowSettings.Default)
		{
			this.CenterWindow();
		}

		protected override void OnUpdateFrame(FrameEventArgs args)
		{
			base.OnUpdateFrame(args);

			
		}

		protected override void OnRenderFrame(FrameEventArgs args)
		{
			
			GL.Clear(ClearBufferMask.ColorBufferBit);

			this.Context.SwapBuffers();

			base.OnRenderFrame(args);
		}

		protected override void OnLoad()
		{
			GL.ClearColor(Color4.Black);

			base.OnLoad();
		}

		protected override void OnUnload()
		{
			base.OnUnload();
		}

		protected override void OnResize(ResizeEventArgs e)
		{
			GL.Viewport(0, 0, e.Width, e.Height);
			base.OnResize(e);
		}

		protected override void OnKeyDown(KeyboardKeyEventArgs e)
		{
			base.OnKeyDown(e);

			if(e.Key == Keys.Escape)
			{
				this.Close();
			}
		}
	}
}
