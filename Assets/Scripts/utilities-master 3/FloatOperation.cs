using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatOperation : MonoBehaviour
{
	public enum Operation {plus,minus,divide,multiply,percentage}; 
	public Operation operation;
	public FloatReference OperatorFloat;
	public FloatEvent OutputFloat;
	public void InputFloat(float input){
		float output = 0;
		switch (operation)
		{
		case Operation.plus:
			output = input + OperatorFloat.Value;
			break;
		case Operation.minus:
			output = input - OperatorFloat.Value;
			break;
		case Operation.divide:
			output = input / OperatorFloat.Value;
			break;
		case Operation.multiply:
			output = input * OperatorFloat.Value;
			break;
		case Operation.percentage:
			output = input % OperatorFloat.Value;
			break;
		}
		OutputFloat.Invoke(output);
	}
	
	public void SetOperator(float input){
		OperatorFloat.Value = input;
	}
	
}
