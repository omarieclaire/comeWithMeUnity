using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StateVariables : MonoBehaviour
{
	[System.Serializable]
	public class StateFloat{
		public string Name;
		public float Value{
			get{
				return _value;
			}
			set{
				_value = value;
				OutputFloat.Invoke(_value);
			}
		}
		[SerializeField]
		private float _value;
		public FloatEvent OutputFloat;
	}
	[System.Serializable]
	public class StateInteger{
		public string Name;
		public int Value{
			get{
				return _value;
			}
			set{
				if(value>=Min){
					_value = value;
				}
				//Debug.Log("int " + _value);
				OutputIntEvent.Invoke(_value);
			}
		}
		[SerializeField]
		private int _value;
		public int Min = 0;
		public IntEvent OutputIntEvent;
	}
	
	public List<StateFloat> StateFloats;
	public List<StateInteger> StateIntegers;
	
	public void IncrementFloat(string key, float amount){
		StateFloats.FirstOrDefault(i => i.Name == key).Value+=amount;
	}
	public void IncrementInteger(string key,int amount){
		//Debug.Log("incrementing " + key + " ");
		StateIntegers.FirstOrDefault(i=>i.Name==key).Value+=amount;
	}
	public float GetFloat(string key){
		Debug.Log("looking for " + key);
		return StateFloats.FirstOrDefault(i => i.Name == key).Value;
	}
	public int GetInt(string key){
		//Debug.Log("looking for " + key);
		return StateIntegers.FirstOrDefault(i=>i.Name==key).Value;
	}
	public void SetInt(string key,int value){
		StateIntegers.FirstOrDefault(i=>i.Name==key).Value=value;
	}
	public void OutputInt(string key){
		StateInteger output = StateIntegers.FirstOrDefault(i=>i.Name==key);
		output.OutputIntEvent.Invoke(output.Value);
	}
}
