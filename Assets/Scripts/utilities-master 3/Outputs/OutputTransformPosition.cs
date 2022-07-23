using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputTransformPosition : MonoBehaviour
{
	public Transform TransformToOutput{
		get {
			return _output;
		}
		set {
			_output = value;
		}
	}
	[SerializeField]
	private Transform _output;
	
	public Vector3Event vector3Event;
	public void Output(){
		//Debug.Log(TransformToOutput.position);
		vector3Event.Invoke(TransformToOutput.position);
	}
}
