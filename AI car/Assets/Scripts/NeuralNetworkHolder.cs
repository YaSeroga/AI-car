using Newtonsoft.Json;
using UnityEngine;

[CreateAssetMenu(menuName = "Create NeuralLinkHolder", fileName = "NeuralLinkHolder", order = 0)]
public class NeuralNetworkHolder : ScriptableObject, ISerializationCallbackReceiver
{
	[SerializeField] private int[] _layers;
	[SerializeField, TextArea] private string _serializedNetwork;
	[SerializeField] private int _counter = 0;

	public int Counter
	{
		get => _counter;
		set => _counter = value;
	}

	private NeuralNetwork _neuralNetwork = new NeuralNetwork();

	public string SerializedNetwork => _serializedNetwork;

	public NeuralNetwork NeuralNetwork
	{
		get => _neuralNetwork;
		set => _neuralNetwork = value;
	}

	public void Setup()
	{
		_neuralNetwork.Layers = _layers;
		_neuralNetwork.Setup();
	}

	public void OnBeforeSerialize()
	{
		_serializedNetwork = JsonConvert.SerializeObject(_neuralNetwork);
	}

	public void OnAfterDeserialize()
	{
		_neuralNetwork = JsonConvert.DeserializeObject<NeuralNetwork>(_serializedNetwork);
	}
}