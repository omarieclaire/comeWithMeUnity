using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DeactivateObject : MonoBehaviour
{
	public bool state = false;
	public void SetState(GameObject input){
		input.SetActive(state);
	}

}

