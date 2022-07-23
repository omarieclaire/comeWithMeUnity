using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Sets/DictionaryObject")]
public class DictionaryObjectRuntimeSet : RuntimeSet<DictionaryObject>
{
	public string CompareKey;
	public DictionaryObject DictionaryObjectPrefab;
	public GameObjectEvent OutputGameObject;
	public void InputDictionary(Dictionary<string,string> input){
		DictionaryObject dictionaryObject;
		//if exists
		if(!input.ContainsKey(CompareKey)){
			return;
		}
		
		if(dictionaryObject = Items.Find(f => f.Dictionary[CompareKey] == input[CompareKey])){
			//exists
			
		} else {
			dictionaryObject = Instantiate(DictionaryObjectPrefab);
			Items.Add(dictionaryObject);
		}
		
		dictionaryObject.Dictionary = input;
		OutputGameObject.Invoke(dictionaryObject.gameObject);
		Debug.Log(input["InternName"],dictionaryObject.gameObject);
		
	}
	public GameObjectListEvent OutputGameObjectListEvent;
	public void OutputAsGameObjectList(){
		List<GameObject> Output = new List<GameObject>();
		foreach(DictionaryObject dictObj in Items){
			Output.Add(dictObj.gameObject);
		}
		OutputGameObjectListEvent.Invoke(Output);
	}
}
