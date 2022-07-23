using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputDictionaryToNamedStringEvent : MonoBehaviour
{
	[System.Serializable]
	public class NamedStringEvent{
		public string Name;
		public StringEvent StringEvent;
	}
	
	public List<NamedStringEvent> NamedStringEvents;
	
	public void InputDictionary(Dictionary<string,string> input){
		if(input==null){
			return;
		}
		foreach(NamedStringEvent nse in NamedStringEvents){
			
			if(input.ContainsKey(nse.Name)){
				//Debug.Log(nse.Name + " " + input[nse.Name]);
				nse.StringEvent.Invoke(input[nse.Name]);
			}
		}
	}
	
	
}
