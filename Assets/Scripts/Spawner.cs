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