using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	
	}

	void OnTriggerEnter(Collider other) 
	{
		if(other.gameObject.tag == "Ball"){
			GameManager.instance.ball.GetComponent<Ball>().ResetVelocity();
		}
	}

	// Update is called once per frame
	void Update ()
	{
	
	}
}

