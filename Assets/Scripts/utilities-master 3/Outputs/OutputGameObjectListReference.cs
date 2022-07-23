using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputGameObjectListReference : MonoBehaviour
{
	public GameObjectListReference GameObjectListToOutput;
	//{
	//	get{
	//		return _gameObjectListToOutput;
	//	}
	//	set{
	//		_gameObjectListToOutput = value;
	//	}
	//}
	//[SerializeField]
	//private List<GameObject> _gameObjectListToOutput;
	[SerializeField]
	private int _iteratorInteger;
	public int IteratorInteger{
		get{
			return _iteratorInteger;
		} set{
			_iteratorInteger = value;
		}
	}
	public void Iterate(){
		if(IteratorInteger<GameObjectListToOutput.Value.Count){
			GameObject output = GameObjectListToOutput.Value[IteratorInteger];
			GameObjectEvent.Invoke(output);
			IteratorInteger++;
		} else {
			IteratorInteger = 0;
		}
	}
	
	public GameObjectListEvent GameObjectListEvent;
	public GameObjectEvent GameObjectEvent;
	
	public void Output(){
		GameObjectListEvent.Invoke(GameObjectListToOutput.Value);
	}
	public void Set(List<GameObject> input){
		GameObjectListToOutput.Value = input;
	}
	public void Find(string objectName){
		GameObject output = GameObjectListToOutput.Value.Find(obj=>obj.name==objectName);
		Debug.Log("name " + objectName,gameObject);
		if(output!=null){
			
			GameObjectEvent?.Invoke(output);	
		}
		
	}
	public void IterateAll(){
		while(IteratorInteger<GameObjectListToOutput.Value.Count){
			GameObject output = GameObjectListToOutput.Value[IteratorInteger];
			GameObjectEvent.Invoke(output);
			IteratorInteger++;
		}
	}
	
	
}
