using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class StringDictionary : ScriptableObject
{
	public Dictionary<string,string> Dictionary
	{
		get
		{
			//Debug.Log("getting dict");
			return _dictionary;
		}
		set
		{
			//Debug.Log("setting dict");
			_dictionary= value;
		}
	}
	private Dictionary<string,string> _dictionary;
	
	public StringStringDictionaryEvent DictionaryEvent;
	
	public void AddKeyValue(string key,string value){
		
		Debug.Log("adding " + key +value);
		if(Dictionary==null){
			Dictionary = new Dictionary<string, string>();
		}
		string debug="";
		foreach(KeyValuePair<string,string> kvp in Dictionary){
			debug+=kvp.Key + kvp.Value+"\n";
			
		}
		Debug.Log(debug);
		Dictionary.Add(key,value);
	}
	public void UpdateKeyValue(string key, string value){
		if(Dictionary==null){
			Dictionary = new Dictionary<string, string>();
		}
		if(Dictionary.ContainsKey(key)){
			Dictionary[key] = value;
		} else {
			Dictionary.Add(key,value);
		}
	}
	
	public void OutputDictionary(){
		DictionaryEvent.Invoke(Dictionary);
	}
}
