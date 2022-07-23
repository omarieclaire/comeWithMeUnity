using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/String")]
public class StringVar : ScriptableObject
{
	[SerializeField]
	private string _value;
	public string Value{
		get{return _value;}
		set{
			_value = value;
			Debug.Log("changed " + name + " " + _value);
			OnChange.Invoke(_value);
		}
	}
	
	public string InitialValue;
	public bool UseInitialValue;
	
	public StringEvent OnChange{
		get{Debug.Log("changing!");return onChange;}set{onChange = value;Debug.Log("changing!");}
	}
	[SerializeField]
	private StringEvent onChange;
	
	void Awake(){
		if(UseInitialValue){
			Value = InitialValue;
		}
	}
	public void Output(){
		OnChange.Invoke(Value);
	}
	
}
