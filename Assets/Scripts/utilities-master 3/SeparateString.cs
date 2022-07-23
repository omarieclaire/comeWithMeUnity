using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeparateString : MonoBehaviour
{
	[SerializeField]
	private string _parseString = " ";
	public string ParseString{
		get{
			return _parseString;
		} set{
			_parseString = value;
		}
	}
	public StringListEvent OutputSeparatedStrings;
	
	public void InputString(string input){
		Debug.Log("parse str input " + input,gameObject);
		string[] str = input.Split(char.Parse(ParseString));
		List<string> output = new List<string>(str);
		OutputSeparatedStrings.Invoke(output);
	}
}
