using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class NeuralNetwork
{
	public int[] Layers;
	public float[][] Biases;
	public float[][] Neurons;
	public float[][,] Weights;
	[HideInInspector] public int[] Activations;

	private float _fitness;

	public float Fitness
	{
		get => _fitness;
		set => _fitness = value;
	}

	public float[] FeedForward(float[] inputs)
	{
		for (int i = 0; i < inputs.Length; i++)
			Neurons[0][i] = inputs[i];

		for (int i = 1; i < Layers.Length; i++)
		{
			int previousLayer = i - 1;
			for (int j = 0; j < Layers[i]; j++)
			{
				float neuronRawValue = 0f;
				for (int k = 0; k < Layers[previousLayer]; k++)
				{
					neuronRawValue += Weights[previousLayer][k, j] * Neurons[previousLayer][k];
				}

				Neurons[i][j] = Activate(neuronRawValue + Biases[i][j]);
			}
		}
		return Neurons[^1];
	}

	public void Setup()
	{
		SetupBiases();
		SetupNeurons();
		SetupWeights();
	}

	private float Activate(float value)
	{
		return value;
		// return (float) Math.Tanh(value);
	}

	private void SetupWeights()
	{
		Weights = new float[Layers.Length - 1][,];
		for (int i = 0; i < Layers.Length - 1; i++)
		{
			Weights[i] = new float[Layers[i], Layers[i + 1]];
			for (int j = 0; j < Weights[i].GetLength(0); j++)
			{
				for (int k = 0; k < Weights[i].GetLength(1); k++)
				{
					Weights[i][j, k] = UnityEngine.Random.Range(-.5f, .5f);
				}
			}
		}
	}

	private void SetupNeurons()
	{
		Neurons = new float[Layers.Length][];
		for (int i = 0; i < Layers.Length; i++)
		{
			Neurons[i] = new float[Layers[i]];
		}
	}

	private void SetupBiases()
	{
		Biases = new float[Layers.Length][];
		for (int i = 0; i < Layers.Length; i++)
		{
			Biases[i] = new float[Layers[i]];
			for (int j = 0; j < Biases[i].Length; j++)
			{
				Biases[i][j] = UnityEngine.Random.Range(-.5f, .5f);
			}
		}
	}

	public NeuralNetwork GetMutatedCopy(float biasMaxMutation, float weightMaxMutation)
	{
		NeuralNetwork copy = new NeuralNetwork();
		copy.Layers = new int[Layers.Length];
		for (var i = 0; i < Layers.Length; i++)
		{
			copy.Layers[i] = Layers[i];
		}
		copy.Biases = new float[Biases.Length][];
		for (int i = 0; i < Biases.Length; i++)
		{
			copy.Biases[i] = new float[Biases[i].Length];
			for (int j = 0; j < Biases[i].Length; j++)
			{
				copy.Biases[i][j] = Biases[i][j];
			}

		}
		copy.Neurons = new float[Neurons.Length][];
		for (int i = 0; i < Neurons.Length; i++)
		{
			copy.Neurons[i] = new float[Neurons[i].Length];
			for (int j = 0; j < Neurons[i].Length; j++)
			{
				copy.Neurons[i][j] = Neurons[i][j];
			}

		}
		
		copy.Weights = new float[Weights.Length][,];
		for (int i = 0; i < Weights.Length; i++)
		{
			int d0 = Weights[i].GetLength(0);
			int d1 = Weights[i].GetLength(1);
			copy.Weights[i] = new float[d0, d1];
			for (int j = 0; j < d0; j++)
			{
				for (int k = 0; k < d1; k++)
				{
					copy.Weights[i][j, k] = Weights[i][j, k];
				}
			}
		}
		
		for (int i = 0; i < Biases.Length; i++)
		{
			for (int j = 0; j < Biases[i].Length; j++)
			{
				copy.Biases[i][j] += UnityEngine.Random.Range(-biasMaxMutation, biasMaxMutation);
			}
		}

		for (int i = 0; i < Weights.Length; i++)
		{
			for (int j = 0; j < Weights[i].GetLength(0); j++)
			{
				for (int k = 0; k < Weights[i].GetLength(1); k++)
				{
					copy.Weights[i][j, k] += UnityEngine.Random.Range(-weightMaxMutation, weightMaxMutation);
				}
			}
		}

		return copy;
	}
}