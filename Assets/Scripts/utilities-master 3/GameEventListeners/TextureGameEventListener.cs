using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TextureGameEventListener : MonoBehaviour
{
	public TextureGameEvent Event;
	public TextureEvent Response;
	
	private void OnEnable(){
		Event.RegisterListener(this);
	}
	public void OnDisable(){
		Event.UnRegisterListener(this);
	}
	public void OnEventRaised(Texture texture){
		Response.Invoke(texture);
	}
}
