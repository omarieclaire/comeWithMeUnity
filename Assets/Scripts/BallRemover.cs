using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallRemover : MonoBehaviour
{
	void Start(){
	}
	
	private void OnTriggerEnter(Collider other) {
		//Debug.Log("collider");
		if (other.gameObject.CompareTag("sphere"))
		{	
			//Debug.Log("spehere tag");
			if (this.gameObject.CompareTag("hand"))
			{
				float pointlesswhy = 0;
				//Debug.Log("hand tag");

				AkSoundEngine.PostEvent( "delightHit" , gameObject);

			}
			other.gameObject.GetComponent<Animator>().SetTrigger("vanish");
			//I destroy it in the animator
				
		}
	} 

}
