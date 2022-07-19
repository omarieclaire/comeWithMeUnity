using UnityEngine;

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
}
