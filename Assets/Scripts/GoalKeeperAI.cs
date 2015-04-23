using UnityEngine;
using System.Collections;

public class GoalKeeperAI : MonoBehaviour
{
	public GameObject ball;
	public GameObject leftHand;
	public GameObject rightHand;
	public float minReflectDistance= 5f;
	public float agility= 5f;

	public enum AIState{
		None,
		Idle,
		JumpLeft,
		JumpRight,
		Jockey
	}

	public AIState aiState = AIState.None;

	public enum SubState{
		Init,
		Active,
		Deactive,
		Finish
	}

	public SubState substate = SubState.Init;

	public Animation anim;
	// Use this for initialization
	void Start ()
	{
		anim = gameObject.GetComponent<Animation> ();
		aiState = AIState.Idle;

	}

	void DoNone(){

		if(substate == SubState.Init){
			gameObject.SetActive(false);
		}

		if(substate == SubState.Active){
			
		}

		if(substate == SubState.Deactive){
			
		}

		if(substate == SubState.Finish){
			
		}

	}

	void DoIdle(){

		if (substate == SubState.Init) {
			gameObject.SetActive(true);
			substate = SubState.Active;
		}

		if (substate == SubState.Active) {
			if(!ball)
				ball = GameManager.instance.ball;
			if(GameState.instance.isFlyBall){
				substate = SubState.Deactive;
			}
		}

		if (substate == SubState.Deactive) {
			substate = SubState.Finish;

		}

		if (substate == SubState.Finish) {
			substate = SubState.Init;
			aiState = AIState.Jockey;
		}
	}

	void DoJockey(){
		
		if (substate == SubState.Init) {
			substate = SubState.Active;
		}
		
		if (substate == SubState.Active) {
			if(Vector3.Distance(ball.transform.position,leftHand.transform.position)<Vector3.Distance(ball.transform.position,rightHand.transform.position)){
				transform.Translate(Vector3.left*Time.deltaTime*agility);
				if(Vector3.Distance(ball.transform.position,leftHand.transform.position)<minReflectDistance){
					aiState = AIState.JumpLeft;
					substate = SubState.Init;
				}
			}
			else{
				transform.Translate(Vector3.right*Time.deltaTime*agility);
				if(Vector3.Distance(ball.transform.position,rightHand.transform.position)<minReflectDistance){
					aiState = AIState.JumpRight;
					substate = SubState.Init;
				}
			}
		}
		
		if (substate == SubState.Deactive) {
			
		}
		
		if (substate == SubState.Finish) {
			
		}
	}

	void DoJumpRight(){
		
		if (substate == SubState.Init) {
			transform.Translate(Vector3.right*1);
			substate = SubState.Active;
		}
		
		if (substate == SubState.Active) {
			
		}
		
		if (substate == SubState.Deactive) {
			
		}
		
		if (substate == SubState.Finish) {
			
		}
	}

	void DoJumpLeft(){
		
		if (substate == SubState.Init) {
			transform.Translate(Vector3.left*1);
			substate = SubState.Active;
		}
		
		if (substate == SubState.Active) {
			
		}
		
		if (substate == SubState.Deactive) {
			
		}
		
		if (substate == SubState.Finish) {
			
		}
	}

	void UpdateState(){
		switch (aiState) {
		case AIState.None :
			DoNone();
			break;
		case AIState.Idle :
			DoIdle();
			break;
		case AIState.Jockey :
			DoJockey();
			break;
		case AIState.JumpLeft:
			DoJumpLeft();
			break;
		case AIState.JumpRight:
			DoJumpRight();
			break;
		}
	}

	
	void UpdateAnimation(){
		switch (aiState) {
		case AIState.Idle :
			anim.CrossFade("Idle");
			break;
		case AIState.Jockey :
			anim.CrossFade("Idle");
			break;
		case AIState.JumpLeft:
			anim.CrossFade("JumpLeft");
			break;
		case AIState.JumpRight:
			anim.CrossFade("JumpRight");
			break;
		}
	}
	

	// Update is called once per frame
	void Update ()
	{
		UpdateState ();
		UpdateAnimation ();
	}
}

