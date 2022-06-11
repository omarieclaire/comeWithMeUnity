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
	
	
	// OPEN QUESTIONS
	// - There's a lot of "connecting" in the AddPlayer function that I don't understand
	// - I feel like there must be a better way to deal with time and activating/deactivating players
	// - I'm curious to move some of the stuff out of the update function and into their own functions, but I have to do it from the metalab
	// - 
	// - 
	// - 
	// - 
	// - 
	
		

public class CustomOSC : MonoBehaviour
{
    
	public float speed = 0.15f;

	[SerializeField]
	private GameObject dummyPrefab;
	
	[SerializeField]
	private GameObject playerPrefab;
	
	[SerializeField]
	private GameObject delightBall;
	
	[SerializeField]
	private GameObject sadSquare;
    
	//[SerializeField]
	//private List<GameObject> nose = new List<GameObject>();
	
	///////////////////////////
	// PLAYER RIG CLASS!!!! //
	//////////////////////////
    
	public class PlayerRig{
		public GameObject dummy;
		public GameObject playerTransform; //transform is the main way to scale, rotate and position a gameobject
		public bool active;
		public GameObject nose;
		public GameObject leftWrist;
		public GameObject rightWrist;
		public GameObject leftElbow;
		public GameObject rightElbow;
		public GameObject leftAnkle;
		public GameObject rightAnkle;
		public GameObject leftKnee;
		public GameObject rightKnee;
		public Dictionary<string, GameObject> addressBodyPartMap;
		public float lastUpdated;
		public long lastOSCTimeStamp;
		public int playerNumber;
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
	public List<PlayerRig> players = new List<PlayerRig>();
	
	
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
		return int.Parse(splitString[4]);
	}
	
	////////////////////////////////
	// DECTIVATE MISSING PLAYERS //
	//////////////////////////////
	
