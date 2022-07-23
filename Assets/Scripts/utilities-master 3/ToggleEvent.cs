using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToggleEvent : MonoBehaviour
{
	[SerializeField]
	private bool state;
	public bool State{
		get{
			return state;
		}
		set{
			state = value;
			CheckState();
		}
	}
	
	public UnityEvent trueEvent;
	public UnityEvent falseEvent;
	
	public void Toggle(){
		
		State = !State;
		
		//CheckState();
	}
	
	public void CheckState(){
		if(State){
			//Debug.Log(gameObject.name,gameObject);
			trueEvent.Invoke();
		} else {
			falseEvent.Invoke();
		}
	}
		
		
}
