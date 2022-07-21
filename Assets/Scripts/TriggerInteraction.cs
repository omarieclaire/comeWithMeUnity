using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class GameObjectEvent : UnityEvent <GameObject>
{
	
	
}


public class TriggerInteraction : MonoBehaviour
{
	[System.Serializable]
	public class TriggerEvent {
		public string targetTag = "sphere";
		public GameObjectEvent onTrigger;
	}
	public List<TriggerEvent> triggerEvents;
	void OnTriggerEnter(Collider other){
		foreach (var item in triggerEvents)
		{
			if (other.tag == item.targetTag)
			{
				item.onTrigger.Invoke(other.gameObject);
				Debug.Log(other.name);		
			}
		}		
	}
}
