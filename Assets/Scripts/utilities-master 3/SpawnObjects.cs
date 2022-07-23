using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnObjects : MonoBehaviour
{
	public List<GameObject> possibleObjects{
		get{
			return PossibleObjects;
		} set{
			PossibleObjects=value;
		}
	}
	[SerializeField]
	private List<GameObject> PossibleObjects;
	[MinMaxSlider(0,10000)]
	public Vector2 quantityRange;
	public float spawnRange;
	public Transform parent;
	public Transform spawnAround;
	public bool RandomizeRotation;
	public GameObjectEvent OutputSpawned;
	public UnityEvent eventOnComplete;
	
	public void Spawn(){
		int quantity = Mathf.RoundToInt(Random.Range(quantityRange.x,quantityRange.y));
		for(int i=0;i<quantity;i++){
			int randomObject = Random.Range(0,possibleObjects.Count-1);
			Vector3 randomRotation = new Vector3(0,Random.Range(0,4) * 90,0);
			Vector3 position = spawnAround.position + (Random.onUnitSphere * spawnRange);
			GameObject newObject;
			if(RandomizeRotation){
				newObject = Instantiate(possibleObjects[randomObject],position,Quaternion.Euler(randomRotation),parent);	
			} else {
				newObject = Instantiate(possibleObjects[randomObject],position,Quaternion.identity,parent);
			}
			OutputSpawned.Invoke(newObject);
			
		}
		eventOnComplete.Invoke();
	}
	
}
