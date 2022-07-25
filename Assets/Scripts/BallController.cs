using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
	public Vector3 rotateVelocity = new Vector3(100, 100, 0);
	public Vector3 acceleration = new Vector3(0, 0, 0.01f);
	public float speedLimit = .002f;
	private new Rigidbody rigidbody;
	
	
    void Start()
    {
	    rigidbody = GetComponent<Rigidbody>();
	    //AkSoundEngine.PostEvent( "PlayerTwo" , gameObject);

    }

	void FixedUpdate()
	{
		//rigidbody.rotation = rotateVelocity;
		rigidbody.velocity += acceleration;
		if(rigidbody.velocity.z >= speedLimit){
			rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, speedLimit);
		}
    }
    
	void Update(){
		//if (transform.position.z < -7)
		//{
		//Debug.Log("destroyed");
		//Destroy(gameObject);
		//}
	}
 
}
