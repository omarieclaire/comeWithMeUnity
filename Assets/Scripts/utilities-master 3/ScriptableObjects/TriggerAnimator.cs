using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Object Function/Trigger Animator")]
public class TriggerAnimator : GameObjectFunction
{
	public string triggerName;
	public override void TriggerFunction(GameObject gameObject){
		Animator animator;
		if(animator = gameObject.GetComponent<Animator>()){
			animator.SetTrigger(triggerName);
		}
	}
}
