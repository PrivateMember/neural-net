using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNet.Games
{
	public class Coordinate
	{
		public int X { set; get; } = 0;
		public int Y { set; get; } = 0;

		public void CopyTo(Coordinate obj)
		{
			obj.X = X;
			obj.Y = Y;
		}

		public void Add(Coordinate c)
		{
			X += c.X;
			Y += c.Y;
		}

		public Coordinate Clone()
		{
			return new Coordinate() { X = this.X, Y = this.Y };
		}

		public bool IsEqual(Coordinate cord)
		{
			return (cord.X == X && cord.Y == Y);
		}
	}
}
