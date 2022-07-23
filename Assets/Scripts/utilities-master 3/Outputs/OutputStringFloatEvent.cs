using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OutputStringFloatEvent : MonoBehaviour
{
	public string String{
		get{
			return _string;
		}
		set{
			_string = value;
		}
	}
	[SerializeField]
	private string _string;
	
	public float Float{
		get{
			return _float;
		}
		set{
			_float = value;
		}
	}
	[SerializeField]
	private float _float;
	
	
	
	public StringFloatEvent EventToOutput;
	
	public void Output(){
		EventToOutput.Invoke(String,Float);
	}
}
