using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNet.Network
{
	[DataContract]
	public class TrainingSet
	{
		[DataMember]
		private List<TrainingPair> mPairs;

		public List<TrainingPair> Pairs
		{ 
			set { mPairs = value; }
			get { return mPairs; }
		}

		public TrainingSet()
		{
			mPairs = new List<TrainingPair>();
		}

		public static TrainingSet LoadFromFile(string path)
		{
			return Utility.FileOps.DeserializeXML<TrainingSet>(path);
		}

		public void Clear()
		{
			mPairs.Clear();
		}

		public void SaveToFile(string path)
		{
			string xml = Utility.FileOps.SerializeXml<TrainingSet>(this);
			System.IO.File.WriteAllText(path, xml);
		}

		public void Add(TrainingPair pair)
		{
			mPairs.Add(pair);
		}
	}
}
