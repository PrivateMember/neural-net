using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using NeuralNet.Utility;

namespace NeuralNet
{
	[DataContract]
	public class NetLayer
	{
		[DataMember]
		private uint mSize;
		[DataMember]
		private uint mInputSize;
		[DataMember]
		private bool mIsInput = false;
		[DataMember]
		private BitArray mNeuronsEnable;
		[DataMember]
		private Neuron[] mNeurons;
		[DataMember]
		private ActivationFunction mActFunc;

		private double[] mOutput;
		private double[] mOutputD;

		/// <summary>
		/// this field is used when the object is loaded from the XML file ti initilize the fields that are not included in the XML
		/// </summary>
		private bool mIsInitialized = false;
		public Neuron[] Neurons { get { return mNeurons; } }
		public double[] Outputs { get { return mOutput; } }
		public double[] OutputsD { get { return mOutputD; } }
		public int Size { get { return mNeurons.Length; } }
		public bool IsInput { get { return mIsInput; } }
		

		public NetLayer(uint layerSize, uint inputSize, ActivationFunction actF, bool isInput = false)
		{
			mSize = layerSize;
			mInputSize = inputSize;
			mActFunc = actF;
			mIsInput = isInput;

			InitMemory();
		}

		public void InitMemory()
		{
			if (mNeurons == null)
			{
				mNeurons = new Neuron[mSize];

				for (int i = 0; i < mSize; i++)
				{
					if (mIsInput)
					{
						mNeurons[i] = new Neuron(1, mActFunc);
					}
					else
					{
						mNeurons[i] = new Neuron(mInputSize, mActFunc);
					}
				}
			}
			else
			{
				foreach (var n in mNeurons)
				{
					n.InitMemory();
				}
			}
			if (mNeuronsEnable == null)
				mNeuronsEnable = new BitArray((int)mSize, true);
			if (mOutput == null)
				mOutput = new double[mSize];
			if (mOutputD == null)
				mOutputD = new double[mSize];
			mIsInitialized = true;
		}

		public void RandomizeWeights(Random rnd)
		{
			foreach (Neuron n in mNeurons)
			{
				//n.RandomizeWeightsNormalized(rnd);
				n.RandomizeWeights(rnd);
			}
		}

		public void ResetDeltaW()
		{
			foreach (var n in mNeurons)
				n.ResetDeltaW();
		}

		public double[] CalculateOutput(double[] input)
		{
			// check for the input array size and throw exceptions if required !!!!!!!!!
			if(input.Length != mInputSize)
			{
				throw new ArgumentException();
			}
			// continue
			if (mIsInput)
			{
				for (int i = 0; i < mSize; i++)
				{
					mOutput[i] = input[i];
				}
			}
			else
			{
				for (int i = 0; i < mSize; i++)
				{
					if (mNeuronsEnable[i])
					{
						var result = mNeurons[i].Calculate(input);
						mOutput[i] = result.Item1;
						mOutputD[i] = result.Item2;
					}
					else
					{
						mOutput[i] = 0;
						mOutputD[i] = 0;
					}
				}
			}

			return mOutput;
		}

		public void NeuronEnable(int index)
		{
			mNeuronsEnable.Set(index, true);
		}

		public void NeuronDisbale(int index)
		{
			mNeuronsEnable.Set(index, false);
		}

		public bool IsNeuronEnabled(int index)
		{
			return mNeuronsEnable.Get(index);
		}

		public void RemoveNeuronRandomly()
		{

		}
	
		public void Normalize(double scale = 1)
		{
			foreach(var n in mNeurons)
			{
				n.Normalize();
			}
		}
	}
}
