// The dream
// When one player’s hand comes close enough to any other player’s hand, a line is drawn between those hands
// Over time that line gets thinner and eventually disappears completely *unless* the hands remain close
 
// What I have now
// Lines are drawn from every hand to every hand
// the lines are a bit jittery
 
 
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Events;
 
public class handLineDrawer : MonoBehaviour
{
	public Transform me;
	public List<GameObject> allOtherHands;
	public GameObject myOpposingHand;
	public float closestDistance = 100; 
	public Transform target;
	public Vector3 lineTargetV3;
	public Vector3 lineOriginV3;
	public float FollowSpeed = 1;
	public float TargetFollowSpeed = 1;
	private Vector3 lastLineOriginPosition;
	private Vector3 lastLineTargetPosition;
	public float minimumDistance = 0.1f;
	public UnityEvent distanceEvent;
 
	float ampT;
	public Material traceMaterial;
	public float traceWidth = 0.3f;
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
	LineRenderer myLine;
    
	void Awake(){
		myLine = GetComponent<LineRenderer>(); // get my line renderer
		myLine.useWorldSpace = false; // this is to draw the line where the player is 
		myLine.material = traceMaterial; // set material
	}
    
	private void Update(){ // once per frame
		if(allOtherHands == null) { // if I haven't seen any other hands
			PopulateHands(); // get the hands
		}
		ChooseTarget(); // then chose a target
	}
    
	void FixedUpdate(){ // once or several times a frame depending on the framerate
		DrawLines(); // draw lines
	}
    
	private void DrawLines(){ 
		if(target != null){ // if I have a target
			myLine.enabled = true; // draw my line
			myLine.SetWidth(traceWidth, traceWidth); // set the width of the line
 
			if (warpRandom <= 0) {  // line warp stuff
				warpRandom = 0; 
			}
			if (size <= 2) { // make sure the line has at least two elements?
				size = 2; 
			}
			int size1 = size - 1; 
			myLine.SetVertexCount(size); // set the size
 
			lengh = lengh / transform.localScale.x; // ?
            
			me = transform; // this is just I can see it in the console
            
			lineOriginV3 = transform.position; // trying to get my own position as a vector3
 
			// this is where I wanted to smooth the origin - but I failed
			//lineOriginV3 = Vector3.MoveTowards(lastLineOriginPosition,transform.position,Time.deltaTime*FollowSpeed);
            
			lastLineOriginPosition = lineOriginV3; // store my line origin so I can use it later for smoothing
            
			lineTargetV3 = target.position; // set the target position
 
			// this is where i wanted to smooth the target - but I railed
			//lineTargetV3 = Vector3.MoveTowards(lastLineTargetPosition,transform.position,Time.deltaTime*TargetFollowSpeed);
 
			origin = Origins.Start; // ??
			lengh = (lineOriginV3 - lineTargetV3).magnitude; // maybe something where I'm setting the direction?
			transform.LookAt(lineTargetV3); // ??
			transform.Rotate(altRotation, -90, 0);
 
			if (ampByFreq) { // line amplitude stuff
				ampT = Mathf.Sin(freq*Mathf.PI);
			}
			else {
				ampT = 1;
			}
			ampT = ampT * amp;
			if (warp && warpInvert) {
				ampT = ampT/2;
			}
 
			// if the distance between the line targetv3 and the previous line target position is 
			// greater than minimimum distance, run a distance event (?)
			if(Vector3.Distance(lineTargetV3,lastLineTargetPosition) > minimumDistance){
				//Debug.Log("distance event");
				distanceEvent.Invoke();
			}
            
			lastLineTargetPosition = lineTargetV3; 
        
			for (int i = 0; i < size; i++) { // iterating through each element in the line??
				angle = (2*Mathf.PI/ size1 * i*freq);
				if (centered) {
					angle -= freq*Mathf.PI;     //Center
					if (centCrest) {
						angle -= Mathf.PI/2;    //Crest/Knot
					}
				}
				else {centCrest = false;}
            
				walkShift -= walkAuto/ size1 * Time.deltaTime;
				angle += (float)walkShift - walkManual;
				sinAngle = Mathf.Sin(angle);
				if (spiral) {
					sinAngleZ = Mathf.Cos(angle);
				}
				else {
					sinAngleZ = 0;
				}
            
				if (origin == Origins.Start) {
					start = 0;
				}
				else {
					start = lengh/2;
				}
                
				if (warp) {
					warpT = size1 - i;
					warpT = warpT / size1;
					warpT = Mathf.Sin(Mathf.PI * warpT * (warpRandom+1));
					if (warpInvert) {warpT = warpT==0?999999999:1/warpT;}
					myLine.SetPosition(i, new Vector3(lengh/ size1 * i - start, sinAngle * ampT * warpT, sinAngleZ * ampT * warpT));
				}
				else {
					myLine.SetPosition(i, new Vector3(lengh/ size1 * i - start, sinAngle * ampT, sinAngleZ * ampT));
					warpInvert = false;
				}
 
				if (i == 1) { 
					posVtx2 = new Vector3(lengh / size1 * i - start, sinAngle * ampT * warpT, sinAngleZ * ampT * warpT); 
				}
				if (i == size-2) {
					posVtxSizeMinusOne = new Vector3(lengh / size1 * i - start, sinAngle * ampT * warpT, sinAngleZ * ampT * warpT); 
				}
			} 
			if (warpInvert)
			{  //Fixes pinned limits when WarpInverted
				myLine.SetPosition(0, posVtx2);
				myLine.SetPosition(size-1, posVtxSizeMinusOne);
			}       
            
		} else {
			myLine.enabled = false;
		}
        
		closestDistance += Time.deltaTime;
	}
    
