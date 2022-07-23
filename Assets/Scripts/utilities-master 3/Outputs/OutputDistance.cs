using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputDistance : MonoBehaviour
{
	public Transform FirstTransform{
		get{return _firstTransform;}
		set{_firstTransform = value;}
	}
	[SerializeField]
	private Transform _firstTransform;
	
	public Transform SecondTransform{
		get{return _secondTransform;}
		set{_secondTransform = value;}
	}
	[SerializeField]
	private Transform _secondTransform;
	
	public float Distance;
	
	public FloatEvent OutputDistanceFloat;
	
	public void Output(){
		float output = Vector3.Distance(FirstTransform.position,SecondTransform.position);
		Distance = output;
		OutputDistanceFloat.Invoke(output);
	}
	
}
