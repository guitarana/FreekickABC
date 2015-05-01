using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
	
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
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

