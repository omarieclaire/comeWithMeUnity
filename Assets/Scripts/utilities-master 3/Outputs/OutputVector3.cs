using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputVector3 : MonoBehaviour
{
	public Vector3Reference Vector3Reference;
	public Vector3Event Vector3Event;
	public void SetVector3(Vector3 input){
		
		Vector3Reference.Value = input;
	}
	public void Output(){
		//Debug.Log(Vector3Reference.Value,gameObject);
		Vector3Event.Invoke(Vector3Reference.Value);
	}
}
