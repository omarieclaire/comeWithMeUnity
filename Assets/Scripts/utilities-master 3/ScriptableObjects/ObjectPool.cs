using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPool : MonoBehaviour
{
	public GameObject Prefab{
		get;set;
	}
	
	public List<GameObject> GameObjectPool;
	
	public GameObjectEvent OutputGameObject;
	
	public void ReturnObject(Vector3 spawnPoint){
		GameObject output = null;
		if(GameObjectPool==null){
			GameObjectPool = new List<GameObject>();
		}
		
		foreach(GameObject go in GameObjectPool){
			if(!go.activeInHierarchy){
				output = go;
				output.SetActive(true);
				output.transform.position = spawnPoint;
				//return output;
			}
		}
		
		if(output == null){
			output = Instantiate(Prefab);
			GameObjectPool.Add(output);
			output.transform.position = spawnPoint;	
		}
		
		
		//return output;
		OutputGameObject.Invoke(output);
	}
}
