using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringToFloat : MonoBehaviour
{
	public StringReference StringReference;
	
	public FloatEvent OutputFloat;
	
	public void SetStringValue(string input){
		StringReference.Value = input;
	}
	public void Output(){
		float output = float.Parse(StringReference.Value);
		OutputFloat.Invoke(output);
	}
}
