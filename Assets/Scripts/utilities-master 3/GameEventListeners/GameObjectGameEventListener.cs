using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameObjectGameEventListener : MonoBehaviour
{
	public GameObjectGameEvent Event;
	public GameObjectEvent Response;
	
	private void OnEnable(){
		Event.RegisterListener(this);
	}
	public void OnDisable(){
		Event.UnRegisterListener(this);
	}
	public void OnEventRaised(GameObject input){
		
		Response.Invoke(input);
	}
}
