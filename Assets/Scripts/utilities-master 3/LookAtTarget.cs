using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
	public Transform Target{
		get;set;
	}
	[TagSelector]
	public string TargetTag;
	
	void Start(){
		if(!string.IsNullOrEmpty(TargetTag)){
			Target = GameObject.FindGameObjectWithTag(TargetTag).transform;
		}
	}
	
	void Update(){
		//transform.LookAt(Target.position);
		if(Target!=null){
			//Debug.Log(Target.name);
			Vector3 dir = transform.position - Target.position;
			//dir.y = 0; // keep the direction strictly horizontal
			Quaternion rot = Quaternion.LookRotation(dir);
			// slerp to the desired rotation over time
			transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime);
		}
		
	}
}
