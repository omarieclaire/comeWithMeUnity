using System.Collections;
using System.Collections.Generic; // need this for lists
using System.Collections.Concurrent;
using UnityEngine;
using OscJack;
using RootMotion.FinalIK;

	////////////////////////////////////////////////////////
	// WHAT IS HAPPENING IN THIS MONSTER FILE ANYWAY??!! //
	////////////////////////////////////////////////////////

	// 1. First: I receive each OSC Message using OSC Jack (it's just a string, one example: livepose/skeletons/0/0/keypoints/RIGHT_RING_FINGER2 fff 370.167511 253.175644 0.810268)
	// 2. Then I extract the address (string), coords (x,y,z values). z is a bit special!
	// 3. Then I constantly add these address (key) and coord (value) pairs to a special threadsafe dictionary (I bundle in the time "System.DateTime.Now.Ticks")
	// 4. Then, in the update function (every frame) I:
	//		- Deactivate any missing players (players who have left the screen)
	//		- Loop through my threadsafe dictionary. For each item in the dictionary I:
	//			- grab the address, coords, and time
	//			- add new players when needed (I create a new player and/or grab a player's number)
	//			- add the body part to the player's "addressBodyPartMap" if it's a new body part (or make spheres)
	//			- reactivate any players when needed (if they were gone and have reappeared)
	//			- check and update all my weird clocks/timestamps (related to deactivating and reactivating players)
	//			- if the player is active, I update body part positions by translating the LivePose coordinates to the Unity coordinates
	
	// Also useful to know?

	// - I have a playerRig class that I use to create players
	// - I have a LIST of players that stores each playerRig
	// - 
	// - 
	// - 
	
	
	
	/////////////////////
	// OPEN QUESTIONS //
	////////////////////
	
	// CAMERA WEIRDNESS
	// Why do I need to rotate the camera Y 180?
	
	
	/// <summary>
	///  do I want to use mm pose and get the face and hands and have a buggy stream?
	///  or trt and not get the face and hands and have a smoohter stream
	/// </summary>
	/// 
	// I NEED TO FIX MY FILTER, I'M BYPASSING MY FILTER WHEN I PASS POSITIONS TO THE MAP
	// ALSO, IF I GET A HAND ONCE, DO I GET IT FOREVER?
	
	
	///////////////
	// TODO!!!! //
	//////////////
	
	// dummy is the wrong size.
	// player is the right size
	// dummy should be bigger when they are closer to the camera
	// but when I make them bigger it messes with their placement on the screen
	
	
	// - player is jumping forward in space. 
	// Am I not getting depth data quickly enough, and the player is reverting to zero? 
	// Or am I getting 0s?
	// I could track the depth only for a bit on the log?
	// why does the z suddenly shoot forward from a -4 to a 2 and a -4 to a 6?
	// can I log the depth only?
	// How much is the jump
	// I could smooth the depth?
	
	
	// why do the feet suddently shoot over the head?
	// why does the foot y suddently shoot from a 0.something to a 2 when I get too close to the camera?
	// the  livepose vizualizer foot DOES not jump
	// the dummy foot does jump
	// the player foot does jump
	
	
	
	// how to solve the depth problem?
	// how to solve the "player twisting" problem"?
	// how to solve the jump scare problem?
	
	
 
	// - make connections between players when they reach.
	//		- make connections pretty
	
	// - create characters in blender.
	//		- rig
	//		- animate
	
	// make the world look like a world
	
	// consider adjusting the map function based on the player depth
	// - the x and y map should shrink based on the z.
	
	
	
	// continue tweaking the FinalIK - https://www.youtube.com/watch?v=tgRMsTphjJo

	
	// - make generated flying objects
	//		- flying objects collide with player and do something to player

	// - there is an animation before a player disappears. If they are far they dissolve, if they are near they explode, if they are still they become transparent
	//		- If they are far they dissolve
	//		- if they are near they explode.
	//		- if they are still they become transparent
	
	// - each player has a unique sound
	//		- connected players have shared sounds
	
	// - words on screen? deeplll?
	//		- ?
	
	// - camera follows player?
	//		- ?
	
		
	// - make world pretty
	//		- ?
		
	// - I'm curious to move some of the stuff out of the update function and into their own functions, but I have to do it from the metalab

		

