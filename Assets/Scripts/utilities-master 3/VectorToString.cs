using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorToString : MonoBehaviour
{
	public StringEvent OutputString;
	public Vector3Event OutputVector3;
	
	public void InputVector(Vector3 input){
		string output = input.ToString();
		//Debug.Log(output);
		OutputString.Invoke(output);
	}
	
	public void InputString(string input){
		//Debug.Log("V3 " +input);
		Vector3 output = stringToVec(input);
		OutputVector3.Invoke(output);
	}
	
	public Vector3 stringToVec(string s) {
		//Debug.Log("string " + s);
		string[] temp = s.Substring (1, s.Length-2).Split (',');
		return new Vector3 (float.Parse(temp[0]), float.Parse(temp[1]), float.Parse(temp[2]));
	}

	
}
