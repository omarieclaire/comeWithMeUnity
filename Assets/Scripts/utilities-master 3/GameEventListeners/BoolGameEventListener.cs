using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoolGameEventListener : MonoBehaviour
{
	public BoolGameEvent Event;
	public BoolEvent Response;
	
	private void OnEnable(){
		Event.RegisterListener(this);
	}
	public void OnDisable(){
		Event.UnRegisterListener(this);
	}
	public void OnEventRaised(bool input){
		Response.Invoke(input);
	}
}