public class CustomOSC : MonoBehaviour
{
    
	public FullBodyBipedIK myIK;
	
	public Material debugSphereMaterial;
	
	[SerializeField]
	public GameObject depthSphere;

	[SerializeField]
	public GameObject debugSphere;
	    
	public float speed = 0.15f;
	public float timeToWaitForMissingPlayers = 0.5f;

	[SerializeField]
	public float minConfidenceAllowed = 0f;

	[SerializeField]
	public float depthFactorial = 0.3f;
	
	[SerializeField]
	public float maxAllowedDepthJumping = 2f; // because of a bug with livepose that makes it jump around

	
	[SerializeField]
	public float oscDepthMin = 0f;
	
	[SerializeField]
	public float oscDepthMax = 7f;
	
	[SerializeField]
	public float unityDepthMin = 8f;
	
	[SerializeField]
	public float unityDepthMax = -8f;
	
	
	[SerializeField]
	public float oscYMin = 0f;
	
	[SerializeField]
	public float oscYMax = 480f;
	
	[SerializeField]
	public float unityYMin = 4.6f;
	
	[SerializeField]
	public float unityYMax = 0f;
	
	
	[SerializeField]
	public float oscXMin = 0f;
	
	[SerializeField]
	public float oscXMax = 640f;
	
	[SerializeField]
	public float unityXMin = 3f;
	
	[SerializeField]
	public float unityXMax = -3f;
	

	
	[SerializeField]
	public float player0Y = 0f;
	
	[SerializeField]
	public float player0X = 0f;


	[SerializeField]
	private GameObject dummyPrefab;
	
	[SerializeField]
	private GameObject playerPrefab;
	
	[SerializeField]
	private GameObject delightBall;
	
	[SerializeField]
	private GameObject sadSquare;

	[SerializeField]
	private bool enableSmoothing = true;
    
	//[SerializeField]
	//private List<GameObject> nose = new List<GameObject>();
	
	///////////////////////////
	// PLAYER RIG CLASS!!!! //
	//////////////////////////
	
	public class BodyPart{
		public GameObject gameObject;
		public ConcurrentQueue<Vector3> positionHistory;
		public float averageX = 0f;
		public float averageY = 0f;
	}
    
	public class PlayerRig{
		public GameObject dummy;
		public GameObject playerTransform; //transform is the main way to scale, rotate and position a gameobject
		public bool active;
		public GameObject nose;
		public GameObject leftHip; 
		public GameObject rightHip; 
		public GameObject pelvis;
		public GameObject chest;
		public GameObject rightToe;
		public GameObject leftToe;
		public GameObject leftWrist;
		public GameObject rightWrist;
		public GameObject leftElbow;
		public GameObject rightElbow;
		public GameObject leftAnkle;
		public GameObject rightAnkle;
		public GameObject leftKnee;
		public GameObject rightKnee;
		//public List<Vector3> positionHistory = new List<Vector3>(new Vector3[10]);
		
		//public Dictionary<string, GameObject> addressBodyPartMap;

		public ConcurrentDictionary<string, BodyPart> addressBodyPartMap;
		public float lastUpdated;
		public long lastOSCTimeStamp;
		public int playerNumber;
		public float lastPlayerDepth = 999; //an impossible number
		public float playerDepth;		
	}

		
	////////////////////////////
	// MY LIST OF PLAYERS!!!! //
	////////////////////////////
	
	// hold all the players in a list, which is basically a dynamically sized array
	// - can access items in a list in the same way as an array 
	// - can find the length: players.length
	// - can add to it: .add(new PlayerRig("Whatever", 50));
	// - remove an element at an index and move everything down a place - list.RemoveAt(0)
	//public List<PlayerRig> players = new List<PlayerRig>();
	public ConcurrentDictionary<int, PlayerRig> players = new ConcurrentDictionary<int, PlayerRig>();
	
	
	//////////////////
	// OSC SERVER! //
	/////////////////
	
	OscJack.OscServer server;

	////////////////////////////
	// MY SPECIAL DICTIONARY! //
	////////////////////////////
	
