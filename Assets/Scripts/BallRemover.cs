using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallRemover : MonoBehaviour
{
	void Start(){
	}
	
	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("sphere"))
		{	
			if (this.gameObject.CompareTag("hand"))
			{
				float pointlesswhy = 0;
				AkSoundEngine.PostEvent( "delightHit" , gameObject);

			}
			other.gameObject.GetComponent<Animator>().SetTrigger("vanish");
			//I destroy it in the animator
				
		}
	} 

}
