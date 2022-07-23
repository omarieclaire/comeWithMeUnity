using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Float")]
public class FloatVar : ScriptableObject
{
	[SerializeField]
	private float _value;
	public float Value{
		get{return _value;}
		set{
			_value = value;
			OnChange.Invoke(_value);
		}
	}
	
	public FloatEvent OnChange;
	
}
