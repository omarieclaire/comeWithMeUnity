using UnityEngine;
using System.Linq;

public class TouchBall : MonoBehaviour
{
    
	void OnTriggerEnter(Collider other){
		//Debug.Log(colllisionInfo.collider.name) 
		Debug.Log("whyyyyy");

		if (other.gameObject.CompareTag("sphere"))
		{
			other.gameObject.SetActive (false);
			Destroy(other.gameObject);
			Debug.Log("destroyyyyyy");
		}

		//other.gameObject.transform.localScale = new Vector3(2f, 2f, 2f); 
		//Destroy(other.gameObject);
	}
	
	private void Update() {
		var hands = GameObject.FindGameObjectsWithTag("hand");
		var otherHands = hands.Where(h => h != this.gameObject).ToList();
		foreach (GameObject hand in otherHands) {
			float distance = Vector3.Distance(transform.position, hand.transform.position);
			Animator partentAnimator = hand.GetComponentInParent<Animator>();
			Debug.Log($"Hand {hand.name} of {partentAnimator.gameObject.name} is {distance}m away");
		}
	}
}
