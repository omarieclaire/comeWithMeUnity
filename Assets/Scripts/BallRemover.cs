using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallRemover : MonoBehaviour
{
	private void OnTriggerEnter(Collider other) {
		
		if (other.gameObject.CompareTag("sphere"))
		{
			
			other.gameObject.GetComponent<Animator>().SetTrigger("vanish");
			//Destroy(other.gameObject);
				
		}
	} 

}
