using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Game Object Function/Toggle GameObject")]
public class ToggleGameObject : GameObjectFunction
{
	public bool stateToSet;
	public override void TriggerFunction(GameObject gameObject){
		if(gameObject!=null){
			gameObject.SetActive(stateToSet);
			//Debug.Log("setting " + gameObject + " to " + stateToSet,gameObject);
		}
		
	}
}
