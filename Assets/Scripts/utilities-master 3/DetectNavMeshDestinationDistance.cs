using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DetectNavMeshDestinationDistance : MonoBehaviour
{
	public NavMeshAgent NavMeshAgent;
	public FloatEvent OutputDistance;
	public void Distance(){
		OutputDistance.Invoke(NavMeshAgent.remainingDistance);
	}
}
