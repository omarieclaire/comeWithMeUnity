using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class KeyEvent{
	public KeyCode keyCode;
	public UnityEvent unityEvent;
}

public class KeyInputEvents : MonoBehaviour
{
	public List<KeyEvent> keyedEvents;
    // Update is called once per frame
    void Update()
    {
	    foreach(KeyEvent keyEvent in keyedEvents){
	    	if(Input.GetKeyDown(keyEvent.keyCode)){
	    		keyEvent.unityEvent.Invoke();
	    	}
	    }
    }
}
