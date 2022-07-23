using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Game Object Function/Change Shader on GameObjects")]
public class ChangeGameObjectShaderValue : GameObjectFunction
{
	[SerializeField]
	private string shaderProperty;
	public string ShaderProperty{
		get{
			return shaderProperty;
		} set{
			shaderProperty = value;
		}
	}
	[SerializeField]
	private float _value;
	public float Value{
		get{
			return _value;
		} set{
			_value = value;
		}
	}
	public override void TriggerFunction(GameObject gameObject){
		Renderer objectRenderer;
		if(objectRenderer = gameObject.GetComponent<Renderer>()){
			foreach(Material mat in objectRenderer.materials){
				if(mat.HasProperty(ShaderProperty)){
					mat.SetFloat(ShaderProperty,Value);
				}
			}
		}
	}
}
