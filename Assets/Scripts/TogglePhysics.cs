using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogglePhysics : MonoBehaviour
{
	public bool kinematicState;
	public bool gravityState;

	public void Toggle(GameObject input){
		input.GetComponent<Rigidbody>().isKinematic = kinematicState;
		input.GetComponent<Rigidbody>().useGravity = gravityState;
	}

}
