namespace NeuralNet.Network
{
	public delegate void Message(string msg);
	public delegate void MessageValue(double value, string msg);
	public delegate void MessageErrorVector(double[] value);
}