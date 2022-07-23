using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StringListGameEventListener : MonoBehaviour
{
	public StringListGameEvent Event;
	public StringListEvent Response;
	
	private void OnEnable(){
		Event.RegisterListener(this);
	}
	public void OnDisable(){
		Event.UnRegisterListener(this);
	}
	public void OnEventRaised(List<string> input){
		//Debug.Log("raised " + input.Count,gameObject);
		Response.Invoke(input);
	}
}
