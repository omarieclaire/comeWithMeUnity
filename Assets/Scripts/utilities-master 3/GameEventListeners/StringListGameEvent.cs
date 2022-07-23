using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/StringListGameEvent")]
public class StringListGameEvent : ScriptableObject
{
	private List<StringListGameEventListener> listeners = new List<StringListGameEventListener>();
	public void Raise(List<string> input){
		//Debug.Log(name+input.Count);
		for(int i = listeners.Count-1;i>=0;i--){
			listeners[i].OnEventRaised(input);
		}
	}
	public void RegisterListener(StringListGameEventListener listener){
		listeners.Add(listener);
	}
	public void UnRegisterListener(StringListGameEventListener listener){
		listeners.Remove(listener);
	}
}
