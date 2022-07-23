using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VectorSamplePosition : MonoBehaviour {

	public NavMeshAgent navMeshAgent;
	public Vector3Event vectorEventOnComplete;
	
	public void Invoke(Vector3 input){
		Vector3 randomPoint = navMeshAgent.transform.position + input + -Vector3.up;
		NavMeshHit hit;
		Vector3 result = input;
		if (NavMesh.SamplePosition(randomPoint, out hit, 3.0f, NavMesh.AllAreas))
		{
			//Debug.Log("sample position " + result);
			result = hit.position;
			navMeshAgent.SetDestination(result);
		} else {
			Debug.Log("no sample");
		}
		
		vectorEventOnComplete.Invoke(result);
	}
}
