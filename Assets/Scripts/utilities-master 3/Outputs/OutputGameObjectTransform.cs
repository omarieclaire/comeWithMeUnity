using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputGameObjectTransform : MonoBehaviour
{
	public GameObject GameObjectToOutput {
		get{
			return _output;
		}
		set{
			//Debug.Log("setting go",gameObject);
			_output = value;
		}
	}
	[SerializeField]
	private GameObject _output;
	
	public TransformEvent OutputTransform;
	public void Output(){
		OutputTransform.Invoke(GameObjectToOutput.transform);
	}
}
