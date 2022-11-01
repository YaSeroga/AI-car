using UnityEngine;

public class Car : MonoBehaviour
{
	private NeuralNetwork _network;

	private Vector2 _input;
	private Vector3 _target;

	public NeuralNetwork Network => _network;

	public void SetNeuralNetwork(NeuralNetwork network)
	{
		_network = network;
	}

	public float GetFitness()
	{
		return 1 / (_target - transform.position).magnitude;
	}

	public void RecalculateInput(Vector3 targetPos)
	{
		_target = targetPos;
		Vector3 targetDirection = targetPos - transform.position;
		float[] inputs = {transform.rotation.y, targetDirection.x, targetDirection.y};
		float[] output = _network.FeedForward(inputs);

		_input.x = output[0];
		_input.y = output[1];	
	}

	public void Update()
	{
		transform.Rotate(0, _input.x * Time.deltaTime, 0);
		transform.position += transform.forward * _input.y  * Time.deltaTime;
	}
}