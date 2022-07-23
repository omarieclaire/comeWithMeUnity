using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputTexture : MonoBehaviour
{
	public TextureReference TextureReference;
	public TextureEvent OutputTextureEvent;
	public void Output(){
		OutputTextureEvent.Invoke(TextureReference.Value);
	}
	
	public void SetValue(Texture input){
		TextureReference.Value = input;
	}
}