	// my special "threadsafe" dictionary - mapping address to coordinates (and a timestamp)
	ConcurrentDictionary<string, (Vector3, long)> addressCoordinateAndTimeStampMap = new ConcurrentDictionary<string,(Vector3, long)>();
			
	
	////////////////////////////
	// START! LET'S GO OSC!!! //
	////////////////////////////

	void Start() {
        server = OscMaster.GetSharedServer(12000);
        server.MessageDispatcher.AddCallback(string.Empty, OscReceiver1); 
    }
    
    
	////////////////////////////////////
	// GET PLAYER NUMBER FROM STRING //
	//////////////////////////////////
	
	int GetPlayerNumber(string address) {
		string[] splitString = address.Split('/');
		if(splitString.Length > 3)
		{
			 return int.Parse(splitString[4]);
		} else
		{
			return -1;
		}		
	}
	
	//////////////////////////
	// ADD TAG RECURSIVELY //
	/////////////////////////
	void AddTagRecursively(Transform trans, string tag)
	{
		trans.gameObject.tag = tag;
		if(trans.childCount > 0)
			foreach(Transform t in trans)
				AddTagRecursively(t, tag);
	}
	
	//////////////////////////////////////////////////////
	// GET PELVIS FROM HIPS / GET CHEST FROM SHOULDERS //
	////////////////////////////////////////////////////
	
	Vector3 GetCenterBetweenTwoBodyParts (Vector3 left, Vector3 right)
	{
		Vector3 midpoint = new Vector3((left.x + right.x) / 2.0f, (left.y + right.y) / 2.0f, (left.z + right.z) / 2.0f);
		return midpoint;
	}

	
	////////////////////////////////
	// DECTIVATE MISSING PLAYERS //
	//////////////////////////////
	
	void CheckForMissingPlayers(){
		// loop through all the players
		foreach (PlayerRig rig in players.Values) {
			if (rig.active)
			{
				//if the UNITY time minus the time of creation is greater than 1 second
				float elapsed = Time.time - rig.lastUpdated;
				
				if (elapsed > timeToWaitForMissingPlayers)
				{
					//Debug.Log($"deactivating {i} elapsed: {elapsed}");
					rig.active = false;
					rig.dummy.SetActive(false);
					rig.playerTransform.SetActive(false);
				}			
			}
		}		
	}
	
	
	PlayerRig MakeNewPlayerOrUpdatePlayer(string address, long oscTime){
		// get the player number by extracting it from the string
		int playerNumber = GetPlayerNumber(address);
		
		if(players.ContainsKey(playerNumber)) {
			return players[playerNumber];
		} else {
			// player corresponding to playerNumber does not exist
			// therefore we make a new rig
			PlayerRig playerRig = AddPlayer(playerNumber, oscTime);
			// Adds a new thing if the key doesn't exist.
			// If the key exists, update it with the function (key, oldValue) => playerRig
			// (for our update we just replace the old value)
			players.AddOrUpdate(playerNumber, playerRig, (key, oldValue) => playerRig);
			return playerRig;
		}
		
	}
		

