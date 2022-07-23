using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchEvent : MonoBehaviour
{
	[System.Serializable]
	public class SwitchOutput{
		public int switchInt;
		public UnityEngine.Events.UnityEvent switchEvent;
	}
	[SerializeField]
	private int currentSwitch;
	public int CurrentSwitch{
		get{return currentSwitch;}set{currentSwitch = value;}
	}
	public List<SwitchOutput> SwitchOutputs;
	
	public void OutputSwitchEvent(){
		SwitchOutput output = SwitchOutputs.Find(sw => sw.switchInt==CurrentSwitch);
		//Debug.Log(output.switchInt + " " + CurrentSwitch,gameObject);
		output.switchEvent.Invoke();
	}
	public void IncrementSwitch(){
		CurrentSwitch++;
	}
}
