using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Events/GameObjectEvent")]
public class GameObjectGameEvent : ScriptableObject
{
	private List<GameObjectGameEventListener> listeners = new List<GameObjectGameEventListener>();
	public void Raise(GameObject input){
		//Debug.Log(name);
		for(int i = listeners.Count-1;i>=0;i--){
			listeners[i].OnEventRaised(input);
		}
	}
	public void RegisterListener(GameObjectGameEventListener listener){
		listeners.Add(listener);
	}
	public void UnRegisterListener(GameObjectGameEventListener listener){
		listeners.Remove(listener);
	}
}
