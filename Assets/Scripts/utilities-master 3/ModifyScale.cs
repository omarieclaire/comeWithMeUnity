using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyScale : MonoBehaviour
{
	public Transform TransformToModify;
	
	public void Modify(float amount){
		TransformToModify.localScale = Vector3.one * amount;
	}
	
}
