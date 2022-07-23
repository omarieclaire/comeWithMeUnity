using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Int")]
public class IntVar : ScriptableObject
{
	[SerializeField]
	private int _value;
	public int Value{
		get{return _value;}
		set{
			_value = value;
			OnChange.Invoke(_value);
		}
	}
	[SerializeField]
	private IntEvent onChange;
	public IntEvent OnChange{
		get{return onChange;}set{onChange = value;}
	}
	
}
