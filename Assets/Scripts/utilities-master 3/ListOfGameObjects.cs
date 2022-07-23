using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ListOfGameObjects : MonoBehaviour
{
	[SerializeField]
	private List<GameObject> gameObjectList;
	public List<GameObject> GameObjectList{
		get{
			return gameObjectList;
		}
		set{
			gameObjectList = value;
		}
	}

	//public int AmountToRandomlyOutput;
	[MinMaxSlider(0,100)]
	public Vector2 RangeToRandomlyOutput;
	
	public GameObjectListEvent gameObjectListEvent;
	public void OutputList(){
		gameObjectListEvent.Invoke(GameObjectList);
	}
	public void CountList(){
		//Debug.Log("count " + GameObjectList.Count);
		OutputCount.Invoke(GameObjectList.Count);
	}
	
	public GameObjectEvent IteratedGameObject;
	public UnityEvent OnIterationComplete;
	public IntEvent OutputCount;
	
	
	public void IterateList(int iterator){
		if(iterator<GameObjectList.Count){
			//Debug.Log(GameObjectList[iterator].name + " " + iterator) ;
			IteratedGameObject.Invoke(GameObjectList[iterator]);
		} else {
			OnIterationComplete.Invoke();
		}
			
	}
	
	int iterator = 0;
	public void IterateSingleObject(){
		IteratedGameObject.Invoke(GameObjectList[iterator]);
		if(iterator<GameObjectList.Count-1){
			iterator++;
		} else {
			iterator = 0;		
		}
	}
	
	public void IterateAllObects(){
		foreach(GameObject obj in GameObjectList){
			IteratedGameObject.Invoke(obj);
		}
		OnIterationComplete.Invoke();
	}
	
	public void CreateListFromTag(string tag){
		GameObjectList = new List<GameObject>(GameObject.FindGameObjectsWithTag(tag));
	}
	
	public void IterateRandomObjects(){
		List<GameObject> output = new List<GameObject>();
		float random = Random.Range(RangeToRandomlyOutput.x,RangeToRandomlyOutput.y);
		//Debug.Log("random " +random);
		for (int i = 0; i < random; i++)
		{
			GameObject outputObj = GameObjectList[Random.Range(0,GameObjectList.Count)];
			output.Add(outputObj);
			IteratedGameObject.Invoke(outputObj);
                 
		}
		gameObjectListEvent.Invoke(output);
	}
	
	public void Add(GameObject input){
		GameObjectList.Add(input);
	}
	
	public TransformListEvent TransformListEvent;
	
	public void OutputTransformList(){
		List<Transform> transformList = new List<Transform>();
		foreach(GameObject go in GameObjectList){
			transformList.Add(go.transform);
		}
		TransformListEvent.Invoke(transformList);
	}
}
