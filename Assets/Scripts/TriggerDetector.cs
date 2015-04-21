using UnityEngine;
using System.Collections;

public class TriggerDetector : MonoBehaviour {

	public bool ballIn;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider other) 
	{
		if(other.gameObject.tag == "Ball"){
			ballIn = true;
		}
	}
	
	void OnTriggerStay(Collider other){
		if(other.gameObject.tag == "Ball"){
			ballIn = true;
		}
	}
	
//	void OnTriggerExit(Collider other){
//		if(other.gameObject.tag == "Balls"){
//			ballIn = false;
//		}
//	}
}
