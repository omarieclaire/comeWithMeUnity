using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/TransformGameEvent")]
public class TransformGameEvent : ScriptableObject
{
	private List<TransformGameEventListener> listeners = new List<TransformGameEventListener>();
	public void Raise(Transform input){
		//Debug.Log(name);
		for(int i = listeners.Count-1;i>=0;i--){
			listeners[i].OnEventRaised(input);
		}
	}
	public void RegisterListener(TransformGameEventListener listener){
		listeners.Add(listener);
	}
	public void UnRegisterListener(TransformGameEventListener listener){
		listeners.Remove(listener);
	}
}
