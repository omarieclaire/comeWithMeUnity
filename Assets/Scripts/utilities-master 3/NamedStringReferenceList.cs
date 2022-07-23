using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

	[System.Serializable]
	public class NamedStringReference{
		public string Name;
		public StringReference StringReference;
	}

[CreateAssetMenu]
public class NamedStringReferenceList : ScriptableObject
{

	
	public List<NamedStringReference> NamedStringReferences;
	
	public StringStringDictionaryEvent DictionaryEvent;
	
	public void SaveToDictionary(){
		//Debug.Log("save dicationary");
		Dictionary<string,string> dictionary = new Dictionary<string, string>();
		foreach(NamedStringReference namedString in NamedStringReferences){
			dictionary.Add(namedString.Name,namedString.StringReference);
		}
		DictionaryEvent.Invoke(dictionary);
	}
	
	public void SetValues(Dictionary<string,string> input){
		foreach(NamedStringReference nsr in NamedStringReferences){
			if(input.ContainsKey(nsr.Name)){
				//Debug.Log(input[nsr.Name] + " " + nsr.StringReference.Value);
				nsr.StringReference.Value = input[nsr.Name];
			}
		}
	}
}
