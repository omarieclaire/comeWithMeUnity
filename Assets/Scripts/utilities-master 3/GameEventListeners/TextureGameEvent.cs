using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/TextureGameEvent")]
public class TextureGameEvent : ScriptableObject
{
	private List<TextureGameEventListener> listeners = new List<TextureGameEventListener>();
	public void Raise(Texture input){
		Debug.Log(name);
		for(int i = listeners.Count-1;i>=0;i--){
			listeners[i].OnEventRaised(input);
		}
	}
	public void RegisterListener(TextureGameEventListener listener){
		listeners.Add(listener);
	}
	public void UnRegisterListener(TextureGameEventListener listener){
		listeners.Remove(listener);
	}
}
