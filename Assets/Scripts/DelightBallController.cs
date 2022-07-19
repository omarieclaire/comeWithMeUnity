using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelightBallController : MonoBehaviour
{
	public Vector3 velocity = new Vector3(0, 0, 10);
	public Vector3 rotateVelocity = new Vector3(100, 100, 0);
	public float speedLimit = 50.0f;
	private new Rigidbody rigidbody;
	
	
    // Start is called before the first frame update
    void Start()
    {
	    rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
	void FixedUpdate()
	{
		rigidbody.velocity += this.velocity;
		if(rigidbody.velocity.z >= speedLimit){
			rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, speedLimit);
		}
	    //transform.Translate(Vector3.forward * 10 * Time.deltaTime);
	    //transform.position += velocity * Time.deltaTime;
	    //transform.Rotate(rotateVelocity * Time.deltaTime);
    }
    
	void Update(){
		if (transform.position.z > 27)
		{
		Debug.Log("destroyed");
		Destroy(gameObject);
		}
	}
 
}
