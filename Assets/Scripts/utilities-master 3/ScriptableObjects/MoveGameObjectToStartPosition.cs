using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGameObjectToStartPosition : MonoBehaviour
{
	public float range;
	[MinMaxSlider(0,1000)]
	public Vector2 timeRange;

	
	public void TriggerFunction(Transform targetTransform){
		StartCoroutine(MoveToStart(targetTransform));
	}
	
	private IEnumerator MoveToStart(Transform targetTransform){
		Vector3 startPos = targetTransform.position;
		Vector3 moveFromPos = targetTransform.position + Random.onUnitSphere * range;
		targetTransform.position = moveFromPos;
		float totalTime = Random.Range(timeRange.x,timeRange.y);
		float elapsed = 0;
		while(elapsed<=1){
			elapsed +=Time.deltaTime/totalTime;
			targetTransform.position = Vector3.Lerp(moveFromPos,startPos,Mathf.Lerp(0,1,elapsed));
			yield return new WaitForEndOfFrame();
		}
	}
}
