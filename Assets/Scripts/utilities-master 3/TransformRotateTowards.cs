using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformRotateTowards : MonoBehaviour
{
	public Transform TargetToRotate{
		get{return _targetToRotate;}
		set{_targetToRotate = value;}
	}
	[SerializeField]
	private Transform _targetToRotate;
	
	public Transform TargetToRotateToward{
		get{return _targetToRotateToward;}
		set{_targetToRotateToward = value;}
	}
	[SerializeField]
	private Transform _targetToRotateToward;
	public float Speed = 1;
	
	public void RotateTowards(){
		if(TargetToRotate!=null&&TargetToRotateToward!=null)
		TargetToRotate.rotation = Quaternion.Slerp(TargetToRotate.rotation,TargetToRotateToward.rotation,Speed);
	}
}
