using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Events/IntGameEvent")]
public class IntGameEvent : ScriptableObject
{
	private List<IntGameEventListener> listeners = new List<IntGameEventListener>();
	public void Raise(int input){
		//Debug.Log(name);
		for(int i = listeners.Count-1;i>=0;i--){
			listeners[i].OnEventRaised(input);
		}
	}
	public void RegisterListener(IntGameEventListener listener){
		listeners.Add(listener);
	}
	public void UnRegisterListener(IntGameEventListener listener){
		listeners.Remove(listener);
	}
}
