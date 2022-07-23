using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntGameEventListener : MonoBehaviour
{
	public IntGameEvent Event;
	public IntEvent Response;
	
	private void OnEnable(){
		Event.RegisterListener(this);
	}
	public void OnDisable(){
		Event.UnRegisterListener(this);
	}
	public void OnEventRaised(int input){
		Response.Invoke(input);
	}
}