	void CheckForMissingPlayers(){
		// loop through all the players
		for (int i = 0; i < players.Count; i++) { // for each player IF they are active
			PlayerRig rig = players[i];	
			if (rig.active)
			{
				//if the UNITY time minus the time of creation is greater than 1 second
				float elapsed = Time.time - rig.lastUpdated;
				
				if (elapsed > 0.5f)
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
		// make a new playerRig?
		PlayerRig playerRig;
		// IF it's a new player (index starts at 0)
		if (playerNumber + 1 > players.Count)
		{
			// create a new player and pass in the player number and oscTime
			playerRig = AddPlayer(playerNumber, oscTime);
			// add the player to the array of players
			// note: this may add the wrong player in the wrong position of the list
			// in future I might fill the array with holes (nulls) until my index
			players.Add(playerRig);
		// if it's not a new player 
		} else
		{
			// set the playerRig to the correct player using the number?
			playerRig = players[playerNumber];
		}
		return (playerRig);
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
	        	// add the body part to my map
	        	playerRig.addressBodyPartMap.Add(address,rigBodyPartObject);
	        } 
	        

	        //////////////////////
	        // REACTIVATE PLAYER//
	        /////////////////////
	                
	        // if the player rig is currently NOT active and I got the nose and ??the time is different than now??
	        if(!playerRig.active && address.Contains("NOSE") && playerRig.lastOSCTimeStamp != oscTime){
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
	        	if(address.Contains("NOSE")){
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
		        GameObject bodyPart = playerRig.addressBodyPartMap[address];
	        
		        // get the "target" position for a given body part using the map function (osc xyz to unity xyz)
		        Vector3 target = new Vector3(MapToRange(coords[0], 0, 640, -4, 4), MapToRange(coords[1], 0, 480, 6, 0), MapToRange(playerRig.playerDepth, 0, 4, 10, -10)); 
		       
		        // use lerp to smooth out the movement - NOTE is this making the movements inaccurate?
		        bodyPart.transform.position = Vector3.Lerp(bodyPart.transform.position, target, speed);  
		        //bodyPart.transform.position = target;
	        }
     
        }

    }
    
    
	//////////////////////////////////////////////////////
	// MAKE A NEW PLAYER - RIG, DUMMY, ISACTIVE, MORE ///
	/////////////////////////////////////////////////////
	
	public PlayerRig AddPlayer(int playernum, long oscTime) {
		PlayerRig playerRig = new PlayerRig(); // make a new playerRig using my playerRig class
		playerRig.addressBodyPartMap = new Dictionary<string, GameObject>(); //make a new addressBodyPartMap
		GameObject dummy = Instantiate(dummyPrefab); //Instantiate a dummy
		dummy.name = $"Dummy {playernum}"; //set the dummy prefab's name name to the player num
		VRIK vrik = dummy.GetComponent<VRIK>(); //grab the VRIK component
		GameObject player = Instantiate(playerPrefab); //instantiate a player transform
		player.name = $"Player {playernum}"; //set the player prefab's name to the player num
		PlayerTransform playerTransform = player.GetComponent<PlayerTransform>(); //get the player's transform (position, scale, depth)
		
		// Connect the playerRig's game objects to the actual body part of the player that is in the scene. why?? 
		// example: The nose of the playerRig is whereever that players head is in the scene?
		// I think this only happens once on setup?
		playerRig.nose = playerTransform.head;
		playerRig.leftWrist = playerTransform.leftWrist;
		playerRig.rightWrist = playerTransform.rightWrist;
		playerRig.leftAnkle = playerTransform.leftAnkle;
		playerRig.rightAnkle = playerTransform.rightAnkle;		
		playerRig.rightElbow = playerTransform.rightElbow;
		playerRig.leftElbow = playerTransform.leftElbow;
		playerRig.rightKnee = playerTransform.rightKnee;
		playerRig.leftKnee = playerTransform.leftKnee;
		//playerRig.pelvis = playerTransform.leftKnee;

		// connect the "dummy" in a playerRig to the actual dummy in the scene???
		playerRig.dummy = dummy;
		// connect the "playerTransfor" in a playerRig to the actual player in the scene???
		playerRig.playerTransform = player;

		// Connect the VRIK solver to the playerTransforms?????
		vrik.solver.spine.headTarget = playerTransform.head.transform;
		vrik.solver.leftArm.target = playerTransform.leftWrist.transform;
		vrik.solver.rightArm.target = playerTransform.rightWrist.transform;
		
		//vrik.solver.pelvis.target = playerTransform.pelvis.transform;
		//vrik.solver.chest.target = playerTransform.chest.transform;
		//vrik.solver.leftToe.target = playerTransform.leftToe.transform;
		//vrik.solver.rightToe.target = playerTransform.rightToe.transform;
		
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
	
	public GameObject OSCBodyPartAssigner(string address, PlayerRig rig) {
		
		GameObject thisObject;
		
		if (address.Contains("NOSE")) {
			thisObject = rig.nose;
			// update the time the player was last updated - should I be doing this for all body parts?!!!			
			// Debug.Log($"lastUpdated {rig.lastUpdated}");			
			// Debug.Log($"lastOSCTimeStamp {rig.lastOSCTimeStamp}");
			rig.lastUpdated = Time.time;
			
			
		} else if (address.Contains("RIGHT_WRIST")) { 
			thisObject = rig.rightWrist;
		}  else if (address.Contains("LEFT_WRIST")) { 
			thisObject = rig.leftWrist;
		
		} else if (address.Contains("RIGHT_KNEE")) { 
			thisObject = rig.rightKnee;
		}  else if (address.Contains("LEFT_KNEE")) { 
			thisObject = rig.leftKnee;
				
		} else if (address.Contains("RIGHT_ELBOW")) { 
			thisObject = rig.rightElbow;
		}  else if (address.Contains("LEFT_ELBOW")) { 
			thisObject = rig.leftElbow;
			
		} else if (address.Contains("RIGHT_ANKLE")) { 
			thisObject = rig.rightAnkle;
		}  else if (address.Contains("LEFT_ANKLE")) { 
			thisObject = rig.leftAnkle;
				
			//MID_HIP
			
		} else {
			// if I can't find a relevant body part, just make a sphere
	        thisObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			thisObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
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
		
		
		if (address.Contains("livepose/pyrealsense_head_follower/0") && data.GetElementAsFloat(2) != 0.000000)
		{
			int playerId = GetPlayerNumber(address); // extract the player number from the OSC string
			float z = data.GetElementAsFloat(2); // extract the depth from the OSC data - which looks like this   livepose/position/0/1 fff -0.212375 0.803258 2.666000


		
			if (players.Count > 0) // make sure I don't try to set a depth value before I actually have any players
			{
						
				PlayerRig currPlayer = players[playerId]; //grab the current player

				// update playerDepth of that player with the Z -- does this even work?
				currPlayer.playerDepth = z;
			}
			
		}
			
		long currentTime = System.DateTime.Now.Ticks;	
		Vector3 vec = new Vector3(data.GetElementAsFloat(0), data.GetElementAsFloat(1));
	

		//If it's a skeleton-position message, which looks like this livepose/skeletons/0/0/keypoints/RIGHT_RING_FINGER2 fff 370.167511 253.175644 0.810268
		if (address.Contains("livepose/skeletons/0"))
		{
			//Update a special threadsafe map of all the OSC Messages (a string, and a vector3-time tupple 
			addressCoordinateAndTimeStampMap.AddOrUpdate(address, (vec, currentTime), (k,v) => v = (vec, currentTime));
		}
    }
}



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