    void Update()
	{
		// First, deactivate any missing players
		CheckForMissingPlayers();
    	    
		// Next, loop through the "safe" dictionary (osc address string: vector3 coordinates, time) and process each key value pair
        foreach (KeyValuePair<string, (Vector3, long)> addressCoordinateTimeStampKVPair in addressCoordinateAndTimeStampMap)
        {
	        // Extract the osc address string, the osc vector 3 coordinate, and the osc timestamp
	        string address = addressCoordinateTimeStampKVPair.Key;
            (Vector3, long) coordinateAndTimeStamp = addressCoordinateTimeStampKVPair.Value;
            Vector3 coords = coordinateAndTimeStamp.Item1;
	        long oscTime = coordinateAndTimeStamp.Item2;
	        
	        ///////////////////////////////
	        // ADD NEW PLAYER AS NEEDED //
	        /////////////////////////////
	        
	        // grab the relevant player (if the player number is new, add a player - if not, update the player)
	        PlayerRig playerRig = MakeNewPlayerOrUpdatePlayer(address, oscTime);
	        
	        /////////////////////////////////////////////////////
	        // ADD NEW BODY PARTS TO THE ADDRESS BODY PART MAP //
	        ////////////////////////////////////////////////////

	        // if it's a NEW body part - if I haven't seen this this body part for this player before
	        if(!playerRig.addressBodyPartMap.ContainsKey(address)){
		        // grab the relevant body-part-object from the player rig (or a sphere if there is no relevant body part)
	        	GameObject rigBodyPartObject = OSCBodyPartAssigner(address, playerRig);
	        	// set the body part name to the OSC string so I can identify it in the scene
	        	rigBodyPartObject.name = address;
	        	// create a body part using the body part class
	        	// we're passing it the game object and an empty queue (this queue will hold the history of vectors)
	        	BodyPart bodyPart = new BodyPart();
	        	bodyPart.gameObject = rigBodyPartObject;
	        	bodyPart.positionHistory = new ConcurrentQueue<Vector3>();
	        	// add the body part to my map
	        	playerRig.addressBodyPartMap.AddOrUpdate(address, bodyPart, (k,v) => v = bodyPart);
	        } 
	        

	        //////////////////////
	        // REACTIVATE PLAYER//
	        /////////////////////
	                
	        // if the player rig is currently NOT active and I got the nose and ??the time is different than now??
	        if(!playerRig.active && address.Contains("FACE_38") && playerRig.lastOSCTimeStamp != oscTime){
	        	//reactivate player
		        //Debug.Log($"reactivating {playerRig.playerNumber}");
		        playerRig.active = true;
		        playerRig.dummy.SetActive(true);
		        playerRig.playerTransform.SetActive(true);	
	        }
	        
	        ////////////////////////////////////////////////////////////
	        // TIME STUFF - KNOW WHEN TO ACTIVATE/DEACTIVATE PLAYERS //
	        ///////////////////////////////////////////////////////////
	        
	        if (playerRig.active)
	        {
	        	//WHAT IS UP WITH THIS NOSE STUFF? I think I was trying to deal with missing body parts
	        	if(address.Contains("FACE_38")){
		        	//Debug.Log($"player number {playerRig.playerNumber}");
		        	//Debug.Log($"last osc: {playerRig.lastOSCTimeStamp} | current: {oscTime}");
		        	//Debug.Log($"last updated: {playerRig.lastUpdated } | time: {Time.time}");		
		        	//Debug.Log($"player postion {coords}");

		        	if (playerRig.lastOSCTimeStamp != oscTime)
		        	{
			        	playerRig.lastOSCTimeStamp = oscTime;
			        	playerRig.lastUpdated = Time.time;
		        	}
	        	}
	        	
	        	////////////////////////////////////////////////////////////////
		        // UPDATE BODY PART POSITIONS SO THE PLAYER-DUMMY WILL MOVE //
		        //////////////////////////////////////////////////////////////
	        	
	        	// get the old vector3 for a given body part (by looking up the osc string in the addressBodyPartMap)
	        	BodyPart bodyPart = playerRig.addressBodyPartMap[address];
	        	GameObject bodyPartGameObject = bodyPart.gameObject;
	        
		        //playerRig.dummy.transform.transform.rotation = Quaternion.identity;
	        
		        // get the "target" position for a given body part using the map function (osc xyz to unity xyz)

				Vector3 target;
				if(enableSmoothing)
				{
					target = MapOSCCoordToUnityCoord(bodyPart.averageX, bodyPart.averageY, playerRig.playerDepth);
				} else
				{
					target = MapOSCCoordToUnityCoord(coords[0], coords[1], playerRig.playerDepth);
				}
		        
		        // account for depth dimensions in the OSC to unity mapping - it's a 3d world!
		        if(playerRig.playerDepth > 0)
		        {
			        float depthFactor = depthFactorial * playerRig.playerDepth;
			        target = new Vector3(target[0] * depthFactor, target[1], target[2]);
		        }	
			    
		        //so the feet don't go above the head - annoying bug
		        
		        if(address.Contains("TOE")) 
		        {
			        Debug.Log("got a toe");
			        target[1] = 0;
		        }
		        
		        //Debug.Log(playerRig.playerDepth);
		        
		        
		        //depthSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		        //depthSphere.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
		        //depthSphere.GetComponent<Renderer>().material = debugSphereMaterial;
				
		        Vector3 p = transform.position;
		        p.y = playerRig.playerDepth;
		        

		        //depthSphere.transform.position = new Vector3(0,0,playerRig.playerDepth * -1);
		        
		        // use lerp to smooth out the movement - NOTE is this making the movements inaccurate?
		        bodyPartGameObject.transform.position = Vector3.Lerp(bodyPartGameObject.transform.position, target, speed);  
		        //bodyPart.transform.position = target;
	        }
     
        }
        
		UpdatePlayersPelvis();
        
	}
    
    
	/////////////////////////////////////////////////////////
	// Use Map function to map OSC coords to Unity coords //
	////////////////////////////////////////////////////////
	Vector3 MapOSCCoordToUnityCoord(float x, float y, float z)
	{		
		float unityX = MapToRange(x, oscXMin, oscXMax, unityXMin, unityXMax);
		float unityY = MapToRange(y, oscYMin, oscYMax, unityYMin, unityYMax);
		float unityZ = MapToRange(z, oscDepthMin, oscDepthMax, unityDepthMin, unityDepthMax);
		
		Vector3 unityCoords = new Vector3(unityX, unityY, unityZ);
		return unityCoords;
		
	}
	
