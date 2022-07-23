using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Variables/Transform")]
public class TransformVar : ScriptableObject
{
	[SerializeField]
	private Transform _value;
	public Transform Value{
		get{
			return _value;
		} set{
			_value = value;
			OnChange.Invoke(_value);
		}
	}
	public TransformEvent OnChange;
}
