using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/StringList")]
public class StringListVar : ScriptableObject
{
	[SerializeField]
	private List<string> _value;
	public List<string> Value{
		get{
			return _value;
		} set{
			_value = value;
			StringListEvent.Invoke(_value);
		}
	}
	[SerializeField]
	private int _iteratorInteger;
	public int IteratorInteger{
		get{
			return _iteratorInteger;
		} set{
			_iteratorInteger = value;
		}
	}
	
	public StringListEvent StringListEvent;
	public StringEvent IterateStringListOutput;
	
	public void Add(string input){
		Value.Add(input);
	}
	
	public void Iterate(){
		if(IteratorInteger<Value.Count){
			string output = Value[IteratorInteger];
			IterateStringListOutput.Invoke(output);
			IteratorInteger++;
		} else {
			IteratorInteger = 0;
		}
	}
	
	public StringEvent OutputRandomEvent;
	public void OutputRandom(){
		string output = Value[Random.Range(0,Value.Count)];
		OutputRandomEvent.Invoke(output);
	}
	public void OutputList(){
		//Debug.Log("output list");
		StringListEvent.Invoke(Value);
	}
	public void ClearList(){
		Value = new List<string>();
	}
	
}
