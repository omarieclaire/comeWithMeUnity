using UnityEngine;
using System.Linq;

public class TouchBall : MonoBehaviour
{
    
	void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag("sphere"))
		{
			other.gameObject.SetActive (false);
			Destroy(other.gameObject);
		}
	}

}