	public void PopulateHands(){
		allOtherHands = new List<GameObject>(); // make a list 
		var hands = GameObject.FindGameObjectsWithTag("hand"); // of all the items with the tag "hand"
		// this doesn't work properly, it includes "this" hand
		allOtherHands = hands.Where(h => h != this.gameObject).ToList();
		Debug.Log(allOtherHands);
	}
    
	private void ChooseTarget(){
		// if there are hands in the scene? When would all other hands be null??
		if(allOtherHands != null && allOtherHands.Count > 0){
			Animator myParentAnimator = GetComponentInParent<Animator>();
			// for each hand (let's call it cuteHand)
			foreach(GameObject cuteHand in allOtherHands) {
				// check the distance from me (the current game object) to cuteHand
				float distance = Vector3.Distance(transform.position, cuteHand.transform.position);
				// get the parent of cuteHand (ultimately this will be to make sure it doesn't belong to me)
				Animator cuteHandParentAnimator = cuteHand.GetComponentInParent<Animator>();
				// if we haven't identified my opposing hand (left or right) yet
				if(myOpposingHand == null){
					// if I share a parent animator with cuteHand and cuteHand isn't me
					if(myParentAnimator == cuteHandParentAnimator && cuteHand != transform.parent.gameObject)
					{
						// then cuteHand must be my opposing hand!
						myOpposingHand = cuteHand;
					}
				}

				// if cuteHand is NOT my opposing hand AND cutehand is NOT me AND cutehand IS active
				if(cuteHand != myOpposingHand && cuteHand != transform.parent.gameObject && cuteHand.gameObject.activeInHierarchy){
					// and if distance is more than the closest distance
					if(distance <= closestDistance){
						// set the closest distance to distance (why?)
						closestDistance = distance;
						// if cutehand has a parent animator (why?)
						if(cuteHandParentAnimator != null){
							//set the target to cutehand
							target = cuteHand.transform;
							//Debug.Log($"Drawing line between {myParentAnimator.gameObject.name} and {cuteHandParentAnimator.gameObject.name}");
						}   
					}
				}
                
				//if(cuteHandParentAnimator !=null)
				//Debug.Log($"Hand {hand.name} of {cuteHandParentAnimator.gameObject.name} is {distance}m away");
			}

			// if target is not null and target is not active (what is &&!? )
			if(target != null && !target.gameObject.activeInHierarchy){
				//Debug.Log("reset target");

				// set the target to null
				target = null;
				// change the clostest distance to something impossible
				closestDistance = 100;
			}
            
		}
	}
}