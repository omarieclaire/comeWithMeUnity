using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetGameObjectName : MonoBehaviour
{
	[SerializeField]
	private GameObject _gameObjectToSet;
	public GameObject GameObjectToSet{
		get{
			return _gameObjectToSet;
		} set{
			_gameObjectToSet = value;
		}
	}
	
	public void InputString(string input){
		GameObjectToSet.name = input;
	}
	
	
}
