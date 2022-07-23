using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringStringGameEventListener : MonoBehaviour
{
	public StringStringGameEvent Event;
	
	public StringStringEvent Response;
	
	private void OnEnable(){
		Event.RegisterListener(this);
	}
	public void OnDisable(){
		Event.UnRegisterListener(this);
	}
	public void OnEventRaised(string input1,string input2){
		Response.Invoke(input1,input2);
	}
}
