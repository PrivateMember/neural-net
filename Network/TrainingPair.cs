using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNet.Network
{
	[DataContract]
	public class TrainingPair
	{
		[DataMember]
		public double[] Inputs { set; get; } = null;
		[DataMember]
		public double[] Outputs { set; get; } = null;

		public static TrainingPair FromXML(string path)
		{
			var result = Utility.FileOps.DeserializeXML<TrainingPair>(path);
			return result;
		}
		public void Save(string path)
		{
			string xml = Utility.FileOps.SerializeXml<TrainingPair>(this);
			System.IO.File.WriteAllText(path, xml);
		}

		public TrainingPair Clone()
		{
			var result = new TrainingPair();
			result.Inputs = (double[])this.Inputs.Clone();
			result.Outputs = (double[])this.Outputs.Clone();
			return result;
		}
	}
}