	//////////////////////////////////////////////////////
	// Get and update the player pelvis for each player //
	/////////////////////////////////////////////////////
	
	// The pelvis is special because it doesn't actually exist. I'm reusing the hip coords to get it working.
	
	void UpdatePlayersPelvis() {
		foreach(PlayerRig player in players.Values) {
			Vector3 leftHipCoord = player.leftHip.transform.position;
			Vector3 rightHipCoord = player.rightHip.transform.position;
			Vector3 pelvisCoord = GetCenterBetweenTwoBodyParts(leftHipCoord, rightHipCoord);
			player.pelvis.transform.position = Vector3.Lerp(player.pelvis.transform.position, pelvisCoord, speed);
		}
	}
    
    
	//////////////////////////////////////////////////////
	// MAKE A NEW PLAYER - RIG, DUMMY, ISACTIVE, MORE ///
	/////////////////////////////////////////////////////
	
	public PlayerRig AddPlayer(int playernum, long oscTime) {
		PlayerRig playerRig = new PlayerRig(); // make a new playerRig using my playerRig class
		playerRig.addressBodyPartMap = new ConcurrentDictionary<string, BodyPart>(); //make a new addressBodyPartMap - no v3s, just the links
		GameObject dummy = Instantiate(dummyPrefab); //Instantiate a dummy
		dummy.name = $"Dummy {playernum}"; //set the dummy prefab's name name to the player num
		
		string dummyTag = $"Player{playernum}";
		//dummy.tag = dummyTag;
		AddTagRecursively(dummy.transform, dummyTag);

			
		VRIK vrik = dummy.GetComponent<VRIK>(); //grab the VRIK component
		//myIK = dummy.GetComponent<FullBodyBipedIK>(); //grab the IK component

		GameObject player = Instantiate(playerPrefab); //instantiate a player transform
		player.name = $"Player {playernum}"; //set the player prefab's name to the player num
		
		string playerTag = $"Player{playernum}";
		AddTagRecursively (player.transform, playerTag);
		
		PlayerTransform playerTransform = player.GetComponent<PlayerTransform>(); //get the player's transform (position, scale, depth)
		
		// Connect the playerRig's game objects to the actual body part of the player that is in the scene. why?? 
		// example: The nose of the playerRig is whereever that players head is in the scene?
		// I think this only happens once on setup?
		
		//attaching the "nose" key to the empty "head" game objects that live inside the "player" (ghost/puppeteer)
		playerRig.nose = playerTransform.head;
		playerRig.pelvis = playerTransform.pelvis; //special
		playerRig.leftHip = playerTransform.leftHip; 
		playerRig.rightHip = playerTransform.rightHip;

		playerRig.chest = playerTransform.chest;
		playerRig.rightToe = playerTransform.rightToe;
		playerRig.leftToe = playerTransform.leftToe;
		playerRig.leftWrist = playerTransform.leftWrist;
		playerRig.rightWrist = playerTransform.rightWrist;
		playerRig.leftAnkle = playerTransform.leftAnkle;
		playerRig.rightAnkle = playerTransform.rightAnkle;		
		playerRig.rightElbow = playerTransform.rightElbow;
		playerRig.leftElbow = playerTransform.leftElbow;
		playerRig.rightKnee = playerTransform.rightKnee;
		playerRig.leftKnee = playerTransform.leftKnee;

		// connect the "dummy" in a playerRig to the actual dummy in the scene???
		playerRig.dummy = dummy;
		
		//dummy.transform.transform.rotation = Quaternion.identity;

		
		// assign the "playerTransform" key to the "player" value (ghost/puppeteer)
		playerRig.playerTransform = player;

		// Connect the VRIK solver to the playerTransforms?????
		vrik.solver.spine.headTarget = playerTransform.head.transform;
		vrik.solver.leftArm.target = playerTransform.leftWrist.transform;
		vrik.solver.rightArm.target = playerTransform.rightWrist.transform;
		
		vrik.solver.spine.pelvisTarget = playerTransform.pelvis.transform;
		//vrik.solver.spine.chestGoal = playerTransform.chest.transform;
				
		vrik.solver.leftLeg.target = playerTransform.leftToe.transform;
		vrik.solver.rightLeg.target = playerTransform.rightToe.transform;
		
		//playerRig.pelvis = getCenterBetweenTwoBodyParts(playerRig.leftHip, playerRig.rightHip);

		//myIK.solver.bodyEffector.target = playerTransform.pelvis.transform;
		//myIK.solver.rightFootEffector.target = playerTransform.rightToe.transform;
		//myIK.solver.leftFootEffector.target = playerTransform.leftToe.transform;
		//myIK.solver.rightHandEffector.target = playerTransform.rightWrist.transform;
		//myIK.solver.leftHandEffector.target = playerTransform.leftWrist.transform;
		
	
		playerRig.active = true; // player rig is set to active because it was just set up
		playerRig.lastUpdated = Time.time; // the lastupdated time should be set to now
		playerRig.lastOSCTimeStamp = oscTime; // the OSC time is set to the ticks time we passed in when receiving the message
		playerRig.playerNumber = playernum; // add player number
		return playerRig;
		
	} 
	
	
	
    
	/////////////////////////////////////////////////////////////////
	// LINK SPECIFIC OSC MESSAGES TO SPECIFIC PLAYER BODY PARTS  ///
	//   only called if the address has NOT been seen before      ///
	////////////////////////////////////////////////////////////////
	
