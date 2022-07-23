using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Game Object Function/Add Force")]
public class AddForceToObject : GameObjectFunction
{
	[SerializeField]
	private float forceToAdd;
	public float ForceToAdd{
		get{
			return forceToAdd;
		} set{
			forceToAdd = value;
		}
		
	}
	
	public override void TriggerFunction(GameObject gameObject){
		Rigidbody rigidbody;
		Transform target = gameObject.transform.parent;
		if(target==null){
			target=gameObject.transform;
		}
		if(rigidbody=gameObject.GetComponent<Rigidbody>()){
			rigidbody.AddForce((target.transform.position - gameObject.transform.position).normalized * ForceToAdd,ForceMode.Force);
			//rigidbody.AddExplosionForce(ForceToAdd,)
		}
	}
	
	public void IncreaseForce(){
		ForceToAdd+=0.5f;
	}
}
