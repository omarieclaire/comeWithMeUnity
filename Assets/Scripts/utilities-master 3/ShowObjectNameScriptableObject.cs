using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ShowObjectNameScriptableObject : ScriptableObject
{
	public void InputGameObject(GameObject input){
		ShowObjectName showObjectName;
		if(showObjectName = input.GetComponent<ShowObjectName>())
		{
			showObjectName.SpawnTextMesh();
		}
	}
}
