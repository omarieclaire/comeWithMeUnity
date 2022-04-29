using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using UnityEngine;
using OscJack;
using RootMotion.FinalIK;

public class CustomOSC : MonoBehaviour
{
    
	[SerializeField]
	private GameObject dummyPrefab;
	
	[SerializeField]
	private GameObject playerPrefab;
	
	[SerializeField]
	private GameObject delightBall;
	
	[SerializeField]
	private GameObject sadSquare;
    
	[SerializeField]
	//private GameObject nose;
	private List<GameObject> nose = new List<GameObject>();

	[SerializeField]
	//private GameObject rightWrist;
	private List<GameObject> rightWrist = new List<GameObject>();

	[SerializeField]
	//private GameObject leftWrist;
	private List <GameObject> leftWrist = new List<GameObject>();
    
	public class PlayerRig{
		public GameObject dummy;
		public GameObject playerTransform;
		public bool active;
		public GameObject nose;
		public GameObject leftWrist;
		public GameObject rightWrist;
		public Dictionary<string, GameObject> addressBodyPartMap;
		public float lastUpdated;
		public long lastOSCTimeStamp;
		public int playerNumber;
	}
	// hold all the players
	private List<PlayerRig> players = new List<PlayerRig>();
	
	OscJack.OscServer server;
	
	//  holds a map from address to sphere (or body part) because I can't touch the 
	// addressBodyPartMap outside of the update function
	Dictionary<string, GameObject> addressBodyPartMap = new Dictionary<string, GameObject>();
	
	// a special "threadsafe" map - mapping address to coordinates and a timestamp"
	ConcurrentDictionary<string, (Vector3, long)> addressCoordinateAndTimeStampMap = new ConcurrentDictionary<string,(Vector3, long)>();
	
	// a special "threadsafe" map -mapping players to the last timestamp
	//ConcurrentDictionary<int, long> playerLastUpdated = new ConcurrentDictionary<int, long>();
	
	public float speed = 0.15f;

    void Start() {
        server = OscMaster.GetSharedServer(12000);
        server.MessageDispatcher.AddCallback(string.Empty, OscReceiver1); 
    }
	
	int GetPlayerNumber(string address) {
		string[] splitString = address.Split('/');
		Debug.Log(address);
		
		//Debug.Log(splitString[4]);
		return int.Parse(splitString[4]);
	}
	
	
	////////////////////////////////
	// DECTIVATE MISSING PLAYERS //
	//////////////////////////////
	
