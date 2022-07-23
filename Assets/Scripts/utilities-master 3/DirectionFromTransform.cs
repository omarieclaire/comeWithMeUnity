using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionFromTransform : MonoBehaviour
{
	[SerializeField]
	private Transform _targetTransform;
	public Transform TargetTransform{
		get{
			return _targetTransform;
		}set {
			_targetTransform = value;
		}
	}
	
	public Vector3Event OutputDirection;
	
	void Update(){
		OutputDirection.Invoke(TargetTransform.forward);
	}
	
}
