using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityDrawer : MonoBehaviour
{
	Transform target;
	public LineRenderer line;

	
	// OnTriggerEnter is called when the Collider other enters the trigger.
	protected void OnTriggerEnter(Collider other)
	{
		if (other.tag != gameObject.tag)
		{
			line.enabled = true;
			target = other.transform;
			//Debug.Log(target.name + " " + target.position, target);
		}
	}
	
	// OnTriggerExit is called when the Collider other has stopped touching the trigger.
	protected void OnTriggerExit(Collider other)
	{
		if (other.transform == target)
		{
			
		}
	}
	
	void Update(){
		if (target != null)
		{
			line.SetPosition(1, target.position);
			line.SetPosition(0, transform.position);	
	

		}
	}
}
