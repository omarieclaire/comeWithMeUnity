using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputGameObjectTag : MonoBehaviour
{
	public StringEvent OutputTag;
	public void InputGameObject(GameObject input){
		string output = input.tag;
		//Debug.Log("tag" + output);
		OutputTag.Invoke(output);
	}
}
