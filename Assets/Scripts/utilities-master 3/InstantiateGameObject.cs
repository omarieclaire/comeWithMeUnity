using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateGameObject : MonoBehaviour
{
	[SerializeField]
	private GameObject _objectToInstantiate;
	public GameObject ObjectToInstantiate{
		get{
			return _objectToInstantiate;
		} set{
			_objectToInstantiate = value;
		}
	}
	
	[SerializeField]
	private Vector3 _position;
	public Vector3 Position{
		get{
			return _position;
		} set{
			_position = value;
		}
	}
	
	[SerializeField]
	private Transform _parent;
	public Transform Parent{
		get{
			return _parent;
		} set{
			_parent = value;
		}
	}
	public bool MatchParent = true;
	
	public GameObjectEvent OutputInstantiatedGameobject;
	
	public void Instantiate(){
		GameObject output;
		if(Parent!=null){
			if(MatchParent){
				output = Instantiate(ObjectToInstantiate,Parent.position + Position,ObjectToInstantiate.transform.rotation,Parent);
			} else {
				output = Instantiate(ObjectToInstantiate,Position,Quaternion.identity,Parent);
			}
				
		} else {
			output = Instantiate(ObjectToInstantiate,Position,Quaternion.identity);
		}
		OutputInstantiatedGameobject.Invoke(output);
		
	}
	
	public void InstantiateFromMonobehaviour(MonoBehaviour input){
		
	}
}
