using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class LifeCycleTriggerEvents : MonoBehaviour
{
	[System.Serializable]
	public class TriggerEvent{
		public TriggerTime trigger;
		public UnityEvent eventToTrigger;
	}
	public enum TriggerTime {Start,Awake,Disable,Update,FixedUpdate,Quit};
	public List<TriggerEvent> triggerEvents;
	
	void Start(){
		foreach(TriggerEvent te in triggerEvents){
			if(te.trigger==TriggerTime.Start){
				//Debug.Log("trigger",gameObject);
				te.eventToTrigger.Invoke();
			}	
		}
	}
	
	void Awake(){
		foreach(TriggerEvent te in triggerEvents){
			if(te.trigger==TriggerTime.Awake){
				Debug.Log("invoking");
				te.eventToTrigger.Invoke();
			}	
		}
	}
	void Update(){
		foreach(TriggerEvent te in triggerEvents){
			if(te.trigger==TriggerTime.Update){
				te.eventToTrigger.Invoke();
			}	
		}
	}
	void FixedUpdate(){
		foreach(TriggerEvent te in triggerEvents){
			if(te.trigger==TriggerTime.FixedUpdate){
				te.eventToTrigger.Invoke();
			}	
		}
	}
	void OnApplicationQuit(){
		foreach(TriggerEvent te in triggerEvents){
			if(te.trigger==TriggerTime.Quit){
				te.eventToTrigger.Invoke();
			}	
		}
	}
}
