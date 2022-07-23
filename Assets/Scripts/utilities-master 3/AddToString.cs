using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToString : MonoBehaviour
{
	[SerializeField]
	private string _string = "";
	public string String{
		get{return _string;}
		set{
			_string = value;
		}
	}
	
	public void Prepend(string input){
		String = input + String;
	}
	public void Append(string input){
		String += input;
	}
	public StringEvent Output;
	public void OutputString(){
		Output.Invoke(String);
	}
}
