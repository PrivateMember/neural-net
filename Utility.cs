using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.Xml;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace NeuralNet
{
	namespace Utility
	{
		public static class FileOps
		{
			/// <summary>
			/// Writes the given object instance to a Json file.
			/// <para>Object type must have a parameterless constructor.</para>
			/// <para>Only Public properties and variables will be written to the file. These can be any type though, even other classes.</para>
			/// <para>If there are public properties/variables that you do not want written to the file, decorate them with the [JsonIgnore] attribute.</para>
			/// </summary>
			/// <typeparam name="T">The type of object being written to the file.</typeparam>
			/// <param name="filePath">The file path to write the object instance to.</param>
			/// <param name="objectToWrite">The object instance to write to the file.</param>
			/// <param name="append">If false the file will be overwritten if it already exists. If true the contents will be appended to the file.</param>
			public static void WriteToJsonFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
			{
				TextWriter writer = null;
				try
				{
					var contentsToWriteToFile = JsonConvert.SerializeObject(objectToWrite);
					writer = new StreamWriter(filePath, append);
					writer.Write(contentsToWriteToFile);
				}
				finally
				{
					if (writer != null)
						writer.Close();
				}
			}

			/// <summary>
			/// Reads an object instance from an Json file.
			/// <para>Object type must have a parameterless constructor.</para>
			/// </summary>
			/// <typeparam name="T">The type of object to read from the file.</typeparam>
			/// <param name="filePath">The file path to read the object instance from.</param>
			/// <returns>Returns a new instance of the object read from the Json file.</returns>
			public static T ReadFromJsonFile<T>(string filePath) where T : new()
			{
				TextReader reader = null;
				try
				{
					reader = new StreamReader(filePath);
					var fileContents = reader.ReadToEnd();
					return JsonConvert.DeserializeObject<T>(fileContents);
				}
				finally
				{
					if (reader != null)
						reader.Close();
				}
			}

			public static string SerializeXml<T>(T item)
			{
				DataContractSerializer ser = new DataContractSerializer(item.GetType());

				StringBuilder sb = new StringBuilder();
				XmlWriterSettings settings = new XmlWriterSettings { OmitXmlDeclaration = true, ConformanceLevel = ConformanceLevel.Fragment };
				using (XmlWriter writer = XmlWriter.Create(sb, settings))
				{
					ser.WriteObject(writer, item);
				}
				return sb.ToString();
			}

			public static T DeserializeXML<T>(string filePath)
			{
				T t;
				DataContractSerializer dcs = new DataContractSerializer(typeof(T));
				StringBuilder sb = new StringBuilder();
				XmlWriterSettings settings = new XmlWriterSettings { OmitXmlDeclaration = true, ConformanceLevel = ConformanceLevel.Fragment };
				using (XmlReader reader = XmlReader.Create(filePath))
				{
					t = (T)dcs.ReadObject(reader);
				}

				return t;
			}
		}

		public static class MemoryOps
		{
			public static Int32 Bit2Little(Int32 val)
			{
				var buffer = BitConverter.GetBytes(val);
				return (buffer[0] << 24) | (buffer[1] << 16) | (buffer[2] << 8) | buffer[3];
			}
		}

		public static class VectorOps
		{
			public static float DotProduct(float[] vec1, float[] vec2)
			{
				if (vec1 == null || vec2 == null)
					throw new ArgumentNullException();
				if (vec1.Length != vec2.Length)
					throw new ArgumentOutOfRangeException();

				float result = 0;
				for (int i = 0; i < vec1.Length; i++)
				{
					result += vec1[i] * vec2[i];
				}
				return result;
			}

			public static double DotProduct(double[] vec1, double[] vec2)
			{
				if (vec1 == null || vec2 == null)
					throw new ArgumentNullException();
				if (vec1.Length != vec2.Length)
					throw new ArgumentOutOfRangeException();

				double result = 0;
				for (int i = 0; i < vec1.Length; i++)
				{
					result += vec1[i] * vec2[i];
				}
				return result;
			}

			/// <summary>
			/// element by element multiplication of two vectors and a constant (output[i] = vec1[i] * vec2[i] * constant)
			/// it creates the output array
			/// </summary>
			/// <param name="vec1"></param>
			/// <param name="vec2"></param>
			/// <param name="constant"></param>
			/// <returns></returns>
			public static float[] Multiply(float[] vec1, float[] vec2, float constant)
			{
				if (vec1 == null || vec2 == null)
					throw new ArgumentNullException();
				if (vec1.Length != vec2.Length)
					throw new ArgumentOutOfRangeException();

				float[] output = new float[vec1.Length];
				for (int i = 0; i < vec1.Length; i++)
				{
					output[i] = vec1[i] * vec2[i] * constant;
				}
				return output;
			}

			/// <summary>
			/// element by element multiplication of two vectors and a constant (output[i] = vec1[i] * vec2[i] * constant)
			/// it does not create the output array
			/// </summary>
			/// <param name="vec1"></param>
			/// <param name="vec2"></param>
			/// <param name="constant"></param>
			/// <param name="output"></param>
			/// <returns></returns>
			public static float[] Multiply(float[] vec1, float[] vec2, float constant, float[] output)
			{
				if (vec1 == null || vec2 == null || output == null)
					throw new ArgumentNullException();
				if (vec1.Length != vec2.Length || vec1.Length != output.Length)
					throw new ArgumentOutOfRangeException();

				for (int i = 0; i < vec1.Length; i++)
				{
					output[i] = vec1[i] * vec2[i] * constant;
				}
				return output;
			}

			/// <summary>
			/// (Hadamard Product) element by element multiplication of two vectors (output[i] = vec1[i] * vec2[i])
			/// it creates the output array
			/// </summary>
			/// <param name="vec1"></param>
			/// <param name="vec2"></param>
			/// <returns></returns>
			public static float[] Multiply(float[] vec1, float[] vec2)
			{
				if (vec1 == null || vec2 == null)
					throw new ArgumentNullException();
				if (vec1.Length != vec2.Length)
					throw new ArgumentOutOfRangeException();

				float[] output = new float[vec1.Length];
				for (int i = 0; i < vec1.Length; i++)
				{
					output[i] = vec1[i] * vec2[i];
				}
				return output;
			}

			/// <summary>
			/// (Hadamard Product) element by element multiplication of two vectors (output[i] = vec1[i] * vec2[i])
			/// it does not create the output array
			/// </summary>
			/// <param name="vec1"></param>
			/// <param name="vec2"></param>
			/// <returns></returns>
			public static float[] Multiply(float[] vec1, float[] vec2, float[] output)
			{
				if (vec1 == null || vec2 == null || output == null)
					throw new ArgumentNullException();
				if (vec1.Length != vec2.Length || vec1.Length != output.Length)
					throw new ArgumentOutOfRangeException();

				for (int i = 0; i < vec1.Length; i++)
				{
					output[i] = vec1[i] * vec2[i];
				}
				return output;
			}

			/// <summary>
			/// multiplication of a vectors by a constant (output[i] = vec1[i] * constant)
			/// it creates the output array
			/// </summary>
			/// <param name="vec1"></param>
			/// <param name="vec2"></param>
			/// <returns></returns>
			public static float[] Multiply(float[] vec1, float constant)
			{
				if (vec1 == null)
					throw new ArgumentNullException();

				float[] result = new float[vec1.Length];
				for (int i = 0; i < vec1.Length; i++)
				{
					result[i] = vec1[i] * constant;
				}
				return result;
			}

			/// <summary>
			/// multiplication of a vectors by a constant (output[i] = vec1[i] * constant)
			/// it does not create the output array
			/// </summary>
			/// <param name="vec1"></param>
			/// <param name="vec2"></param>
			/// <returns></returns>
			public static float[] Multiply(float[] vec1, float constant, float[] output)
			{
				for (int i = 0; i < vec1.Length; i++)
				{
					output[i] = vec1[i] * constant;
				}
				return output;
			}

			public static double[] Multiply(double[] vec1, double[] vec2, double constant)
			{
				if (vec1 == null || vec2 == null)
					throw new ArgumentNullException();
				if (vec1.Length != vec2.Length)
					throw new ArgumentOutOfRangeException();

				double[] output = new double[vec1.Length];
				for (int i = 0; i < vec1.Length; i++)
				{
					output[i] = vec1[i] * vec2[i] * constant;
				}
				return output;
			}

			public static double[] Multiply(double[] vec1, double[] vec2)
			{
				if (vec1 == null || vec2 == null)
					throw new ArgumentNullException();
				if (vec1.Length != vec2.Length)
					throw new ArgumentOutOfRangeException();

				double[] output = new double[vec1.Length];
				for (int i = 0; i < vec1.Length; i++)
				{
					output[i] = vec1[i] * vec2[i];
				}
				return output;
			}

			public static double[] Multiply(double[] vec1, double[] vec2, double[] output)
			{
				if (vec1 == null || vec2 == null || output == null)
					throw new ArgumentNullException();
				if (vec1.Length != vec2.Length || vec1.Length != output.Length)
					throw new ArgumentOutOfRangeException();

				for (int i = 0; i < vec1.Length; i++)
				{
					output[i] = vec1[i] * vec2[i];
				}
				return output;
			}

			public static double[] Multiply(double[] vec1, double[] vec2, double constant, double[] output)
			{
				if (vec1 == null || vec2 == null || output == null)
					throw new ArgumentNullException();
				if (vec1.Length != vec2.Length || vec1.Length != output.Length)
					throw new ArgumentOutOfRangeException();

				for (int i = 0; i < vec1.Length; i++)
				{
					output[i] = vec1[i] * vec2[i] * constant;
				}
				return output;
			}

			public static double[] Multiply(double[] vec1, double constant)
			{
				if (vec1 == null)
					throw new ArgumentNullException();

				double[] result = new double[vec1.Length];
				for (int i = 0; i < vec1.Length; i++)
				{
					result[i] = vec1[i] * constant;
				}
				return result;
			}

			public static double[] Multiply(double[] vec1, double constant, double[] output)
			{
				for (int i = 0; i < vec1.Length; i++)
				{
					output[i] = vec1[i] * constant;
				}
				return output;
			}

			public static float[] Linear(float[] vec, float coef, float offset, float[] output)
			{
				for (int i = 0; i < vec.Length; i++)
				{
					output[i] = vec[i] * coef + offset;
				}
				return output;
			}

			public static float[] Linear(float[] vec, float[] coef, float[] offset, float[] output)
			{
				for (int i = 0; i < vec.Length; i++)
				{
					output[i] = vec[i] * coef[i] + offset[i];
				}
				return output;
			}

			public static float[] Linear(float[] vec, float coef, float offset)
			{
				float[] output = new float[vec.Length];
				for (int i = 0; i < vec.Length; i++)
				{
					output[i] = vec[i] * coef + offset;
				}
				return output;
			}

			public static float[] Linear(float[] vec, float[] coef, float[] offset)
			{
				float[] output = new float[vec.Length];
				for (int i = 0; i < vec.Length; i++)
				{
					output[i] = vec[i] * coef[i] + offset[i];
				}
				return output;
			}

			public static double[] Linear(double[] vec, double coef, double offset, double[] output)
			{
				for (int i = 0; i < vec.Length; i++)
				{
					output[i] = vec[i] * coef + offset;
				}
				return output;
			}

			public static double[] Linear(double[] vec, double[] coef, double[] offset, double[] output)
			{
				for (int i = 0; i < vec.Length; i++)
				{
					output[i] = vec[i] * coef[i] + offset[i];
				}
				return output;
			}

			public static double[] Linear(double[] vec, double coef, double offset)
			{
				double[] output = new double[vec.Length];
				for (int i = 0; i < vec.Length; i++)
				{
					output[i] = vec[i] * coef + offset;
				}
				return output;
			}

			public static double[] Linear(double[] vec, double[] coef, double[] offset)
			{
				double[] output = new double[vec.Length];
				for (int i = 0; i < vec.Length; i++)
				{
					output[i] = vec[i] * coef[i] + offset[i];
				}
				return output;
			}

			public static float Sum(float[] vec1)
			{
				float result = 0;
				for (int i = 0; i < vec1.Length; i++)
				{
					result += vec1[i];
				}
				return result;
			}

			public static float Magnitude(float[] vec1)
			{
				float result = DotProduct(vec1, vec1);
				return (float)Math.Sqrt(result);
			}

			public static double Magnitude(double[] vec1)
			{
				double result = DotProduct(vec1, vec1);
				return Math.Sqrt(result);
			}

			public static float[] Normalize(float[] vec1, float scale = 1)
			{
				var mag = Magnitude(vec1);
				return Multiply(vec1, scale / mag);
			}

			public static double[] Normalize(double[] vec1, double scale = 1)
			{
				var mag = Magnitude(vec1);
				return Multiply(vec1, scale / mag);
			}

			public static float[] Normalize(float[] vec1, float[] output, float scale = 1)
			{
				var mag = Magnitude(vec1);
				return Multiply(vec1, scale / mag, output);
			}

			public static double[] Normalize(double[] vec1, double[] output, double scale = 1)
			{
				var mag = Magnitude(vec1);
				return Multiply(vec1, scale / mag, output);
			}

			public static float[] Add(float[] vec1, float[] vec2)
			{
				if (vec1 == null || vec2 == null)
					throw new ArgumentNullException();
				if (vec1.Length != vec2.Length)
					throw new ArgumentOutOfRangeException();

				float[] result = new float[vec1.Length];
				for (int i = 0; i < vec1.Length; i++)
				{
					result[i] = vec1[i] + vec2[i];
				}
				return result;
			}

			public static float[] Add(float[] vec1, float[] vec2, float[] output)
			{
				if (vec1 == null || vec2 == null || output == null)
					throw new ArgumentNullException();
				if (vec1.Length != vec2.Length || vec1.Length != output.Length)
					throw new ArgumentOutOfRangeException();

				for (int i = 0; i < vec1.Length; i++)
				{
					output[i] = vec1[i] + vec2[i];
				}
				return output;
			}

			public static double[] Add(double[] vec1, double[] vec2)
			{
				if (vec1 == null || vec2 == null)
					throw new ArgumentNullException();
				if (vec1.Length != vec2.Length)
					throw new ArgumentOutOfRangeException();

				double[] result = new double[vec1.Length];
				for (int i = 0; i < vec1.Length; i++)
				{
					result[i] = vec1[i] + vec2[i];
				}
				return result;
			}

			public static double[] Add(double[] vec1, double[] vec2, double[] output)
			{
				if (vec1 == null || vec2 == null || output == null)
					throw new ArgumentNullException();
				if (vec1.Length != vec2.Length || vec1.Length != output.Length)
					throw new ArgumentOutOfRangeException();

				for (int i = 0; i < vec1.Length; i++)
				{
					output[i] = vec1[i] + vec2[i];
				}
				return output;
			}

			public static float[] Add(float[] vec1, float constant)
			{
				if (vec1 == null)
					throw new ArgumentNullException();

				float[] result = new float[vec1.Length];
				for (int i = 0; i < vec1.Length; i++)
				{
					result[i] = vec1[i] + constant;
				}
				return result;
			}

			public static float[] Add(float[] vec1, float constant, float[] output)
			{
				if (vec1 == null)
					throw new ArgumentNullException();

				for (int i = 0; i < vec1.Length; i++)
				{
					output[i] = vec1[i] + constant;
				}
				return output;
			}

			public static double[] Add(double[] vec1, double constant)
			{
				if (vec1 == null)
					throw new ArgumentNullException();

				double[] result = new double[vec1.Length];
				for (int i = 0; i < vec1.Length; i++)
				{
					result[i] = vec1[i] + constant;
				}
				return result;
			}

			public static double[] Add(double[] vec1, double constant, double[] output)
			{
				if (vec1 == null)
					throw new ArgumentNullException();

				for (int i = 0; i < vec1.Length; i++)
				{
					output[i] = vec1[i] + constant;
				}
				return output;
			}

			public static float[] Subtract(float[] vec1, float[] vec2)
			{
				if (vec1 == null || vec2 == null)
					throw new ArgumentNullException();
				if (vec1.Length != vec2.Length)
					throw new ArgumentOutOfRangeException();

				float[] result = new float[vec1.Length];
				for (int i = 0; i < vec1.Length; i++)
				{
					result[i] = vec1[i] - vec2[i];
				}
				return result;
			}

			public static float[] Subtract(float[] vec1, float[] vec2, float[] output)
			{
				if (vec1 == null || vec2 == null)
					throw new ArgumentNullException();
				if (vec1.Length != vec2.Length)
					throw new ArgumentOutOfRangeException();

				for (int i = 0; i < vec1.Length; i++)
				{
					output[i] = vec1[i] - vec2[i];
				}
				return output;
			}

			public static double[] Subtract(double[] vec1, double[] vec2)
			{
				if (vec1 == null || vec2 == null)
					throw new ArgumentNullException();
				if (vec1.Length != vec2.Length)
					throw new ArgumentOutOfRangeException();

				double[] result = new double[vec1.Length];
				for (int i = 0; i < vec1.Length; i++)
				{
					result[i] = vec1[i] - vec2[i];
				}
				return result;
			}

			public static double[] Subtract(double[] vec1, double[] vec2, double[] output)
			{
				if (vec1 == null || vec2 == null)
					throw new ArgumentNullException();
				if (vec1.Length != vec2.Length)
					throw new ArgumentOutOfRangeException();

				for (int i = 0; i < vec1.Length; i++)
				{
					output[i] = vec1[i] - vec2[i];
				}
				return output;
			}

			public static float MeanSquared(float[] vec)
			{
				float result = 0;
				for (int i = 0; i < vec.Length; i++)
				{
					result += vec[i] * vec[i];
				}
				return 0.5f * result / vec.Length;
			}

			public static double MeanSquared(double[] vec)
			{
				double result = 0;
				for (int i = 0; i < vec.Length; i++)
				{
					result += vec[i] * vec[i];
				}
				return 0.5 * result / vec.Length;
			}
		}

		public static class ImageOps
		{
			public static Bitmap BitmapFromPixelArray(byte[] pixels, int rows, int cols)
			{
				var b = new Bitmap(rows, cols, PixelFormat.Format8bppIndexed);

				ColorPalette ncp = b.Palette;
				for (int i = 0; i < 256; i++)
					ncp.Entries[i] = Color.FromArgb(255, i, i, i);
				b.Palette = ncp;

				var BoundsRect = new Rectangle(0, 0, rows, cols);
				BitmapData bmpData = b.LockBits(BoundsRect,
												ImageLockMode.WriteOnly,
												b.PixelFormat);

				IntPtr ptr = bmpData.Scan0;

				int index = 0;
				for (int r = 0; r < rows; r++)
				{
					Marshal.Copy(pixels, index, ptr, cols);
					index += cols;
					ptr += bmpData.Stride;
				}

				b.UnlockBits(bmpData);
				return b;
			}
		}

		public static class NumberOps
		{
			public static int[] GenerateRandNonRepetitive(int min, int max)
			{
				if (max < min) throw new ArgumentOutOfRangeException("max must be >= than min");
				int[] result = new int[max - min + 1];

				for (int i = 0; i < result.Length; i++)
					result[i] = i;

				var half = result.Length / 2;
				Random rnd = new Random();
				for (int i = 0; i < half; i++)
				{
					var idx1 = rnd.Next(0, result.Length - 1);
					var idx2 = rnd.Next(0, result.Length - 1);
					var val = result[idx1];
					result[idx1] = result[idx2];
					result[idx2] = val;
				} 

				return result;
			}
		}
	}
}
