using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRenderTexture : MonoBehaviour
{
	public Camera RenderTextureCamera;
	
	public RenderTexture TemplateRenderTexture;
	
	public TextureEvent RenderTextureEvent;
	
	private RenderTexture renderTexture;
	

	// Use this for initialization
	public void Create () {
		if (RenderTextureCamera.targetTexture != null)
		{
			RenderTextureCamera.targetTexture.Release();
		}
		renderTexture = new RenderTexture(TemplateRenderTexture);
		RenderTextureCamera.targetTexture = renderTexture;
		RenderTextureEvent.Invoke(renderTexture);

	}
}
