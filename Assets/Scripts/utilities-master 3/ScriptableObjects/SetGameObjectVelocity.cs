using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Game Object Function/set velocity")]
public class SetGameObjectVelocity : GameObjectFunction
{	
	public Vector3 Velocity;
	public override void TriggerFunction(GameObject gameObject){
		Rigidbody rigidbody;
		if(rigidbody = gameObject.GetComponent<Rigidbody>()){
			rigidbody.velocity = Velocity;
		}
	}
}
