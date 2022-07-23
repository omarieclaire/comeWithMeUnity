using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Sprite List")]
public class SpriteListVar : ScriptableObject
{
	[SerializeField]
	private List<Sprite> _value;
	public List<Sprite> Value{
		get{
			return _value;
		} set{
			_value = value;
		}
	}
	[SerializeField]
	private int _iteratorInteger;
	public int IteratorInteger{
		get{
			return _iteratorInteger;
		} set{
			_iteratorInteger = value;
		}
	}
	
	public SpriteListEvent ListEvent;
	public SpriteEvent IterateListOutput;
	
	public void Add(Sprite input){
		Value.Add(input);
	}
	
	public void Iterate(){
		if(IteratorInteger<Value.Count){
			Sprite output = Value[IteratorInteger];
			IterateListOutput.Invoke(output);
			IteratorInteger++;
		} else {
			IteratorInteger = 0;
		}
	}
	
	//public StringEvent OutputRandomEvent;
	//public void OutputRandom(){
	//	string output = Value[Random.Range(0,Value.Count)];
	//	OutputRandomEvent.Invoke(output);
	//}
	public void OutputList(){
		//Debug.Log("output list");
		ListEvent.Invoke(Value);
	}
	public void ClearList(){
		Value = new List<Sprite>();
	}
}