	void CheckForMissingPlayers(){
		// loop through all the players
		for (int i = 0; i < players.Count; i++) {	
			// for each player IF they are active
			PlayerRig rig = players[i];	
			if (rig.active)
			{
				//if the UNITY time minus the time of creation is greater than 1 second
				float elapsed = Time.time - rig.lastUpdated;
				
				if (elapsed > 0.5f)
				{
					Debug.Log($"deactivating {i} elapsed: {elapsed}");
		
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
		// IF it's a new player
		if (playerNumber + 1 > players.Count)
		{
			// create a new player and pass in the player number and oscTime
			playerRig = AddPlayer(playerNumber, oscTime);
			// add the player to the array of players
			players.Add(playerRig);
		// if it's not a new player 
		} else
		{
			// set the playerRig to the correct player using the number?
			playerRig = players[playerNumber];
		}
		return (playerRig);
	}
		

	// 1. I receive each OSC Message (a string).
	// 2. I extract an OSC String ("address") and x, y, z ("coordinate") from each OSC Message. Additionally, I get the current time.
	// 3. Update function: For each address-coordinate pair, I grab the sphere (or body part) that belongs to that address
	//    update that sphere (or body part) with the coordinate.
	// 4. Independent of this update function, the OSC receiver is updating the address-coordinate pairs
	//    every frame, I get the new coordinate and update the sphere. 
	//    long currentTicks = System.DateTime.Now.Ticks;
		
    void Update()
	{
		// Deactivate any missing players
		CheckForMissingPlayers();
    	    
		// Loop through the "safe" map (osc address string: vector3 coordinates, time) and process each key value pair
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
	        // ADD NEW BODY PARTS TO MY ADDRESS BODY PART MAP //
	        ////////////////////////////////////////////////////

	        // if my playerRig map **hasn't** seen this body part before, that means it's a new body part
	        if(!playerRig.addressBodyPartMap.ContainsKey(address)){
	        	// get and update the relevant body part (transform) and/or circle
	        	GameObject g = OSCBodyPartAssigner(address, playerRig);
	        	// set the body part name to the OSC string so I can identify it
	        	g.name = address;
	        	// add the body part to my map
	        	playerRig.addressBodyPartMap.Add(address, g);
	        } 
	        

	        //////////////////////
	        // REACTIVATE PLAYER//
	        /////////////////////
	                
	        if(!playerRig.active && address.Contains("NOSE") && playerRig.lastOSCTimeStamp != oscTime){
	        	//reactivate player
		        Debug.Log($"reactivating {playerRig.playerNumber}");
		        playerRig.active = true;
		        playerRig.dummy.SetActive(true);
		        playerRig.playerTransform.SetActive(true);	
	        }
	        
	        ////////////////////////////////////////////////////////////////
	        // UPDATE BODY PART POSITIONS SO THE PLAYER-DUMMY WILL MOVE //
	        //////////////////////////////////////////////////////////////
	        
	        if (playerRig.active)
	        {
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
	        	// get a vector3 for a given part by looking up the osc string in the unsafe addressBodyPartMap
		        GameObject bodyPart = playerRig.addressBodyPartMap[address];
	        
		        // get the "target" position for the body part using the map function (osc xyz to unity xyz)
		        Vector3 target = new Vector3(MapToRange(coords[0], 0, 640, -8, 8), MapToRange(coords[1], 0, 480, 10, -3), 0); //Vector3 target = new Vector3(MapToRange(coords[0], 0, 640, -4, 4), MapToRange(coords[1], 0, 480, 8, -4), 0);
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
		PlayerRig playerRig = new PlayerRig();
		playerRig.addressBodyPartMap = new Dictionary<string, GameObject>();
		//Instantiate a dummy, set its name to the player num
		GameObject dummy = Instantiate(dummyPrefab);
		dummy.name = $"Dummy {playernum}";
		VRIK vrik = dummy.GetComponent<VRIK>();
		//Instantiate a player transform 
		GameObject player = Instantiate(playerPrefab);
		player.name = $"Player {playernum}";
		PlayerTransform playerTransform = player.GetComponent<PlayerTransform>();
		//Connect OSC to the player transform 
		playerRig.nose = playerTransform.head;
		playerRig.leftWrist = playerTransform.leftHand;
		playerRig.rightWrist = playerTransform.rightHand;
		// Record dummy and playertransform gameobjects
		playerRig.dummy = dummy;
		playerRig.playerTransform = player;
		// Connect the player transform to the dummy
		vrik.solver.spine.headTarget = playerTransform.head.transform;
		vrik.solver.leftArm.target = playerTransform.leftHand.transform;
		vrik.solver.rightArm.target = playerTransform.rightHand.transform;
		// Is active because it was just set up
		playerRig.active = true;
		playerRig.lastUpdated = Time.time;
		//record time 
		playerRig.lastOSCTimeStamp = oscTime; 
		//Debug.Log(oscTime);
		// add player number
		playerRig.playerNumber = playernum; 

		return playerRig;
	} 
    
	/////////////////////////////////////////////////////////////////
	// LINK SPECIFIC OSC MESSAGES TO SPECIFIC PLAYER BODY PARTS  ///
	//   only called if the address has NOT been seen before      ///
	////////////////////////////////////////////////////////////////
	
	public GameObject OSCBodyPartAssigner(string address, PlayerRig rig) {
		GameObject go;
		if (address.Contains("NOSE")) {
			//Debug.Log($"lastUpdated {rig.lastUpdated}");			
			//Debug.Log($"lastOSCTimeStamp {rig.lastOSCTimeStamp}");
			go = rig.nose;
			rig.lastUpdated = Time.time;
		} else if (address.Contains("RIGHT_WRIST")) { 
	        go = rig.leftWrist;
		}  else if (address.Contains("LEFT_WRIST")) { 
	        go = rig.rightWrist;
		} else {
	        go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			go.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
			go.transform.parent = rig.playerTransform.transform;
        }
		return go;
    }

	//////////////////////////////////////////////////////////////
	// MAP THE OSC VALUES (X,Y,Z) TO THE UNITY VALUES (X,Y,Z) ///
	////////////////////////////////////////////////////////////
	
    public float MapToRange(float numberInRange1, float start1, float end1, float start2, float end2) {
	    //calcuate the "distance" of the number
	    float distance = numberInRange1 - start1;
	    //how much of range1 does this distance cover
        float distanceRatio = distance / (end1 - start1);
	    //take this ratio and apply it to the length of the other range
	    float amountInRange2 = distanceRatio * (end2 - start2);
	    //now I know how far into range2 the number should be so I add that to the beginning of range2
        float finalValue = start2 + amountInRange2;
        return finalValue;
    }
    
    
	//public ObjectGenerator(){
		//https://docs.unity3d.com/Manual/InstantiatingPrefabs.html
		//Instantiate (delightBall, new Vector3(Random.Range(1,10), Random.Range(1,10), Random.Range(1,10)), Quaternion.identity);
	//}
 
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// RECEIVE OSC MESSAGES PORT 12000 & SEND EACH MESSAGE (osc string, v3, and osctime) TO MY THREADSAFE MAP  ///
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OscReceiver1 (string address, OscDataHandle data){
		// receive OSC message and update the values (coordinates) for the keys (addresses)
		// it would be much easier if I could just update the sphere coordinates HERE in the osc receiver
		// but I can't create a gameobject outside of the update function 
        Vector3 vec = new Vector3(data.GetElementAsFloat(0), data.GetElementAsFloat(1));
		long currentTime = System.DateTime.Now.Ticks;
	    
		//Update a special threadsafe map I made that matches the time of the update to a specific player
		//playerLastUpdated.AddOrUpdate(GetPlayerNumber(address), currentTime, (k,v) => v = (currentTime));
		
		//If it's a relevant message, update the threadsafe map of all the OSC Messages (getting a string, and a vector3-time tupple
		if (address.Contains("livepose/skeletons/0"))
		{
			addressCoordinateAndTimeStampMap.AddOrUpdate(address, (vec, currentTime), (k,v) => v = (vec, currentTime));
		}
    }
}





