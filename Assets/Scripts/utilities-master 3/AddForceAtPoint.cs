using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForceAtPoint : MonoBehaviour
{
	public Rigidbody RigidBody;
	[SerializeField]
	private float _force;
	public float Force{
		get{
			return _force;
		} set{
			_force = value;
		}
	}
	public void AddForce(Vector3 point){
		//RigidBody.AddForceAtPosition(Force,point,ForceMode.Force);
		RigidBody.AddExplosionForce(Force,point,1);
	}
	
	
}
