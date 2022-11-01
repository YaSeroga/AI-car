using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SimulationSpawner : MonoBehaviour
{
	[SerializeField] private NeuralNetworkHolder _holder;
	[SerializeField] private Car _carPrefab;
	[SerializeField] private Transform _parkingPrefab;
	[SerializeField] private TMP_Text _counter;
	[SerializeField] private string _counterFormat;
	[Space]
	[SerializeField] private int _tickRate = 5;
	[SerializeField] private int _clonesCount = 10;
	[SerializeField] private float _oneTestDuration = 10;

	[SerializeField] private float _biasMutation = 0.05f;
	[SerializeField] private float _weightMutation = 0.05f;
	[SerializeField] private bool _speedUp;
	
	private Transform _parkingInstance;
	private Car[] _carsInstances;
	
	private void Awake()
	{
		StartCoroutine(TrainNeuralNetwork());
	}

	private IEnumerator TrainNeuralNetwork()
	{
		while (true)
		{
			if (_speedUp)
			{
				for (int i = 0; i < 100; i++)
				{

					SpawnObjects();
					foreach (Car car in _carsInstances)
					{
						for (int j = 0; j < _oneTestDuration / Time.deltaTime; j++)
						{
							if (j % 100 == 0)
							{
								car.RecalculateInput(_parkingInstance.transform.position);
							}

							car.Update();
						}
					}

					if (i % 20 == 0)
						yield return null;

					FindBestInstance();
					DisposeObjects();
				}

			}
			else
			{
				SpawnObjects();

				for (float i = 0; i < _oneTestDuration; i += 1f / _tickRate)
				{
					foreach (Car car in _carsInstances)
					{
						car.RecalculateInput(_parkingInstance.transform.position);
					}
					yield return new WaitForSeconds(1f / _tickRate);
				}

				FindBestInstance();
				DisposeObjects();
				yield return new WaitForSeconds(0.5f);
			}

			yield return null;
		}
	}

	private void SpawnObjects()
	{
		float x = UnityEngine.Random.Range(-10f, 10f);
		float z = UnityEngine.Random.Range(-10f, 10f);
		Vector3 parkingPosition = new Vector3(x, 0.05f, z);
		
		_parkingInstance = Instantiate(_parkingPrefab, parkingPosition, Quaternion.identity);
		
		_carsInstances ??= new Car[_clonesCount];
		if (_carsInstances.Length != _clonesCount)
			_carsInstances = new Car[_clonesCount];
		
		for (int i = 0; i < _carsInstances.Length; i++)
		{
			_carsInstances[i] = Instantiate(_carPrefab);
			_carsInstances[i].SetNeuralNetwork(_holder.NeuralNetwork.GetMutatedCopy(_biasMutation, _weightMutation));
		}
	}

	private void FindBestInstance()
	{
		float bestFitness = -1;
		foreach (Car car in _carsInstances)
		{
			float fitness = car.GetFitness();
			if (bestFitness > fitness) continue;
			
			bestFitness = fitness;
			_holder.NeuralNetwork = car.Network;
		}

		_holder.Counter++;
		_counter.text = string.Format(_counterFormat, _holder.Counter);
		Debug.Log("Fitness = " + bestFitness.ToString());

	}

	private void DisposeObjects()
	{
		Destroy(_parkingInstance.gameObject);
		foreach (Car car in _carsInstances)
		{
			Destroy(car.gameObject);
		}
	}
}