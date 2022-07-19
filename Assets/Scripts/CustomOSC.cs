using System.Collections;
using System.Collections.Generic; // need this for lists
using System.Collections.Concurrent;
using UnityEngine;
using OscJack;
using RootMotion.FinalIK;

	
	/////////////////////
	// OPEN QUESTIONS //
	////////////////////
	
	// CAMERA WEIRDNESS
	// Why do I need to rotate the camera Y 180?
	
	
	
	// turned off "is kinematic" on hands
	
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
	static public bool usingMMPose = false;
	
	[SerializeField]
	public GameObject depthSphere;

	[SerializeField]
	public GameObject debugSphere;
	
	[SerializeField]
	    
	public float speed = 0.15f;

	[SerializeField]
	public float timeToWaitForMissingPlayers = 0.5f;
	
	[SerializeField]
	public float timeToWaitForMissingFeet = 0.5f;

	[SerializeField]
	public float minConfidenceAllowed = 0f;
	
	[SerializeField]
	public float minWristConfidenceAllowed = 0f;

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
 
	
	////////////////////////////////////
	// GET PLAYER NUMBER FROM STRING //
	//////////////////////////////////
	
	static int GetPlayerNumber(string address) {
		string[] splitString = address.Split('/');
		if(splitString.Length > 3)
		{
			return int.Parse(splitString[4]);
		} else
		{
			return -1;
		}		
	}
	
	///////////////////////////
	// PLAYER RIG CLASS!!!! //
	//////////////////////////
	
	// Class to hold data for all skeleton position messages
	// These messages differ from "head follower" messages because they have a position (x and y)
	// (head follower messages only have a depth)
	public class SkeletonPositionMessage
	{
		public string address;
		public int playerId;
		public Vector3 position;
		public float threshold; 
		
		public SkeletonPositionMessage(string add, OscDataHandle data)
		{
			Vector3 vec = new Vector3(data.GetElementAsFloat(0), data.GetElementAsFloat(1)); 
			threshold = data.GetElementAsFloat(2);
			position = vec;
			// invert the x.
			//position.x = -1 * position.x;
			
			if (!usingMMPose)
			{
				position.x = 640 - position.x;
			}
			address = add;
			playerId = GetPlayerNumber(address);
		}
	}
	
	// Class to hold data for all head follower messages
	// These messages differ from "skeleton position" messages beacause they only have a depth (z)
	// (skeleton position messages have a vector position but no depth)
	public class HeadFollowerMessage
	{
		public string address;
		public int playerId;
		public float depth;
		
		public HeadFollowerMessage(string add, OscDataHandle data)
		{
			depth = data.GetElementAsFloat(2);
			address = add;
			playerId = GetPlayerNumber(address);
		}
		
	}
	
	public class BodyPart{
		public GameObject gameObject;
		public Queue<Vector3> positionHistory;
		public float averageX = 0f;
		public float averageY = 0f;
		public double lastOSCTimeStamp = 0.0;
	}
    
	public class PlayerRig{
		public GameObject dummy;
		public GameObject playerTransform; 
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
		public Dictionary<string, BodyPart> addressBodyPartMap;
		public double lastOSCTimeStamp;
		public int playerNumber;
		public float lastPlayerDepth = 999; //an impossible number
		public float playerDepth;		
	}
	
	public ConcurrentQueue<SkeletonPositionMessage> skeletonPositionMessages = new ConcurrentQueue<SkeletonPositionMessage>();
	public ConcurrentQueue<HeadFollowerMessage> headFollowerMessages = new ConcurrentQueue<HeadFollowerMessage>();

	////////////////////////////
	// MY LIST OF PLAYERS!!!! //
	////////////////////////////
	public Dictionary<int, PlayerRig> players = new Dictionary<int, PlayerRig>();
	
	
	//////////////////
	// OSC SERVER! //
	/////////////////
	OscJack.OscServer server;			
	
	////////////////////////////
	// START! LET'S GO OSC!!! //
	////////////////////////////

	void Start() {
        server = OscMaster.GetSharedServer(12000);
        server.MessageDispatcher.AddCallback(string.Empty, OscReceiver1); 
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
	
	void CheckForMissingPlayers(double currentTime){
		// loop through all the players
		foreach (PlayerRig rig in players.Values) {
			if (rig.active)
			{
				//if the UNITY time minus the time of creation is greater than 1 second
				double elapsed = currentTime - rig.lastOSCTimeStamp;
								
				if (elapsed > timeToWaitForMissingPlayers)
				{
					rig.active = false;
					rig.dummy.SetActive(false);
					rig.playerTransform.SetActive(false);
				}			
			}
		}		
	}
	
	PlayerRig MakeNewPlayer(int playerNumber, double oscTime)
	{
		// make a new player
		PlayerRig playerRig = AddPlayer(playerNumber, oscTime);
		// add the player to my list
		players.Add(playerNumber, playerRig);
		// return my rig
		return playerRig;
	}
	
	
	// update the player depth
	// note: when we initialize a player we set their depth to 999 because they have no sensible "last player depth"
	// * we get some weird OSC messages that cause the player to jump around, so we don't update the depth, which can cause problems
	void UpdatePlayerDepth(PlayerRig currPlayer, string address, float depth)
	{
		// head follower depth nonsense
		if (depth != 0.000000)
		{							
			if (currPlayer.lastPlayerDepth == 999){
				currPlayer.lastPlayerDepth = depth; // the first time is tricky
			} else {
				currPlayer.lastPlayerDepth = currPlayer.playerDepth; // go store current depth in "last depth so its there when we look for it"				
			} 
			
			if (Mathf.Abs(currPlayer.lastPlayerDepth - depth) > maxAllowedDepthJumping) // if we are jumping too much
			{
				// do nothing
			} else {
				currPlayer.playerDepth = depth;	
			}
		}	
	}
	
	// This function gets a body party if it already exists
	// if the body part doesn't exist, we create it and update it with position & time information, and add it to the address map.
	BodyPart GetOrMakeAndUpdateBodyPart(PlayerRig currPlayer, string address, Vector3 position, double oscTime)
	{		
		if(currPlayer.addressBodyPartMap.ContainsKey(address))
		{
			return currPlayer.addressBodyPartMap[address];
		} else
		{
			/////////////////////////////////////////////////////
			// ADD NEW BODY PARTS TO THE ADDRESS BODY PART MAP //
			////////////////////////////////////////////////////
			// if it's a NEW body part - if I haven't seen this this body part for this player before
			// grab the relevant body-part-object from the player rig (or a sphere if there is no relevant body part)
			GameObject rigBodyPartObject = OSCBodyPartAssigner(address, currPlayer);
			// set the body part name to the OSC string so I can identify it in the scene
			rigBodyPartObject.name = address;
			// create a body part using the body part class
			// we're passing it the game object and an empty queue (this queue will hold the history of vectors)
			BodyPart bodyPart = new BodyPart();
			bodyPart.gameObject = rigBodyPartObject;
			bodyPart.positionHistory = new Queue<Vector3>();
			bodyPart.lastOSCTimeStamp = oscTime;
			// add the body part to my map
			currPlayer.addressBodyPartMap.Add(address, bodyPart);
			return bodyPart;
		}
		
	}
	
	
	// This function updates the Body Part position history. We then do an averaging over the position history
	// so we can smoothen out their movement.
	void UpdateBodyPartPositionHistoryAndAverage(BodyPart bodyPart, Vector3 position)
	{				
		// low pass filter to average out the values and jumpiness
		bodyPart.positionHistory.Enqueue(position);
		if(bodyPart.positionHistory.Count > 10) {
			Vector3 tmp;
			bodyPart.positionHistory.TryDequeue(out tmp);
		}
		// now calculate the average
		Vector3 sum = new Vector3();
		foreach(var v in bodyPart.positionHistory) {
			sum.x = sum.x + v.x;
			sum.y = sum.y + v.y;
		}
		bodyPart.averageX = sum.x / bodyPart.positionHistory.Count;
		bodyPart.averageY = sum.y / bodyPart.positionHistory.Count;
	}
	

	//////////////////////
	// REACTIVATE PLAYER//
	/////////////////////
	void ReactivatePlayer(PlayerRig playerRig, string address, double oscTime)
	{
                
		// if the player rig is currently NOT active and I get a nose 
		if(!playerRig.active && address.Contains("FACE_38") || !playerRig.active && address.Contains("NOSE")){
			//reactivate player
			playerRig.active = true;
			playerRig.dummy.SetActive(true);
			playerRig.playerTransform.SetActive(true);	
		}
	}
	
	// Update the position of a body part. Lots of complex logic here because
	// we deal with a few things:
	// 1. depth projection
	// 2. if we haven't seen this body part for a while, we set its y = 0
	// 3. LERP for additional smoothing
	void UpdateBodyPartPosition(PlayerRig playerRig, BodyPart bodyPart, string address, double currentTime)
	{       
		if (playerRig.active)
		{
	        	
			////////////////////////////////////////////////////////////////
			// UPDATE BODY PART POSITIONS SO THE PLAYER-DUMMY WILL MOVE //
			//////////////////////////////////////////////////////////////
	        	
			// get the old vector3 for a given body part (by looking up the osc string in the addressBodyPartMap)
			GameObject bodyPartGameObject = bodyPart.gameObject;
			
			// get the "target" position for a given body part using the map function (osc xyz to unity xyz)
			Vector3 target = MapOSCCoordToUnityCoord(bodyPart.averageX, bodyPart.averageY, playerRig.playerDepth);
		        
			// account for depth dimensions in the OSC to unity mapping - it's a 3d world!
			//if(playerRig.playerDepth > 0)
			//{
			//	float depthFactor = depthFactorial * playerRig.playerDepth;
			//	target = new Vector3(target[0] * depthFactor, target[1], target[2]);
			//}	
		    
			/*
			//so the feet don't go above the head - annoying bug
			if(address.Contains("TOE")) 
			{
				//Debug.Log("got a toe");
				//target[1] = 0;
			}
			*/
			
			// if we haven't received a message for this body part
			// in 0.5 seconds then we set it to zero.
			double elapsed = currentTime - bodyPart.lastOSCTimeStamp;
			if(address.Contains("TOE") && elapsed > timeToWaitForMissingFeet)
			{
				//target[1] = 10;
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
	
	// Small helper to clean up some code. This just fetches a player from the
	// map of players, or if that player doesn't exist yet it makes one.
	PlayerRig GetOrMakePlayer(int playerId, double oscTime)
	{
		if(players.ContainsKey(playerId))
		{
			return players[playerId];
		} else
		{
			return MakeNewPlayer(playerId, oscTime);
		}
	}
		

    void Update()
	{
		
		// Get the "current time"! This is used for determining whether
		// we should draw a player or amend the .y position of a body part.
		double currentTime = Time.unscaledTimeAsDouble;
				
		// handle Head follower messages
		HeadFollowerMessage headFollowerMessage;
		while(headFollowerMessages.TryDequeue(out headFollowerMessage))
		{
			// extract useful data from the message
			string address = headFollowerMessage.address;
			int playerId = headFollowerMessage.playerId;
			float depth = headFollowerMessage.depth;
			
			PlayerRig currPlayer = GetOrMakePlayer(playerId, currentTime);
			
			UpdatePlayerDepth(currPlayer, address, depth);
		}
		
		// handle Skeleton position messages
		SkeletonPositionMessage skeletonPositionMessage;
		while(skeletonPositionMessages.TryDequeue(out skeletonPositionMessage))
		{
			// extract useful data from the message
			string address = skeletonPositionMessage.address;
			Vector3 position = skeletonPositionMessage.position;
			int playerId = skeletonPositionMessage.playerId;
			
			PlayerRig currPlayer = GetOrMakePlayer(playerId, currentTime);
			// when we receive an osc message, update the position of the body part (and more?)
			BodyPart bodyPart = GetOrMakeAndUpdateBodyPart(currPlayer, address, position, currentTime);
			
			// update the timestamp on this body part so we know the last time we received a message for it
			bodyPart.lastOSCTimeStamp = currentTime;

			UpdateBodyPartPositionHistoryAndAverage(bodyPart, position);
			ReactivatePlayer(currPlayer, address, currentTime);
			
			// update the timestamp on this player so we know the last time we received a message for this player.
			currPlayer.lastOSCTimeStamp = currentTime;
		}
		
		
		// First, deactivate any missing players
		CheckForMissingPlayers(currentTime);
		
		// Lets go through each known body part an dupdate it's position.
		foreach(PlayerRig player in players.Values)
		{
			foreach(KeyValuePair<string,BodyPart> pair in player.addressBodyPartMap)
			{
				string address = pair.Key;
				BodyPart bodyPart = pair.Value;
				
				// we need to touch every single body part to force it to draw
				UpdateBodyPartPosition(player, bodyPart, address, currentTime);
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
	
	public PlayerRig AddPlayer(int playernum, double oscTime) {
		PlayerRig playerRig = new PlayerRig(); // make a new playerRig using my playerRig class
		playerRig.addressBodyPartMap = new Dictionary<string, BodyPart>(); //make a new addressBodyPartMap - no v3s, just the links
		GameObject dummy = Instantiate(dummyPrefab); //Instantiate a dummy
		dummy.name = $"Dummy {playernum}"; //set the dummy prefab's name name to the player num
		
		string dummyTag = $"Player{playernum}";
		//dummy.tag = dummyTag;
		//AddTagRecursively(dummy.transform, dummyTag);

			
		VRIK vrik = dummy.GetComponent<VRIK>(); //grab the VRIK component
		//myIK = dummy.GetComponent<FullBodyBipedIK>(); //grab the IK component

		GameObject player = Instantiate(playerPrefab); //instantiate a player transform
		player.name = $"Player {playernum}"; //set the player prefab's name to the player num
		
		string playerTag = $"Player{playernum}";
		//AddTagRecursively (player.transform, playerTag);
		
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
		
		vrik.solver.leftLeg.target = playerTransform.leftAnkle.transform;
		vrik.solver.rightLeg.target = playerTransform.rightAnkle.transform;
		
				
		//vrik.solver.leftLeg.target = playerTransform.leftToe.transform;
		//vrik.solver.rightLeg.target = playerTransform.rightToe.transform;
		
		//playerRig.pelvis = getCenterBetweenTwoBodyParts(playerRig.leftHip, playerRig.rightHip);

		//myIK.solver.bodyEffector.target = playerTransform.pelvis.transform;
		//myIK.solver.rightFootEffector.target = playerTransform.rightToe.transform;
		//myIK.solver.leftFootEffector.target = playerTransform.leftToe.transform;
		//myIK.solver.rightHandEffector.target = playerTransform.rightWrist.transform;
		//myIK.solver.leftHandEffector.target = playerTransform.leftWrist.transform;
		
	
		playerRig.active = true; // player rig is set to active because it was just set up
		playerRig.lastOSCTimeStamp = oscTime; //oscTime; // the OSC time is set to the ticks time we passed in when receiving the message
		playerRig.playerNumber = playernum; // add player number
		return playerRig;
		
	} 
    
	/////////////////////////////////////////////////////////////////
	// LINK SPECIFIC OSC MESSAGES TO SPECIFIC PLAYER BODY PARTS  ///
	//   only called if the address has NOT been seen before      ///
	////////////////////////////////////////////////////////////////
	
	// This function determines what object is related to a given string address
	
	public GameObject OSCBodyPartAssigner(string address, PlayerRig rig) {
		GameObject thisObject;
		
		if (usingMMPose)
		{
			if (address.Contains("FACE_38") || address.Contains("NOSE")) {
				thisObject = rig.nose;
			} else if (address.Contains("RIGHT_MIDDLE_FINGER3") || address.Contains("RIGHT_WRIST")) { 
				thisObject = rig.rightWrist;
			}  else if (address.Contains("LEFT_MIDDLE_FINGER3") || address.Contains("LEFT_WRIST")) { 
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
							
			} else if (address.Contains("RIGHT_ANKLE")) { 
				thisObject = rig.rightAnkle;
			}  else if (address.Contains("LEFT_ANKLE")) { 
				thisObject = rig.leftAnkle;
			} else {
				// if I can't find a relevant body part, just make a sphere
				thisObject = Instantiate(debugSphere);
				thisObject.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
				thisObject.GetComponent<Renderer>().material = debugSphereMaterial;

				// I can't remember why I do this parenting step
				thisObject.transform.parent = rig.playerTransform.transform;
			}
			//not using mmpose (using a buggy version of TRT) so flip everything
		} else {
			if (address.Contains("FACE_38") || address.Contains("NOSE")) {
				thisObject = rig.nose;
			} else if (address.Contains("RIGHT_MIDDLE_FINGER3") || address.Contains("RIGHT_WRIST")) { 
				thisObject = rig.leftWrist;
			}  else if (address.Contains("LEFT_MIDDLE_FINGER3") || address.Contains("LEFT_WRIST")) { 
				thisObject = rig.rightWrist;
		
			} else if (address.Contains("RIGHT_KNEE")) { 
				thisObject = rig.leftKnee;
			}  else if (address.Contains("LEFT_KNEE")) { 
				thisObject = rig.rightKnee;
				
			} else if (address.Contains("RIGHT_ELBOW")) { 
				thisObject = rig.leftElbow;
			}  else if (address.Contains("LEFT_ELBOW")) { 
				thisObject = rig.rightElbow;
				
			} else if (address.Contains("LEFT_HIP")) { 
				thisObject = rig.rightHip;

			} else if (address.Contains("RIGHT_HIP")) { 
				thisObject = rig.leftHip;	
			}  else if (address.Contains("NECK")) { 
				thisObject = rig.chest;	
						
			} else if (address.Contains("RIGHT_ANKLE")) { 
				thisObject = rig.leftAnkle;
			}  else if (address.Contains("LEFT_ANKLE")) { 
				thisObject = rig.rightAnkle;
						
			} else {
				// if I can't find a relevant body part, just make a sphere
				thisObject = Instantiate(debugSphere);
				thisObject.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
				thisObject.GetComponent<Renderer>().material = debugSphereMaterial;

				// I can't remember why I do this parenting step
				thisObject.transform.parent = rig.playerTransform.transform;
			}
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
		//if(address.Contains("ANKLE"))
		//{
		//	Debug.Log("got a toe");
		//}
		
		if(address.Contains("livepose/skeletons/0"))
		{
			
			SkeletonPositionMessage msg = new SkeletonPositionMessage(address, data);
			// make sure confidence is big enough before adding msg to the queue
			
			if ((address.Contains("ANKLE") || address.Contains("WRIST")) && msg.threshold > minWristConfidenceAllowed || msg.threshold > minConfidenceAllowed)
			{
				skeletonPositionMessages.Enqueue(msg);

			}
		} else if(address.Contains("livepose/pyrealsense_head_follower/0"))
		{
			headFollowerMessages.Enqueue(new HeadFollowerMessage(address, data));
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




