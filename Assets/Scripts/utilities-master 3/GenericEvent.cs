using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericEvent : MonoBehaviour
{
    public UnityEvent eventToInvoke;
	public void Invoke()
    {
        eventToInvoke.Invoke();
    }
}
