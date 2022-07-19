using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveTowardsSimilarlyNamed : MonoBehaviour
{
	public Transform target;
	public string targetTag;
	public float speed;
	public float distance = 0.2f;


    // Update is called once per frame
    void Update()
    {
	    if (target == null){
	    	List<GameObject> targets = new List<GameObject>(GameObject.FindGameObjectsWithTag(targetTag));
	    	foreach (var item in targets)
	    	{
	    		if (item != gameObject && Vector3.Distance(item.transform.position, transform.position) > distance)
	    		{
	    			target = item.transform;
	    		}
	    	}
	    	if (target != null)
	    	{
	    		// do nothing
	    		Debug.Log(gameObject.name, gameObject);
	    		Debug.Log(target, target);
	    	} else // if it's still null
	    	
	    	{
	    		target = null;
	    		return;
	    	}   	
	    } 
	    // time delta time gives me the amount of time since the last frame was rendered, good way to move things consistently
	    //transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
	    transform.position = target.position;
    }
}
