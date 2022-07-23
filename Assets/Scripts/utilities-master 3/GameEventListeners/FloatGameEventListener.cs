using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FloatGameEventListener : MonoBehaviour
{
	public FloatGameEvent Event;
	public FloatEvent Response;
	
	private void OnEnable(){
		Event.RegisterListener(this);
	}
	public void OnDisable(){
		Event.UnRegisterListener(this);
	}
	public void OnEventRaised(float input){
		Response.Invoke(input);
	}
}
