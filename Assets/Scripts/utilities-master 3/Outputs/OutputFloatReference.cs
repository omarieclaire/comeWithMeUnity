using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputFloatReference : MonoBehaviour
{
	public FloatReference FloatReference;
	public bool round;
	public FloatEvent OutputFloat;
	public void Output(){
		float output = FloatReference.Value;
		if(round)
			output = Mathf.Round(output);
		OutputFloat.Invoke(output);
	}
	public void Set(float input){
		FloatReference.Value = input;
	}
	public void InputInt(int input){
		FloatReference.Value = input;
	}
}
