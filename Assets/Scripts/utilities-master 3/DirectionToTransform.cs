using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionToTransform : MonoBehaviour
{
	public Transform TransformFrom{
		get{
			return _transformFrom;
		} set{
			_transformFrom = value;
		}
	}
	[SerializeField]
	private Transform _transformFrom;
	
	public Transform TransformTo{
		get{
			return _transformTo;
		} set{
			_transformTo = value;
		}
	}
	[SerializeField]
	private Transform _transformTo;
	
	public Vector3Event OutputDirection;
	
	public void Output(){
		Vector3 output = TransformTo.position - TransformFrom.position;
		OutputDirection.Invoke(output);
	}
}
