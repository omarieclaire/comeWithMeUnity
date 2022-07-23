using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/FloatGameEvent")]
public class FloatGameEvent : ScriptableObject
{
	private List<FloatGameEventListener> listeners = new List<FloatGameEventListener>();
	public void Raise(float input){
		//Debug.Log(name);
		for(int i = listeners.Count-1;i>=0;i--){
			listeners[i].OnEventRaised(input);
		}
	}
	public void RegisterListener(FloatGameEventListener listener){
		listeners.Add(listener);
	}
	public void UnRegisterListener(FloatGameEventListener listener){
		listeners.Remove(listener);
	}
}
