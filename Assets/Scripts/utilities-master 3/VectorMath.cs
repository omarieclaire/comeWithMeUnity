using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorMath : MonoBehaviour
{
	public enum Operation {plus,minus}; 
	public Operation operation;
	public Vector3Reference OperatorVector;
	public void SetOperator(Vector3 input){
		OperatorVector.Value = input;
	}
	public Vector3Event OutputVector;
	public void InputVector(Vector3 input){
		Vector3 output = new Vector3();
		switch (operation)
		{
		case Operation.plus:
			output = input + OperatorVector.Value;
			break;
		case Operation.minus:
			output = input - OperatorVector.Value;
			break;

		}
		OutputVector.Invoke(output);
	}
	
}
