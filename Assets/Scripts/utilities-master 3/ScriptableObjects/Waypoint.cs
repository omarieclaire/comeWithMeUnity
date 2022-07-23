using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TimeStampedEvent
{
    public float timeStamp;
    public UnityEvent eventToPlay;
}
[ExecuteInEditMode]
public class Waypoint : MonoBehaviour
{
	public List<Transform> pointsToMoveTo;
	public List<Transform> pointsToLookAt;
	public AudioClip audioClip;
	public float travelTime;
	public List<TimeStampedEvent> timeStampedEvents;
	public UnityEvent EventOnComplete;
	[HideInInspector]
	public List<Vector3> points; 
	public void AddPoint(){
		Vector3 newPoint = Vector3.zero;
		if(pointsToMoveTo.Count==0){
			newPoint = transform.position;
			
		} else {
			newPoint = pointsToMoveTo[pointsToMoveTo.Count - 1].position;
		}
		GameObject point = Instantiate(new GameObject("waypoint" + pointsToMoveTo.Count),newPoint,Quaternion.identity,transform);
		
		pointsToMoveTo.Add(point.transform);
	}
	
	void Awake(){
		if(audioClip!=null)
			gameObject.name = audioClip.name;
	}
}
