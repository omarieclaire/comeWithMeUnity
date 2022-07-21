using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallRemover : MonoBehaviour
{
	private void OnTriggerEnter(Collider other) {
		
		if (other.gameObject.CompareTag("sphere"))
		{
			
			Destroy(other.gameObject);
			
		}
	} 

}
