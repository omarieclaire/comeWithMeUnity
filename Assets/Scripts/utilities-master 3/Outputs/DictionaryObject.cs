using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DictionaryObject : MonoBehaviour
{
	[SerializeField]
	private Dictionary<string,string> dictionary;
	public Dictionary<string,string> Dictionary{
		get{
			if(dictionary==null){
				dictionary = new Dictionary<string,string>();
			}
			return dictionary;
		}
		set{
			if(dictionary==null){
				dictionary = new Dictionary<string,string>();
			}
			dictionary = value;
			OutputData.Invoke(dictionary);
		}
	}
	public StringStringDictionaryEvent OutputData;
}
