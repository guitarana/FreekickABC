using UnityEngine;
using System.Collections;

public class Bullseye : MonoBehaviour
{
	public enum State{
		None,
		Static,
		Dynamic
	}

	public State state = State.None;
	public float goalHeight;
	public float goalLenght;

	public float speed=1;
	public bool isWarp;
	private Vector3 initPos;
	public bool isMoving;
	private Vector3 desiredPosition;

	public float multiplierScore;
	public Transform center;


	// Use this for initialization
	void Start ()
	{
		initPos = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(state == State.Dynamic){
			Move ();
		}

		if(state == State.Static){
			if(isWarp){
				isWarp = false;
				Warp();
			}
		}


	}

	void Move(){
		if(!isMoving){
			desiredPosition = new Vector3(transform.position.x,Random.Range(initPos.y,goalHeight),Random.Range(initPos.z,goalLenght));
			isMoving = true;
		}else{

			if(Vector3.Distance(transform.position,desiredPosition)>1){
				transform.position = Vector3.Lerp(transform.position,desiredPosition,Time.deltaTime*speed);
			}else{
				isMoving = false;
			}
		}
	}

	void Warp(){
		gameObject.transform.position = new Vector3(transform.position.x,Random.Range(initPos.y,goalHeight),Random.Range(initPos.z,goalLenght));
	}


	float ScoreChecker(){
	
		if(Vector3.Distance(center.position,GameManager.instance.ball.transform.position)<=1.28f){
			return 100;
		}

		if(Vector3.Distance(center.position,GameManager.instance.ball.transform.position)>1.28f && Vector3.Distance(center.position,GameManager.instance.ball.transform.position)<=3f){
			return 50;
		}

		if(Vector3.Distance(center.position,GameManager.instance.ball.transform.position)>3f && Vector3.Distance(center.position,GameManager.instance.ball.transform.position)<=4.7f){
			return 25;
		}

		return 0;

	}

	void OnTriggerEnter(Collider other) 
	{
		if(other.gameObject.tag == "Ball"){
			multiplierScore = ScoreChecker();
			Debug.Log("MultiplierScore " + multiplierScore);
		}
	}

//	int GetScore(){
//
//	}
}

