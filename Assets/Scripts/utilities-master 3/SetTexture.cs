using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTexture : MonoBehaviour
{
	public MeshRenderer MeshRenderer;
	public void Input(Texture texture){
		MeshRenderer.sharedMaterial.mainTexture = texture;
	}
}
