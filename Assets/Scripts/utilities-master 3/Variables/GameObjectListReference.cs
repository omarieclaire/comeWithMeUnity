using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class GameObjectListReference
{
	public bool UseConstant = true;
	public List<GameObject> ConstantValue;
	public GameObjectListVar Variable;

	public GameObjectListReference()
	{ }

	public GameObjectListReference(List<GameObject> value)
	{
		UseConstant = true;
		ConstantValue = value;
	}

	public List<GameObject> Value
	{
		get { return UseConstant ? ConstantValue : Variable.Value; }
		set{if(UseConstant)ConstantValue = value; if(!UseConstant)Variable.Value = value;}
	}

	public static implicit operator List<GameObject>(GameObjectListReference reference)
	{
		return reference.Value;
	}
}
