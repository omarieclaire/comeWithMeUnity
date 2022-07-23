using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/StringGameEvent")]
public class StringGameEvent : ScriptableObject
{
	private List<StringGameEventListener> listeners = new List<StringGameEventListener>();
	public void Raise(string input){
		//Debug.Log(name + " " + input);
		for(int i = listeners.Count-1;i>=0;i--){
			listeners[i].OnEventRaised(input);
		}
	}
	public void RegisterListener(StringGameEventListener listener){
		listeners.Add(listener);
	}
	public void UnRegisterListener(StringGameEventListener listener){
		listeners.Remove(listener);
	}
}
