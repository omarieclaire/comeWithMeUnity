using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class TextureReference
{
	public bool UseConstant = true;
	public Texture ConstantValue;
	public TextureVar Variable;

	public TextureReference()
	{ }

	public TextureReference(Texture value)
	{
		UseConstant = true;
		ConstantValue = value;
	}

	public Texture Value
	{
		get { return UseConstant ? ConstantValue : Variable.Value; }
		set{if(UseConstant)ConstantValue = value; if(!UseConstant)Variable.Value = value;}
	}

	public static implicit operator Texture(TextureReference reference)
	{
		return reference.Value;
	}
}
