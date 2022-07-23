using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FindGameObjectInList : MonoBehaviour
{
	[SerializeField]
	private List<GameObject> gameObjectList;
	public List<GameObject> GameObjectList{
		get{
			return gameObjectList;
		} set{
			gameObjectList = value;
		}
	}
	public GameObjectEvent OutputGameObject;
	
	public void InputObject(GameObject input){
		GameObject output = GameObjectList.FirstOrDefault(i => GameObject.ReferenceEquals(i,input));
		OutputGameObject.Invoke(output);
	}
	
	public void InputObjectName(string input){
		GameObject output = GameObjectList.FirstOrDefault(i => i.name == input);
		OutputGameObject.Invoke(output);
	}
}
