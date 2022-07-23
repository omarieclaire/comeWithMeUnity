using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GenericEvent))]
public class GenericEventButton : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		GenericEvent myScript = (GenericEvent)target;
		if(GUILayout.Button("do event"))
		{
			myScript.Invoke();
		}
	}
}
