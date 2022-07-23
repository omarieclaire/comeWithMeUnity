using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelightBallController : MonoBehaviour
{
	public Vector3 rotateVelocity = new Vector3(100, 100, 0);
	public Vector3 acceleration = new Vector3(0, 0, 0.01f);
	public float speedLimit = .002f;
	private new Rigidbody rigidbody;
	
	
    // Start is called before the first frame update
    void Start()
    {
	    rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
	void FixedUpdate()
	{
		rigidbody.velocity += acceleration;
		if(rigidbody.velocity.z >= speedLimit){
			rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, speedLimit);
		}
	    //transform.Translate(Vector3.forward * 10 * Time.deltaTime);
	    //transform.position += velocity * Time.deltaTime;
	    //transform.Rotate(rotateVelocity * Time.deltaTime);
    }
    
	void Update(){
		//if (transform.position.z < -7)
		//{
		//Debug.Log("destroyed");
		//Destroy(gameObject);
		//}
	}
 
}
