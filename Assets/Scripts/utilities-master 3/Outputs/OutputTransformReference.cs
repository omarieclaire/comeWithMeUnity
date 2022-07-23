using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputTransformReference : MonoBehaviour
{
	public TransformReference TransformReference;
	public TransformEvent OutputTransform;
	public void Output(){
		OutputTransform.Invoke(TransformReference.Value);
	}
	public void Set(Transform input){
		TransformReference.Value = input;
	}
}
