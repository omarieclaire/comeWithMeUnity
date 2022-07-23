using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameObjectPool : MonoBehaviour
{
	public bool InstantiateObjects = false;
	public List<GameObject> Pool;
	public Transform Parent{
		get{return parent;}
		set{parent = value;}
	}
	[SerializeField]
	private Transform parent;
	public GameObjectEvent OutputPooledObject;
	
	public void InputGameObject(GameObject input){
		GameObject output;
		if(Pool==null){
			Pool = new List<GameObject>();
		}
		Debug.Log(input,gameObject);
		if(output = Pool.FirstOrDefault(i => i.name== input.name)){
			//object already in pool
			Debug.Log("in pool",gameObject);
		} else{
			if(InstantiateObjects){
				output = Instantiate(input,Parent.position,Parent.rotation,Parent);
			} else{
				output = input;
			}
			output.name = input.name;
			Pool.Add(output);
		}
		OutputPooledObject.Invoke(output);
	}
}
