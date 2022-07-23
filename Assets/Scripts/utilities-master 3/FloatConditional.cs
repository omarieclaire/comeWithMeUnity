using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatConditional : MonoBehaviour
{
	public enum Condition {Greater,Lesser,Equal,GreaterEqual,LesserEqual};
	public Condition condition;
	public FloatReference ConditionFloat;
	public BoolEvent ConditionEvent;
	public void InputFloat(float Input){
		bool output = false;
		switch (condition)
		{
		case Condition.Greater:
			output = Input > ConditionFloat;
			break;
		case Condition.Lesser:
			output = Input < ConditionFloat;
			break;
		case Condition.GreaterEqual:
			output = Input >= ConditionFloat;
			break;
		case Condition.LesserEqual:
			output = Input <= ConditionFloat;
			break;
		case Condition.Equal:
			output = Input == ConditionFloat;
			break;
		}
		ConditionEvent.Invoke(output);
	}
	public void SetConditional(float input){
		ConditionFloat.Value = input;
	}
}
