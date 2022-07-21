using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class lineDrawer : MonoBehaviour
{
	public Transform pointer;
	// public Transform pointee;
	public LineRenderer line;
	
	// public float lineDistance;
	// only colliders of a certain layer will be impacted
	public LayerMask layerMask;
	//public AudioSource audioSource;
	private Vector3 target;
	public UnityEvent onPoint;
	public UnityEvent offPoint;
	
	void Update(){
		line.SetPosition(0, pointer.position);
		// needs a collider and a rigidbody on the object receiving the collision
		RaycastHit hit;
		// Does the ray intersect any objects excluding the player layer
		if (Physics.Raycast(pointer.transform.position, pointer.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
		{
			//line.SetPosition(0, pointer.transform.position);
	//Debug.Log(hit.transform.name + " " + pointer.position, hit.transform);
		
			//need a better solution - the player and the dummy share a tag (player1 and dummy1 have the same tag) and when the player is pointing at something it checks to see if it has the same tag
			// with this soluniot I'd have to add the tag when they spawn			
			//if (hit.collider.transform.parent != pointer.parent)
			//{
			//	// render a line to whereever the ray is hitting. hit holds the information about the raycast hit
			//	line.SetPosition(1, hit.point);
			//}
			
			//if (!audioSource.isPlaying)
			//{
			//	audioSource.Play();
			//}			
			if (hit.collider.tag != pointer.tag)
			{
				target = hit.point;
				//line.enabled = true;
				//Debug.Log("hit");
				onPoint.Invoke();
				//if (!audioSource.isPlaying)
				//{
				//	audioSource.Play();
				//}
				
			} else
			{
				offPoint.Invoke();
				//line.enabled = false;
			}
		} else
		{
			//line.enabled = false;
		}
		if (target == null)
		{
			return;
		}
		// render a line to whereever the ray is hitting. hit holds the information about the raycast hit
		if (target != Vector3.zero)
		{
			line.SetPosition(1, target);			
		} else 
		{
			line.enabled = false;
			
		}
	
	}

}
