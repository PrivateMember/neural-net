using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNet.NetworkGraphics
{
	[DataContract]
	public class NetworkVisualOptions
	{
		public bool WeightsVisible { set; get; } = false;



		public NetworkVisualOptions()
		{
			

		}
	}
}
