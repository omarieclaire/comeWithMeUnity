using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Add Explosion")]
public class AddExplosionToObject : ScriptableObject
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
	public float RateOfIncrease = 1;
	public GameObject ExplosionPrefab;
	
	
	public void TriggerFunction(GameObject gameObject,Vector3 point){
		Rigidbody rigidbody;
		if(rigidbody=gameObject.GetComponent<Rigidbody>()){
			rigidbody.AddExplosionForce(ForceToAdd,point,1);
		}

	}
	
	public void IncreaseForce(){
		ForceToAdd+=RateOfIncrease;
	}
}
