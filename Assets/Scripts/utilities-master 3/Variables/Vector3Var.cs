using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Vector3")]
public class Vector3Var : ScriptableObject
{
	[SerializeField]
	private Vector3 _value;
	public Vector3 Value{
		get{
			return _value;
		} set{
			_value = value;
		}
	}
	
}