	// This is used by map#2. Both my maps use strings as keys. Map #2 connects OSC strings to game objects (example rig.leftWrist)
	// This function determines what object is related to a given string address
	
	public GameObject OSCBodyPartAssigner(string address, PlayerRig rig) {
		
		GameObject thisObject;
		
		if (address.Contains("FACE_38")) {
			thisObject = rig.nose;
			// update the time the player was last updated - should I be doing this for all body parts?!!!			
			// Debug.Log($"lastUpdated {rig.lastUpdated}");			
			// Debug.Log($"lastOSCTimeStamp {rig.lastOSCTimeStamp}");
			rig.lastUpdated = Time.time;
		} else if (address.Contains("RIGHT_MIDDLE_FINGER3")) { 
			thisObject = rig.rightWrist;
		}  else if (address.Contains("LEFT_MIDDLE_FINGER3")) { 
			thisObject = rig.leftWrist;
		
		} else if (address.Contains("RIGHT_KNEE")) { 
			thisObject = rig.rightKnee;
		}  else if (address.Contains("LEFT_KNEE")) { 
			thisObject = rig.leftKnee;
				
		} else if (address.Contains("RIGHT_ELBOW")) { 
			thisObject = rig.rightElbow;
		}  else if (address.Contains("LEFT_ELBOW")) { 
			thisObject = rig.leftElbow;
				
		} else if (address.Contains("LEFT_HIP")) { 
			thisObject = rig.leftHip;

		} else if (address.Contains("RIGHT_HIP")) { 
			thisObject = rig.rightHip;	
			
		}  else if (address.Contains("NECK")) { 
			thisObject = rig.chest;
			
		} else if (address.Contains("RIGHT_BIG_TOE")) { 
			thisObject = rig.rightToe;
		}  else if (address.Contains("LEFT_BIG_TOE")) { 
			thisObject = rig.leftToe;
			
						
		//} else if (address.Contains("RIGHT_ANKLE")) { 
		//	thisObject = rig.rightAnkle;
		//}  else if (address.Contains("LEFT_ANKLE")) { 
		//	thisObject = rig.leftAnkle;
		
		//} else if (address.Contains("RIGHT_MIDDLE_FINGER3")) { 
		//	thisObject = rig.leftWrist;
		//}  else if (address.Contains("LEFT_MIDDLE_FINGER3")) { 
		//	thisObject = rig.rightWrist;
		
		//} else if (address.Contains("RIGHT_KNEE")) { 
		//	thisObject = rig.leftKnee;
		//}  else if (address.Contains("LEFT_KNEE")) { 
		//	thisObject = rig.rightKnee;
				
		//} else if (address.Contains("RIGHT_ELBOW")) { 
		//	thisObject = rig.leftElbow;
		//}  else if (address.Contains("LEFT_ELBOW")) { 
		//	thisObject = rig.rightElbow;
				
		//} else if (address.Contains("MID_HIP")) { 
		//	thisObject = rig.pelvis;	
		//}  else if (address.Contains("NECK")) { 
		//	thisObject = rig.chest;
			
		//} else if (address.Contains("RIGHT_BIG_TOE")) { 
		//	thisObject = rig.leftToe;
		//}  else if (address.Contains("LEFT_BIG_TOE")) { 
		//	thisObject = rig.rightToe;
			
		//} else if (address.Contains("RIGHT_ANKLE")) { 
		//	thisObject = rig.leftAnkle;
		//}  else if (address.Contains("LEFT_ANKLE")) { 
		//	thisObject = rig.rightAnkle;
						
		} else {
			// if I can't find a relevant body part, just make a sphere
			thisObject = Instantiate(debugSphere);
			thisObject.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
			//thisObject.renderer. = debugSphereMaterial;
			thisObject.GetComponent<Renderer>().material = debugSphereMaterial;

			// I can't remember why I do this parenting step
			thisObject.transform.parent = rig.playerTransform.transform;
        }
		return thisObject;
    }

