using System;
using System.Collections.Generic;
[Serializable]
public class StringListReference
{
	public bool UseConstant = true;
	public List<string> ConstantValue;
	public StringListVar Variable;

	public StringListReference()
	{ }

	public StringListReference(List<string> value)
	{
		UseConstant = true;
		ConstantValue = value;
	}

	public List<string> Value
	{
		get { return UseConstant ? ConstantValue : Variable.Value; }
		set{if(UseConstant)ConstantValue = value; if(!UseConstant)Variable.Value = value;}
	}

	public static implicit operator List<string>(StringListReference reference)
	{
		return reference.Value;
	}
}
