using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Game Object Function/Remove Parent")]
public class RemoveParent : GameObjectFunction
{
	public override void TriggerFunction(GameObject gameObject){
		gameObject.transform.SetParent(null);
	}
}