	//////////////////////////////////////////////////////////////
	// MAP THE OSC VALUES (X,Y,Z) TO THE UNITY VALUES (X,Y,Z) ///
	////////////////////////////////////////////////////////////
	
    
	public float MapToRange(float n, float start1, float stop1, float start2, float stop2){
		return ((n-start1)/(stop1-start1))*(stop2-start2)+start2;
	}
   
    
	//public void ObjectGenerator(){
		//https:docs.unity3d.com/Manual/InstantiatingPrefabs.html
		//Instantiate (delightBall, new Vector3(Random.Range(1,10), Random.Range(1,10), Random.Range(1,10)), Quaternion.identity);
	//}
 
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// RECEIVE OSC MESSAGES PORT 12000 & SEND EACH MESSAGE (osc string, v3, and osctime) TO MY THREADSAFE MAP  ///
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	
	void OscReceiver1 (string address, OscDataHandle data){
		// receive OSC message and update the values (coordinates) for the keys (addresses)
		// it would be much easier if I could just update the coordinates HERE in the osc receiver
		// but I can't create a gameobject outside of the update function 
		
		long currentTime = System.DateTime.Now.Ticks;	
		Vector3 vec = new Vector3(data.GetElementAsFloat(0), data.GetElementAsFloat(1));
		
		int playerId = 0;
		if (address.Contains("livepose/pyrealsense_head_follower/0") || address.Contains("livepose/skeletons/0"))
		{
			playerId = GetPlayerNumber(address); // extract the player number from the OSC string

		}
		

		// make sure I don't try to set a depth value before I've seen this player
		if(players.ContainsKey(playerId))
		{
				PlayerRig currPlayer = players[playerId]; //grab the current player

				// head follower depth nonsense
				if (address.Contains("livepose/pyrealsense_head_follower/0") && data.GetElementAsFloat(2) != 0.000000)
				{
					float z = data.GetElementAsFloat(2); // extract the depth from the OSC data - which looks like this   livepose/position/0/1 fff -0.212375 0.803258 2.666000
							
					
					if (currPlayer.lastPlayerDepth == 999){
						currPlayer.lastPlayerDepth = z; // the first time is tricky
					} else {
						currPlayer.lastPlayerDepth = currPlayer.playerDepth; // go store current depth in "last depth so its there when we look for it"				
					} if (Mathf.Abs(currPlayer.lastPlayerDepth - z) > maxAllowedDepthJumping) // if we are jumping too much
					{
						// do nothing
					} else {
						currPlayer.playerDepth = z;	
					}
				}				
			// low pass filter to average out the values and jumpiness
			// so I either need to make a map for this or I need to make a Tupple and store it in the existing address-bodypart map
			if (currPlayer.addressBodyPartMap.ContainsKey(address)){
				BodyPart bodyPart = currPlayer.addressBodyPartMap[address];
				bodyPart.positionHistory.Enqueue(vec);
				if(bodyPart.positionHistory.Count > 10) {
					Vector3 tmp;
					bodyPart.positionHistory.TryDequeue(out tmp);
				}
				// now calculate the average
				Vector3 average = new Vector3();
				foreach(var v in bodyPart.positionHistory) {
					average.x = average.x + v.x;
					average.y = average.y + v.y;
				}
				bodyPart.averageX = average.x / bodyPart.positionHistory.Count;
				bodyPart.averageY = average.y / bodyPart.positionHistory.Count;
			}
		} 
		
	
		//If it's a skeleton-position message, which looks like this livepose/skeletons/0/0/keypoints/RIGHT_RING_FINGER2 fff 370.167511 253.175644 0.810268
		if (address.Contains("livepose/skeletons/0") && data.GetElementAsFloat(2) >= minConfidenceAllowed)
		 //&& data.GetElementAsFloat(2) > 0.5f
		{		
			//Debug.Log(data.GetElementAsFloat(2));
			//Update a special threadsafe map of all the OSC Messages (a string, and a vector3-time tupple 
			addressCoordinateAndTimeStampMap.AddOrUpdate(address, (vec, currentTime), (k,v) => v = (vec, currentTime));
		}
    }
}

