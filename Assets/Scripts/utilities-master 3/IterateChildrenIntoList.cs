using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IterateChildrenIntoList : MonoBehaviour
{
	[SerializeField]
	private Transform targetTransform;
	public Transform TargetTransform{
		get{
			return targetTransform;
		}
		set{
			targetTransform = value;
		}
	}
	[Tooltip("This event will output the target transform's children into a list of game objects")]
	public GameObjectListEvent gameObjectListEvent;
	
	public void OutputChildrenList(){
		List<GameObject> gameObjectList = new List<GameObject>();
		foreach(Transform t in TargetTransform.GetComponentsInChildren<Transform>()){
			if(t!=TargetTransform)
			gameObjectList.Add(t.gameObject);
			//Debug.Log(t.name);
		}
		gameObjectListEvent.Invoke(gameObjectList);
	}
}
