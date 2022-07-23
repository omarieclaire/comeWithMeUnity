using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Game Object Function/Change Layer")]
public class ChangeLayer : ScriptableObject
{
	public int Layer;
	public void InputGameObject(GameObject input){
		if(input==null){
			return;
		}
		input.layer = Layer;
		foreach(Transform t in input.GetComponentsInChildren<Transform>()){
			t.gameObject.layer = Layer;

		}
	}
}
