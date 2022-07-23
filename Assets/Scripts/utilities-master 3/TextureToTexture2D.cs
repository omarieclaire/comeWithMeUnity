using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureToTexture2D : MonoBehaviour
{
	public Texture2DEvent OutputTexture2D;
	public void Input(Texture input){
		Texture2D output = (Texture2D)input;
		OutputTexture2D.Invoke(output);
	}
}
