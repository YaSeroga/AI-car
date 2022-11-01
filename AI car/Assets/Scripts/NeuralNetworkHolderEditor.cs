using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NeuralNetworkHolder))]
public class NeuralNetworkHolderEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		if (GUILayout.Button("Setup"))
		{
			((NeuralNetworkHolder) target).Setup();
		}
		if (GUILayout.Button("Serialize"))
		{
			((NeuralNetworkHolder) target).OnBeforeSerialize();
		}
	}
}