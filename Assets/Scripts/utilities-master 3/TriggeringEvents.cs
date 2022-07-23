using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeringEvents : MonoBehaviour
{
	public enum Trigger{OnTriggerEnter,OnTriggerExit};
	[System.Serializable]
	public class TriggerEvent{
		[TagSelector]
		public List<string> triggeringTags;
		public Trigger trigger;
		public GameObjectEvent eventToTrigger;
	}
	public List<TriggerEvent> triggerEvents;
	
	void OnTriggerEnter(Collider other){
		foreach(TriggerEvent te in triggerEvents){
			if(te.trigger==Trigger.OnTriggerEnter){
				if(te.triggeringTags.Contains(other.tag)){
					//Debug.Log("trigger event on enter",other.gameObject);
					te.eventToTrigger.Invoke(other.gameObject);
				}
			}
		}
	}
	
	void OnTriggerExit(Collider other){
		foreach(TriggerEvent te in triggerEvents){
			if(te.trigger==Trigger.OnTriggerExit){
				if(te.triggeringTags.Contains(other.tag)){
					te.eventToTrigger.Invoke(other.gameObject);
				}
			}
		}
	}
	
}
