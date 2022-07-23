using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowObjectName : MonoBehaviour
{
	public TextMesh TextMeshPrefab;
	
	public void SpawnTextMesh(){
		TextMesh textMesh = Instantiate(TextMeshPrefab,transform.position,Quaternion.identity);
		textMesh.transform.SetParent(transform);
		textMesh.text = gameObject.name;
	}
}
