using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindGameObjectByTag : MonoBehaviour
{
	public GameObjectEvent OutputGameObject;
	public GameObjectListEvent OutputGameObjects;
	public void InputTag(string input){
		List<GameObject> gameObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag(input));
		GameObject output = gameObjects[Random.Range(0,gameObjects.Count-1)];
		OutputGameObject.Invoke(output);
		OutputGameObjects.Invoke(gameObjects);
	}
}
