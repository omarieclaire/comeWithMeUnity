using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomInt : MonoBehaviour
{
	public int Min {
		get {
			return _min;
		} set {
			_min = value;
		}
	}
	[SerializeField]
	private int _min;
	
	public int Max {
		get {
			return _max;
		} set {
			_max = value;
		}
	}
	[SerializeField]
	private int _max;
	
	public IntEvent OutputInt;
	public void Generate(){
		int output = Random.Range(Min,Max);
		OutputInt.Invoke(output);
	}
}
