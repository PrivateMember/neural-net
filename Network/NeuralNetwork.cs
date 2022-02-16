using NeuralNet.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNet
{
	[DataContract]
	public class NeuralNetwork
	{
		[DataMember]
		private uint[] mLayerSizes;
		[DataMember]
		public List<NetLayer> Layers { set; get; }
		public uint[] LayerSizes { get { return mLayerSizes.ToArray(); } }
		public uint OutputSize { get { return mLayerSizes[mLayerSizes.Length - 1]; } }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="layersSizes"> this array must include the input layer, hidden layers, and output layer sizes</param>
		public NeuralNetwork(uint[] layersSizes, ActivationMethod hiddenActF, ActivationMethod outputActF, int seed = 1)
		{
			if (layersSizes == null || layersSizes.Length < 2) throw new Exception("invalid layer sizes");
			
			mLayerSizes = (uint[])layersSizes.Clone();

			Layers = new List<NetLayer>();

			Layers.Add(new NetLayer(layersSizes[0], layersSizes[0], new ActivationFunction(ActivationMethod.Identity), true));

			for (int i = 1; i < layersSizes.Length - 1; i++)
			{
				Layers.Add(new NetLayer(layersSizes[i], layersSizes[i - 1], new ActivationFunction(hiddenActF)));
			}

			int index = layersSizes.Length - 1;
			Layers.Add(new NetLayer(layersSizes[index], layersSizes[index - 1], new ActivationFunction(outputActF)));

			RandomizeWeights();
		}

		/// <summary>
		/// ????????????????
		/// </summary>
		/// <param name="index"></param>
		public void RemoveLayer(uint index)
		{

		}

		public void DisableNeuron(uint layerIndex, uint neuronIndex)
		{

		}

		public void Normalize(double scale = 1)
		{
			foreach(var l in Layers)
			{
				l.Normalize(scale);
			}
		}

		public void KillNeuron(uint layerIndex, uint neuronIndex)
		{

		}

		public void DisableRandomNeuron()
		{

		}

		public void KillRandomNeuron()
		{

		}

		public void ResetDeltaW()
		{
			foreach (var l in Layers)
				l.ResetDeltaW();
		}

		public void RandomizeWeights()
		{
			Random rnd = new Random();
			foreach(var l in Layers)
			{
				l.RandomizeWeights(rnd);
			}
		}

		public double[] Process(double[] input)
		{
			var output = (double[])input.Clone();
			for (int i =0;  i < Layers.Count; i++)
			{
				output = Layers[i].CalculateOutput(output);
			}

			return output;
		}

		private void InitMemory()
		{
			foreach (var l in Layers)
				l.InitMemory();
		}

		public static NeuralNetwork LoadFromFile(string path)
		{
			var result = Utility.FileOps.DeserializeXML<NeuralNetwork>(path);
			result.InitMemory();
			return result;
		}

		public void SaveToFile(string path)
		{
			string xml = Utility.FileOps.SerializeXml<NeuralNetwork>(this);
			if (!path.Contains(".nnt"))
				path += ".nnt";
			System.IO.File.WriteAllText(path, xml);
		}
	}
}
