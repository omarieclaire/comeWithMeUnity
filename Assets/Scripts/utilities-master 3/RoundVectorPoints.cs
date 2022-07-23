using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundVectorPoints : MonoBehaviour
{
	public int Scale;
	public Vector3Event OutputVector3;
	
	
	public void InputVector(Vector3 input){
		Vector3 output = new Vector3(Mathf.Round(input.x * Scale)/Scale,Mathf.Round(input.y * Scale)/Scale,Mathf.Round(input.z * Scale)/Scale);
		//Debug.Log(output,gameObject);
		OutputVector3.Invoke(output);
	}
	
}
