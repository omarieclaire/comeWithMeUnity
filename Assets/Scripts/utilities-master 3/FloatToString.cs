using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FloatToString : MonoBehaviour
{
	public StringEvent stringEvent;
	public string prepend;
	public string append;
	public bool round;
	public void FloatInput(float input){
		if(round)
			input = Mathf.Round(input);
		string str = prepend + input.ToString() + append;
		stringEvent.Invoke(str);
	}
	public void IntInput(int input){
		string str = prepend + input.ToString() + append;
		stringEvent.Invoke(str);
	}
}
