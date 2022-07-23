using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AverageFloatList : MonoBehaviour
{
	public FloatEvent outputAverage;
	
	public void Input(List<float> input){
		float total = 0;
		foreach(float f in input){
			total += f;
		}
		total = total/input.Count;
		//
		outputAverage.Invoke(total);
	}
}
