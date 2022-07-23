using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="Dictionaries/Scriptable Object Dictionary")]
public class ScriptableObjectDictionary : ScriptableObject
{
	[System.Serializable]
	public class Item{
		public string name;
		public ScriptableObject scriptableObject;
	}
	public List<Item> Dictionary;
	public StringEvent OutputToString;
	
	public void Save(){
		//Debug.Log("saving ");
		string output = "";
		for (int i = 0; i < Dictionary.Count; i++) 
		{
			
			output += JsonUtility.ToJson(Dictionary[i]);
		
		}
		Debug.Log(output);
	}
	
	
}
