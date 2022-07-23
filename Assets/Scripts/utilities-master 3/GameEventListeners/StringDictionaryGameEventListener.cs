using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StringDictionaryGameEventListener : MonoBehaviour
{
	public StringDictionaryGameEvent Event;
	public StringStringDictionaryEvent Response;
	
	private void OnEnable(){
		Event.RegisterListener(this);
	}
	public void OnDisable(){
		Event.UnRegisterListener(this);
	}
	public void OnEventRaised(Dictionary<string,string> input){
		Response.Invoke(input);
	}
}
