using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
	public Transform TransformToFollow;
	public Transform TransformToMove;
	
	void Update(){
		TransformToMove.position = TransformToFollow.position;
	}
}