//https://gitlab.com/sat-metalab/livepose/-/blob/main/livepose/keypoints/hand_21.py
//https://gitlab.com/sat-metalab/livepose/-/blob/main/livepose/keypoints/hand_21.py
//https://gitlab.com/sat-metalab/livepose/-/blob/main/livepose/keypoints/face_70.py
//https://gitlab.com/sat-metalab/livepose/-/blob/main/livepose/keypoints/body_25.py


//     NOSE = 0
//     NECK = 1
//     RIGHT_SHOULDER = 2
//     RIGHT_ELBOW = 3
//     RIGHT_WRIST = 4
//     LEFT_SHOULDER = 5
//     LEFT_ELBOW = 6
//     LEFT_WRIST = 7
//     MID_HIP = 8
//     RIGHT_HIP = 9
//     RIGHT_KNEE = 10
//     RIGHT_ANKLE = 11
//     LEFT_HIP = 12
//     LEFT_KNEE = 13
//     LEFT_ANKLE = 14
//     RIGHT_EYE = 15
//     LEFT_EYE = 16
//     RIGHT_EAR = 17
//     LEFT_EAR = 18
//     LEFT_BIG_TOE = 19
//     LEFT_SMALL_TOE = 20
//     LEFT_HEEL = 21
//     RIGHT_BIG_TOE = 22
//     RIGHT_SMALL_TOE = 23
//     RIGHT_HEEL = 24

//Forearm Elbow
//UpperArm Shoulder
//Spine3 Chest
//Spine1 Bellybutton
//Pelvis Pelvis
//Hand Wrist
//Head upper neck
//Thigh - leg connector
//R Calf - knee
//Foot ankle connector




