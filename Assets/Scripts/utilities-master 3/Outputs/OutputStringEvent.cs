using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputStringEvent : MonoBehaviour
{
	public StringReference StringReference;
	public string Prepend{
		get{
			return prepend;
		} set{
			prepend= value;
		}
	}
	[SerializeField]
	private string prepend;
	
	public string Append{
		get{
			return append;
		} set{
			append	= value;
		}
	}
	[SerializeField]
	private string append;
	
	public StringEvent OutputEvent;
	
	public void OutputString(){
		string output = prepend + StringReference.Value + append;
		//Debug.Log("output string " + output,gameObject);
		OutputEvent.Invoke(output);
	}
	
	public void SetValue(string value){
		StringReference.Value = value;
	}
}
