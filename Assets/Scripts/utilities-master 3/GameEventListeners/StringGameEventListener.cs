using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StringGameEventListener : MonoBehaviour
{
	
	public StringGameEvent Event;
	public string Prepend;
	public string Append;
	public StringEvent Response;
	
	
	
	
	private void OnEnable(){
		Event.RegisterListener(this);
	}
	public void OnDisable(){
		Event.UnRegisterListener(this);
	}
	public void OnEventRaised(string input){
		Response.Invoke(Prepend+input+Append);
	}
}
