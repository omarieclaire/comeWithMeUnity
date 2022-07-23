using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Texture")]
public class TextureVar : ScriptableObject
{
	[SerializeField]
	private Texture _value;
	public Texture Value{
		get{return _value;}
		set{
			_value = value;
			OnChange.Invoke(_value);
		}
	}
	
	public TextureEvent OnChange;
}
