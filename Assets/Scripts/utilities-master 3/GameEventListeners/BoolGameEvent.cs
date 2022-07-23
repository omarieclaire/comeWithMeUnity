using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Events/BoolGameEvent")]
public class BoolGameEvent : ScriptableObject
{
	private List<BoolGameEventListener> listeners = new List<BoolGameEventListener>();
	public void Raise(bool input){
		//Debug.Log(name);
		for(int i = listeners.Count-1;i>=0;i--){
			listeners[i].OnEventRaised(input);
		}
	}
	public void RegisterListener(BoolGameEventListener listener){
		listeners.Add(listener);
	}
	public void UnRegisterListener(BoolGameEventListener listener){
		listeners.Remove(listener);
	}
}
