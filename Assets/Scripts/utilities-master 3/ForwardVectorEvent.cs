using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardVectorEvent : MonoBehaviour {

	public Transform TargetTransform;
	
	public float Distance = 1;
	
	public Vector3Event vectorEvent;
	
	public void Invoke(){
		//Debug.Log("forward vector3 event " + TargetTransform.forward * Distance);
		vectorEvent.Invoke(TargetTransform.position + TargetTransform.forward * Distance);
	}
}
