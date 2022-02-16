using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNet
{
	public enum ActivationMethod
	{
		/// <summary>
		/// (Linear) when using this activation functiopn it is better to normalize the input data to the range 0-1 before starting the training
		/// </summary>
		Identity,
		/// <summary>
		/// Logistic
		/// </summary>
		Sigmoid,
		/// <summary>
		/// (Rectified Linear Activation) when using this activation functiopn it is better to normalize the input data to the range 0-1 before starting the training
		/// </summary>
		ReLU,
		/// <summary>
		/// Hyperbolic Tangent
		/// </summary>
		TanH,
		/// <summary>
		/// Output Activation Function. This function is for the output layer. It generates a vector. The vector's elemnts sum to 1.0
		/// </summary>
		Softmax,
		/// <summary>
		/// (Leaky Rectified Linear Activation) when using this activation functiopn it is better to normalize the input data to the range 0-1 before starting the training
		/// </summary>
		LeakyReLU
	}

	[DataContract]
	public class ActivationFunction
	{
		private delegate double ICalculate(double x);

		[DataMember]
		public ActivationMethod Method { set; get; } = ActivationMethod.Sigmoid;
		[DataMember]
		public double LeakyReLUCoef { set; get; } = 0.1f;
		public ActivationFunction()
		{
		}

		public ActivationFunction(ActivationMethod method)
		{
			Method = method;
		}

		/// <summary>
		/// calculates the function and the derivative of the function
		/// </summary>
		/// <param name="input"></param>
		/// <returns> the return value is a 2-elemnt Tuple. item1 is the function output and item2 is the derivative </returns>
		public virtual Tuple<double, double> Calculate(double input)
		{
			double value = 0, valueD = 0;

			switch (Method)
			{
				case ActivationMethod.Identity:
					value = input;
					valueD = 1;
					break;
				case ActivationMethod.Sigmoid:
					value = Sigmoid(input);
					valueD = value * (1 - value);
					break;
				case ActivationMethod.ReLU:
					//	value = ReLU(input);
					if (input <= 0)
					{
						value = 0;
						valueD = 0;
					}
					else
					{
						value = input;
						valueD = 1;
					}
					break;
				case ActivationMethod.LeakyReLU:
					//	value = ReLU(input);
					if (input <= 0)
					{
						value = LeakyReLUCoef * input;
						valueD = LeakyReLUCoef;
					}
					else
					{
						value = input;
						valueD = 1;
					}
					break;
				case ActivationMethod.TanH:
					value = TanH(input);
					valueD = 1 - value * value;
					break;
			}
			return new Tuple<double, double>(value, valueD);
		}

		public double[] Calculate(double[] input)
		{
			double[] result = null;
			switch (Method)
			{
				case ActivationMethod.Identity:
					result = new double[input.Length];
					input.CopyTo(result, 0);
					break;
				case ActivationMethod.Sigmoid: result = CalculateVector(input, Sigmoid); break;
				case ActivationMethod.ReLU: result = CalculateVector(input, ReLU); break;
				case ActivationMethod.TanH: result = CalculateVector(input, TanH); break;
				case ActivationMethod.Softmax: result = Softmax(input); break;
				case ActivationMethod.LeakyReLU: result = CalculateVector(input, LeakyReLU); break;
			}
			return result;
		}


		public double[] Derivative(double[] input)
		{
			double[] result = null;
			//Derivative
			return result;
		}

		private double[] CalculateVector(double[] x, ICalculate func)
		{
			double[] result = new double[x.Length];

			for (int i = 0; i < x.Length; i++)
			{
				result[i] = func(x[i]);
			}

			return result;
		}

		public static double Sigmoid(double x)
		{
			double exp = (double)Math.Exp(-x);
			return 1 / (1 + exp);
		}

		public static double TanH(double x)
		{
			double expP = (double)Math.Exp(x);
			double expN = (double)Math.Exp(-x);
			return (expP - expN) / (expP + expN);
		}

		private double ReLU(double x)
		{
			return Math.Max(0, x);
		}

		private double LeakyReLU(double x)
		{
			if (x > 0) return x;
			else return 0.1f * x;
		}

		private double[] Softmax(double[] xArr)
		{
			double[] expArr = new double[xArr.Length];
			double sum = 0;
			for (int i = 0; i < xArr.Length; i++)
			{
				expArr[i] = (double)Math.Exp(xArr[i]);
				sum += expArr[i];
			}
			sum = 1 / sum;
			for (int i = 0; i < expArr.Length; i++)
			{
				expArr[i] *= sum;
			}

			return expArr;
		}
	}
}
