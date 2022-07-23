using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class SpriteListReference
{
	public bool UseConstant = true;
	public List<Sprite> ConstantValue;
	public SpriteListVar Variable;
	
	public SpriteListReference()
	{ }

	public SpriteListReference(List<Sprite> value)
	{
		UseConstant = true;
		ConstantValue = value;
	}

	public List<Sprite> Value
	{
		get { return UseConstant ? ConstantValue : Variable.Value; }
		set{if(UseConstant)ConstantValue = value; if(!UseConstant)Variable.Value = value;}
	}

	public static implicit operator List<Sprite>(SpriteListReference reference)
	{
		return reference.Value;
	}
}
