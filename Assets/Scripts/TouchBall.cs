using UnityEngine;

public class TouchBall : MonoBehaviour
{
    
	void OnTriggerEnter(Collider other){
		//Debug.Log(colllisionInfo.collider.name) 
		Debug.Log(other.GetComponent<Collider>().name);

		if (other.gameObject.CompareTag("sphere"))
		{
			//other.gameObject.SetActive (false);
			Destroy(other.gameObject);
		}

		//other.gameObject.transform.localScale = new Vector3(2f, 2f, 2f); 
		//Destroy(other.gameObject);
	}
}
