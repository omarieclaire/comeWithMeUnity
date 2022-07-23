using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputGameObjectEvent : MonoBehaviour
{
	public GameObject GameObjectToOutput{
		get{
			return _gameObjectToOutput;
		}
		set{
			_gameObjectToOutput = value;
		}
	}
	[SerializeField]
	private GameObject _gameObjectToOutput;
	
	public GameObjectEvent GameObjectEvent;
	
	public void Output(){
		
		GameObjectEvent?.Invoke(GameObjectToOutput);
	}
	public void Set(GameObject input){
		GameObjectToOutput = input;
	}
}
