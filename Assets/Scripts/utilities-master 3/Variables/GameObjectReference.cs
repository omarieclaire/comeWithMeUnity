using System;
using UnityEngine;
[Serializable]
public class GameObjectReference
{
	public bool UseConstant = true;
	public GameObject ConstantValue;
	public GameObjectVar Variable;

	public GameObjectReference()
	{ }

	public GameObjectReference(GameObject value)
	{
		UseConstant = true;
		ConstantValue = value;
	}

	public GameObject Value
	{
		get { return UseConstant ? ConstantValue : Variable.Value; }
		set{if(UseConstant)ConstantValue = value; if(!UseConstant)Variable.Value = value;}
	}

	public static implicit operator GameObject(GameObjectReference reference)
	{
		return reference.Value;
	}
}
