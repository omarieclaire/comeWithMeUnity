using System;
[Serializable]
public class FloatReference
{
	public bool UseConstant = true;
	public float ConstantValue;
	public FloatVar Variable;

	public FloatReference()
	{ }

	public FloatReference(float value)
	{
		UseConstant = true;
		ConstantValue = value;
	}

	public float Value
	{
		get { return UseConstant ? ConstantValue : Variable.Value; }
		set{if(UseConstant)ConstantValue = value; if(!UseConstant)Variable.Value = value;}
	}

	public static implicit operator float(FloatReference reference)
	{
		return reference.Value;
	}
}

