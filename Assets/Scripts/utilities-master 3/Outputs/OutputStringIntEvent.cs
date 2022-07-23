using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OutputStringIntEvent : MonoBehaviour
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
	
	public int Int{
		get{
			return _int;
		}
		set{
			_int = value;
		}
	}
	[SerializeField]
	private int _int;
	
	[System.Serializable]
	public class StringIntEvent:UnityEvent<string,int>{
		
	}
	
	public StringIntEvent EventToOutput;
	
	public void Output(){
		EventToOutput.Invoke(String,Int);
	}
}
