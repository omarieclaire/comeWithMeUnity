using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetParent : MonoBehaviour
{
	[SerializeField]
	private Transform _transformToSet;
	public Transform TransformToSet{
		get{
			return _transformToSet;
		} set{
			_transformToSet = value;
		}
	}
	[SerializeField]
	private Transform _parent;
	public Transform Parent{
		get{
			return _parent;
		} set{
			_parent = value;
		}
	}
	
	public void Set(){
		TransformToSet.parent = Parent;
	}
}
