using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncrementFloat : MonoBehaviour
{
	public float IncrementAmount;
	
	public void Increment(float input){
		input+=IncrementAmount;
		OutputFloat.Invoke(input);
	}
	
	public FloatEvent OutputFloat;
}
