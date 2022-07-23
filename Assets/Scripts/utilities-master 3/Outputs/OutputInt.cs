using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputInt : MonoBehaviour
{
	public IntReference IntReference;
	public IntEvent OutputIntEvent;
	public void Output(){
		OutputIntEvent.Invoke(IntReference.Value);
	}
	public void Set(int input){
		IntReference.Value = input;
	}
	public void InputFloat(float input){
		IntReference.Value = Mathf.RoundToInt(input);
	}
	public void Increment(int amount){
		IntReference.Value+=amount;
	}
}
