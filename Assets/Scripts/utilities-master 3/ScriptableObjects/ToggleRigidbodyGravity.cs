using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Game Object Function/Toggle Rigidbody")]
public class ToggleRigidbodyGravity : GameObjectFunction
{
	public bool stateToSet{
		get;set;
	}
	public override void TriggerFunction(GameObject gameObject){
		//Debug.Log(gameObject.name,gameObject);
		Rigidbody rigidBody;
		if(rigidBody=gameObject.GetComponent<Rigidbody>()){
			rigidBody.useGravity = stateToSet;
			rigidBody.isKinematic = !stateToSet;
		}
		Animator animator;
		if(animator = gameObject.GetComponent<Animator>()){
			animator.enabled = !stateToSet;
		}
		
	}
}
