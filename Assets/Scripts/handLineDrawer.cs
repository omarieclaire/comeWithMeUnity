using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Events;

public class handLineDrawer : MonoBehaviour
{
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

	float ampT;
	public Material traceMaterial;
	public float traceWidth = 0.3f;
	//public GameObject targetOptional;
	public float altRotation;
	public enum Origins{Start, Middle};
	public handLineDrawer.Origins origin = Origins.Start;
	public int size = 300;
	public float lengh = 10.0f;
	public float freq = 2.5f;
	public float amp = 1;
	public bool ampByFreq;
	public bool centered = true;
	public bool centCrest = true;
	public bool warp = true;
	public bool warpInvert;
	public float warpRandom;
	public float walkManual;
	public float walkAuto;
	public bool spiral;
	float start;
	float warpT;
	float angle;
	float sinAngle;
	float sinAngleZ;
	double walkShift;
	Vector3 posVtx2;
	Vector3 posVtxSizeMinusOne;
	LineRenderer lrComp;
	
	void Awake(){
		lrComp = GetComponent<LineRenderer>();
		lrComp.useWorldSpace = false;
		lrComp.material = traceMaterial;
	}

	
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
			lrComp.enabled = true;
			lrComp.SetWidth(traceWidth, traceWidth);

			if (warpRandom <= 0) { warpRandom = 0; }
			if (size <= 2) { size = 2; }
			int size1 = size - 1;
			lrComp.SetVertexCount(size);

			lengh = lengh / transform.localScale.x;
			
			// I don't want origin to be equal to "Origin.start" (what is that?) 
			// I want it to gently move torwards my last known origin, like this
			//line[0] = Vector3.MoveTowards(lastPosition,transform.position,Time.deltaTime*FollowSpeed);
			//lastPosition = line[0];


			origin = Origins.Start;  
			lengh = (transform.position - target.position).magnitude;
		
			transform.LookAt(target.position); 
			transform.Rotate(altRotation, -90, 0);
	
			if (ampByFreq) {ampT = Mathf.Sin(freq*Mathf.PI);}
			else {ampT = 1;}
			ampT = ampT * amp;
			if (warp && warpInvert) {ampT = ampT/2;}
		
			for (int i = 0; i < size; i++) {
				angle = (2*Mathf.PI/ size1 * i*freq);
				if (centered) {
					angle -= freq*Mathf.PI; 	//Center
					if (centCrest) {
						angle -= Mathf.PI/2;	//Crest/Knot
					}
				}
				else {centCrest = false;}
			
				walkShift -= walkAuto/ size1 * Time.deltaTime;
				angle += (float)walkShift - walkManual;
				sinAngle = Mathf.Sin(angle);
				if (spiral) {sinAngleZ = Mathf.Cos(angle);}
				else {sinAngleZ = 0;}
			
				if (origin == Origins.Start) {start = 0;}
				else {start = lengh/2;}
				
				// I don't want the target to be equal to target 
				// I want it to gently move torwards my last position like this
				// Vector3.MoveTowards(lastTargetPosition,target.position,Time.deltaTime*TargetFollowSpeed);
			
				if (warp) {
					warpT = size1 - i;
					warpT = warpT / size1;
					warpT = Mathf.Sin(Mathf.PI * warpT * (warpRandom+1));
					if (warpInvert) {warpT = warpT==0?999999999:1/warpT;}
					lrComp.SetPosition(i, new Vector3(lengh/ size1 * i - start, sinAngle * ampT * warpT, sinAngleZ * ampT * warpT));
				}
				else {
					lrComp.SetPosition(i, new Vector3(lengh/ size1 * i - start, sinAngle * ampT, sinAngleZ * ampT));
					warpInvert = false;
				}

				if (i == 1) { posVtx2 = new Vector3(lengh / size1 * i - start, sinAngle * ampT * warpT, sinAngleZ * ampT * warpT); }
				if (i == size-2) { posVtxSizeMinusOne = new Vector3(lengh / size1 * i - start, sinAngle * ampT * warpT, sinAngleZ * ampT * warpT); }

			}

			if (warpInvert)
			{  //Fixes pinned limits when WarpInverted
				lrComp.SetPosition(0, posVtx2);
				lrComp.SetPosition(size-1, posVtxSizeMinusOne);
			}
			
			// I need to reintegrate this code below with the new system, but don't know how
			
			// If the distance between the end of the line and the last target position 
			// is greater than minimumDistance, invoke a distance event (what does that do?)
			
			//if(Vector3.Distance(line[1],lastTargetPosition) > minimumDistance){
			//	//Debug.Log("distance event");
			//	distanceEvent.Invoke();
			//}
			
			//lastTargetPosition = line[1]; // what is this doing?
			//lineRenderer.SetPositions(line);	// I don't think I need this line anymore?	
			
		} else{
			// disable the line renderer
			lrComp.enabled = false;
		}
		// why do I do this?
		closestDistance += Time.deltaTime;
	}
	
	public void PopulateHands(){
		otherHands = new List<GameObject>();
		var hands = GameObject.FindGameObjectsWithTag("hand");
		otherHands = hands.Where(h => h != this.gameObject).ToList();
	}
	
	private void ChooseTarget(){
		if(otherHands!=null && otherHands.Count > 0){
			Animator parentAnimator = GetComponentInParent<Animator>();
			
			foreach (GameObject hand in otherHands) {
				float distance = Vector3.Distance(transform.position, hand.transform.position);
				Animator otherParentAnimator = hand.GetComponentInParent<Animator>();
				if(myOtherHand == null){

					if(parentAnimator==otherParentAnimator && hand != transform.parent.gameObject)
					{
						myOtherHand=hand;
					}
				}
				if(hand != myOtherHand && hand != transform.parent.gameObject && hand.gameObject.activeInHierarchy){
					if(distance <= closestDistance){
						closestDistance = distance;
						if(otherParentAnimator != null){
							//Debug.Log($"Drawing line between {parentAnimator.gameObject.name} and {otherParentAnimator.gameObject.name}");
							target = hand.transform;
						}	
					}
				}
				
				//if(otherParentAnimator !=null)
				//Debug.Log($"Hand {hand.name} of {otherParentAnimator.gameObject.name} is {distance}m away");
			}
			if(target != null &&! target.gameObject.activeInHierarchy){
				//Debug.Log("reset target");
				target = null;
				closestDistance = 100;
			}
			
		}
	}
}
