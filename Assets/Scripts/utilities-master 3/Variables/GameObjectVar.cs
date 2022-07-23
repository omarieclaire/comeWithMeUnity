using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Game Object")]
public class GameObjectVar : ScriptableObject
{
	[SerializeField]
	private GameObject _value;
	public GameObject Value{
		get{return _value;}
		set{
			_value = value;
			OnChange.Invoke(_value);
		}
	}
	
	public GameObjectEvent OnChange;
}
