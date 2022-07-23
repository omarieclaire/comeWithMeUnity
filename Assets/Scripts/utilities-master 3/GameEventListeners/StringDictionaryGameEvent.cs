using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/StringDictionaryGameEvent")]
public class StringDictionaryGameEvent : ScriptableObject
{
	private List<StringDictionaryGameEventListener> listeners = new List<StringDictionaryGameEventListener>();
	public void Raise(Dictionary<string,string> input){
		//Debug.Log(name);
		for(int i = listeners.Count-1;i>=0;i--){
			listeners[i].OnEventRaised(input);
		}
	}
	public void RegisterListener(StringDictionaryGameEventListener listener){
		listeners.Add(listener);
	}
	public void UnRegisterListener(StringDictionaryGameEventListener listener){
		listeners.Remove(listener);
	}
}
