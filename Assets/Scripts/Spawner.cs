//using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	public GameObject[] spawnItems;
	public Transform[] spawnPoints;
	public float frequency;
	public float initialSpeed;
	float lastSpawnedTime;
	

	void Update()
	{
		if (Time.time > lastSpawnedTime + frequency) {
			Spawn();
			lastSpawnedTime = Time.time;
		}
	}
    
	public void Spawn(){
		GameObject newSpawnedObject = Instantiate(spawnItems[Random.Range(0, 1)], spawnPoints[Random.Range(0, 6)]);
		newSpawnedObject.GetComponent<Rigidbody>().velocity = transform.forward * initialSpeed;
		newSpawnedObject.transform.parent = transform;
	}
}






	//public float beat = (60/105) * 2;
	//private float timer;
    //void Update()
    //{
	//    if (timer>beat)
	//    { 
	//    	GameObject cube = Instantiate(cubes[Random.Range(0, 2)], points[Random.Range(0, 4)]);
	//	    cube.transform.localPosition = Vector3.zero;
	//	    cube.transform.Rotate(transform.forward, 90 * Random.Range(0, 4));
	//	    timer -= beat;
	//    }
	//    timer += Time.deltaTime; 
	//}