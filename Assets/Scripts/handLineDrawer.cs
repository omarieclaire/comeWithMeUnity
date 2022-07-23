using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Events;

public class handLineDrawer : MonoBehaviour
{
	public LineRenderer lineRenderer;
	public List<GameObject> otherHands;
	public GameObject myOtherHand;
	public float closestDistance=100;
	public Transform target;
	public float FollowSpeed = 1;
	public float TargetFollowSpeed = 1;
	private Vector3 lastPosition;
	private Vector3 lastTargetPosition;
	public float minimumDistance = 0.1f;
	public UnityEvent distanceEvent;
	private bool isPlaying = false;
	
	private void Update() {
		if(otherHands==null){
			PopulateHands();
		}
		
		ChooseTarget();
		
	}
	
	void FixedUpdate(){
		DrawLines();
	}
	
	private void DrawLines(){
		if(target!=null){
			lineRenderer.enabled = true;
			
			// I need to do this only once, for each new connection
			// and to reset it when that connection ends
			// which means I need to track each connection in relation to the playeer
			// which is something i meant to do anyway
			//but I shouod think about scope and howto do i tsimple first.
			
			if (!isPlaying)
			{
				//AkSoundEngine.PostEvent( "connectionMade" , gameObject);
				isPlaying = true;
			}
			
			var line = new Vector3[2];
			line[0] = Vector3.MoveTowards(lastPosition,transform.position,Time.deltaTime*FollowSpeed);
			lastPosition = line[0];
			line[1] = Vector3.MoveTowards(lastTargetPosition,target.position,Time.deltaTime*TargetFollowSpeed);
			if(Vector3.Distance(line[1],lastTargetPosition)>minimumDistance){
				//Debug.Log("distance event");
				distanceEvent.Invoke();
			}
			lastTargetPosition = line[1];
			lineRenderer.SetPositions(line);
		} else{
			lineRenderer.enabled = false;
		}
		closestDistance+=Time.deltaTime;
	}
	
	public void PopulateHands(){
		//Debug.Log("populating",gameObject);
		otherHands = new List<GameObject>();
		var hands = GameObject.FindGameObjectsWithTag("hand");
		otherHands = hands.Where(h => h != this.gameObject).ToList();
	}
	
	private void ChooseTarget(){
		//Debug.Log(otherHands.Count);
		if(otherHands!=null&&otherHands.Count>0){
			Animator parentAnimator = GetComponentInParent<Animator>();
			
			foreach (GameObject hand in otherHands) {
				float distance = Vector3.Distance(transform.position, hand.transform.position);
				Animator otherParentAnimator = hand.GetComponentInParent<Animator>();
				if(myOtherHand==null){

					if(parentAnimator==otherParentAnimator&&hand!=transform.parent.gameObject)
					{
						myOtherHand=hand;
					}
				}
				if(hand!=myOtherHand&&hand!=transform.parent.gameObject&&hand.gameObject.activeInHierarchy){
					if(distance<=closestDistance){
						closestDistance=distance;
						if(otherParentAnimator !=null){
							//Debug.Log($"Drawing line between {parentAnimator.gameObject.name} and {otherParentAnimator.gameObject.name}");
							target = hand.transform;
						}
						
					}
				}
				
				//if(otherParentAnimator !=null)
				//Debug.Log($"Hand {hand.name} of {otherParentAnimator.gameObject.name} is {distance}m away");
			}
			if(target!=null&&!target.gameObject.activeInHierarchy){
				//Debug.Log("reset target");
				target = null;
				closestDistance = 100;
				isPlaying = false;
				//AkSoundEngine.PostEvent( "connectionBroken" , gameObject);
			}
			
		}
	}
}
