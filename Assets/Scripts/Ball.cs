using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
	bool blocked = false;
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnCollisionEnter(Collision collision) {
		foreach (ContactPoint contact in collision.contacts) {
			Debug.DrawRay(contact.point, contact.normal, Color.white);
		}
//		if (collision.relativeVelocity.magnitude > 2)
//			audio.Play();

		if(collision.collider.gameObject.tag == "Keeper"){
			if(!blocked){
				blocked = true;
				GameManager.instance.blockedCounter +=1;
			}

		}
	}

	public void ResetVelocity(){
		gameObject.GetComponent<Rigidbody>().isKinematic = true;
		Debug.Log("reset");
		gameObject.tag ="OffBall";
		gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
		gameObject.GetComponent<Rigidbody>().isKinematic = false;
	}
}

