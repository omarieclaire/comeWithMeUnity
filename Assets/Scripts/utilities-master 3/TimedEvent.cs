using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class TimedEvent : MonoBehaviour
{
	[MinMaxSlider(0, 1000)]
    public Vector2 range;
	public UnityEvent eventToTime;
	public bool onStart = true;
	public bool loop;
	
	private IEnumerator coroutine;

	public void Start()
	{
		if(onStart)
			TriggerTimer();
    }
    
	public void TriggerTimer(){
		coroutine = Timed();
		StartCoroutine(coroutine);
	}
	
	public void StopTimer(){
		StopCoroutine(coroutine);
	}
	
    IEnumerator Timed()
    {
        while (true)
        {
	        float time = Random.Range(range.x, range.y);
	        //Debug.Log(time);
            yield return new WaitForSeconds(time);
            eventToTime.Invoke();
	        if(!loop){
	        	//Debug.Log("stiop");
	        	yield break;
	        }
        }
    }

}
