using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/StringStringGameEvent")]
public class StringStringGameEvent : ScriptableObject
{
	private List<StringStringGameEventListener> listeners = new List<StringStringGameEventListener>();
	public void Raise(string input1,string input2){
		//Debug.Log(name + " " + input);
		for(int i = listeners.Count-1;i>=0;i--){
			listeners[i].OnEventRaised(input1,input2);
		}
	}
	public void RegisterListener(StringStringGameEventListener listener){
		listeners.Add(listener);
	}
	public void UnRegisterListener(StringStringGameEventListener listener){
		listeners.Remove(listener);
	}
}
