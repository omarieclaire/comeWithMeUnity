using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfGameObjectActive : MonoBehaviour
{
	public BoolEvent OutputGameObjectState;
	public void InputGameObject(GameObject input){
		bool output = input.activeInHierarchy;
		OutputGameObjectState.Invoke(output);
	}
}
