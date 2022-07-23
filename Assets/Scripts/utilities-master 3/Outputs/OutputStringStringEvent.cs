using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputStringStringEvent : MonoBehaviour
{
	public string FirstString{
		get{return _firstString;}
		set{_firstString = value;}
	}
	[SerializeField]
	private string _firstString;
	
	public string SecondString{
		get{return _secondString;}
		set{_secondString = value;}
	}
	[SerializeField]
	private string _secondString;
	
	public void Output(){
		StringStringEvent.Invoke(FirstString,SecondString);
	}
	public StringStringEvent StringStringEvent;
}
