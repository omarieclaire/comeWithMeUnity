using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/GameObjectList")]
public class GameObjectListVar : ScriptableObject
{
	[SerializeField]
	private List<GameObject> _value;
	public List<GameObject> Value{
		get{
			return _value;
		} set{
			_value = value;
			ListEvent.Invoke(_value);
		}
	}
	//[SerializeField]
	//private int _iteratorInteger;
	//public int IteratorInteger{
	//	get{
	//		return _iteratorInteger;
	//	} set{
	//		_iteratorInteger = value;
	//	}
	//}
	
	public GameObjectListEvent ListEvent;
	public GameObjectEvent IterateListOutput;
	
	public void Add(GameObject input){
		Value.Add(input);
	}
	
	//public void Iterate(){
	//	if(IteratorInteger<Value.Count){
	//		GameObject output = Value[IteratorInteger];
	//		IterateListOutput.Invoke(output);
	//		IteratorInteger++;
	//	} else {
	//		IteratorInteger = 0;
	//	}
	//}
	
	public GameObjectEvent OutputRandomEvent;
	public void OutputRandom(){
		GameObject output = Value[Random.Range(0,Value.Count)];
		OutputRandomEvent.Invoke(output);
	}
	public void OutputList(){
		//Debug.Log("output list");
		ListEvent.Invoke(Value);
	}
	public void ClearList(){
		Value = new List<GameObject>();
	}
}
