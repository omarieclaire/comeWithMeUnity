using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformGameEventListener : MonoBehaviour
{
	public TransformGameEvent Event;
	public TransformEvent Response;
	
	private void OnEnable(){
		Event.RegisterListener(this);
	}
	public void OnDisable(){
		Event.UnRegisterListener(this);
	}
	public void OnEventRaised(Transform transform){
		Response.Invoke(transform);
	}
}
