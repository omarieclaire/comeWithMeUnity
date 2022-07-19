using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class invokeEvent : MonoBehaviour
{
	public UnityEvent Event;
	
	public void DoEvent(){
		Event.Invoke();
	}
	

}
