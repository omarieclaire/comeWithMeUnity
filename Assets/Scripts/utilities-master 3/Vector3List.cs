using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector3List : MonoBehaviour
{
	[SerializeField]
	private List<Vector3> _list;
	public List<Vector3> List{
		get{
			return _list;
		} set{
			_list = value;
		}
	}
	
	public Vector3ListEvent ListEvent;
	
	public void Add(Vector3 input){
		if(List==null){
			List = new List<Vector3>();
		}
		//Debug.Log("list " + List.Count);
		List.Add(input);
	}
	public void Output(){
		ListEvent.Invoke(List);
	}
}
