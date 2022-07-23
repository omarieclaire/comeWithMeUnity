using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputGameObjectName : MonoBehaviour
{
	public StringEvent OutputNameString;
	public void InputGameObject(GameObject input){
		//Debug.Log(gameObject.name,gameObject);
		OutputNameString.Invoke(input.name);
	}
}
