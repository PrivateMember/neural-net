using NeuralNet.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNet
{
	[DataContract]
	public class Neuron
	{
		[DataMember]
		public double Bias { set; get; } = 1;
		[DataMember]
		private ActivationFunction mActFunc;
		[DataMember]
		public double[] Weights { set; get; }

		private double mActValue;
		public double[] WeightsDelta { set; get; }
		public double BiasDelta { set; get; } = 0;
		public int InputSize { get { return Weights.Length; } }
		public double Activation { get { return mActValue; } }
		public ActivationFunction ActFunction { get { return mActFunc; } }

		public Neuron(uint inputSize, ActivationFunction actf)
		{
			mActFunc = actf;
			Weights = new double[inputSize];
			WeightsDelta = new double[inputSize];
		}

		public void InitMemory()
		{
			if(WeightsDelta == null)
				WeightsDelta = new double[Weights.Length];
		}

		public void ResetDeltaW()
		{
			if (WeightsDelta != null)
				WeightsDelta.Initialize();
			BiasDelta = 0;
		}

		/// <summary>
		/// 
		/// </summary>
		public void RandomizeWeights(Random rnd)
		{
			for (int i = 0; i < Weights.Length; i++)
			{
				Weights[i] = (double)(rnd.NextDouble() - 0.5) * 0.6;
			}

			Bias = (double)(rnd.NextDouble() - 0.5) * 2;
		}

		public void RandomizeWeightsNormalized(Random rnd, double scale = 1)
		{
			RandomizeWeights(rnd);
			Normalize(scale);
		}

		public void Normalize(double scale = 1)
		{
			var temp = new double[Weights.Length + 1];
			Weights.CopyTo(temp, 0);
			temp[temp.Length - 1] = Bias;
			Utility.VectorOps.Normalize(temp, temp, scale);
			Bias = temp[temp.Length - 1];
			Array.Copy(temp, Weights, Weights.Length);
		}

		/// <summary>
		/// kills an input connection (weight). it sets the weight to zero. W(i) = 0
		/// </summary>
		/// <param name="index">the index of the input weight to be set to zero</param>
		public void CutInput(int index)
		{
			if (index < Weights.Length)
				Weights[index] = 0;
		}

		public Tuple<double, double> Calculate(double[] input)
		{
			mActValue = Bias + VectorOps.DotProduct(Weights, input);
			return mActFunc.Calculate(mActValue);
		}
	}
}
