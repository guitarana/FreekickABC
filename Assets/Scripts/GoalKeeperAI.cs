using UnityEngine;
using System.Collections;

public class GoalKeeperAI : MonoBehaviour
{
	public GameObject ball;
	public GameObject leftHand;
	public GameObject rightHand;
	public float minReflectDistance= 5f;
	public float agility= 5f;
	public bool isJump=false;
	public Vector3 initPos;

	public enum AIState{
		None,
		Idle,
		JumpLeft,
		JumpRight,
		Jockey,
		Catch
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
		initPos = transform.position;
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
			isJump = false;
			substate = SubState.Active;
		}

		if (substate == SubState.Active) {
			if(!ball)
				ball = GameManager.instance.ball;
			transform.rotation = Quaternion.RotateTowards(transform.rotation, ball.transform.rotation* Quaternion.Euler(new Vector3(0,180,0)), Time.deltaTime * 100f);

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
	
			//transform.rotation = Quaternion.RotateTowards(transform.rotation, ball.transform.rotation* Quaternion.Euler(new Vector3(0,180,0)), Time.deltaTime * 100f);

			if(Mathf.Abs(ball.transform.position.z-transform.position.z)<=2f){
				if(Vector3.Distance(ball.transform.position,leftHand.transform.position)<12){
					aiState = AIState.Catch;
					substate = SubState.Init;
				}
				if(Vector3.Distance(ball.transform.position,rightHand.transform.position)<12){
					aiState = AIState.Catch;
					substate = SubState.Init;
				}
			
			}else{
				if(Vector3.Distance(ball.transform.position,leftHand.transform.position)<Vector3.Distance(ball.transform.position,rightHand.transform.position)){
					transform.Translate(Vector3.left*Time.deltaTime*agility);
					if(Vector3.Distance(ball.transform.position,leftHand.transform.position)<30){
						aiState = AIState.JumpLeft;
						substate = SubState.Init;
					}
				}
				else{
					transform.Translate(Vector3.right*Time.deltaTime*agility);
					if(Vector3.Distance(ball.transform.position,rightHand.transform.position)<30){
						aiState = AIState.JumpRight;
						substate = SubState.Init;
					}
				}
			}
		}
		
		if (substate == SubState.Deactive) {
			
		}
		
		if (substate == SubState.Finish) {
			
		}
	}

	void DoCatch(){
		
		if (substate == SubState.Init) {

			substate = SubState.Active;
		}
		
		if (substate == SubState.Active) { 
			
		}
		
		if (substate == SubState.Deactive) {
			
		}
		
		if (substate == SubState.Finish) {
			
		}
	}

	void DoJumpRight(){
		
		if (substate == SubState.Init) {
			//transform.localScale = new Vector3(-3.748038f,3.748038f,3.748038f);
			transform.localScale = new Vector3(-1.124411f,1.124411f,1.124411f);
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
			transform.localScale = new Vector3(1.124411f,1.124411f,1.124411f);
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
		case AIState.Catch:
			DoCatch();
			break;
		}
	}

	
	void UpdateAnimation(){
		switch (aiState) {
		case AIState.Idle :
			transform.localScale = new Vector3(1.124411f,1.124411f,1.124411f);

			anim.CrossFade("Idle",0);
			break;
		case AIState.Jockey :
			transform.localScale = new Vector3(1.124411f,1.124411f,1.124411f);

			anim.CrossFade("Idle");
			break;
		case AIState.Catch :
			transform.localScale = new Vector3(1.124411f,1.124411f,1.124411f);

			if(substate == SubState.Init){
				if(ball.transform.position.y>1 && ball.transform.position.y <2)
					anim.CrossFade("Catch",0.25f);
				else
					anim.CrossFade("BlockNear",0.25f);
			}
			break;
		case AIState.JumpLeft:
			if(substate == SubState.Init){
				if(Vector3.Distance(ball.transform.position,rightHand.transform.position)>20)
					anim.CrossFade("BlockFar");
				if(Vector3.Distance(ball.transform.position,rightHand.transform.position)>10 && Vector3.Distance(ball.transform.position,rightHand.transform.position)<20)
					anim.CrossFade("BlockMedium");
				if(Vector3.Distance(ball.transform.position,rightHand.transform.position)>2 && Vector3.Distance(ball.transform.position,rightHand.transform.position)<=10)
					anim.CrossFade("BlockNear");


			}
			break;
		case AIState.JumpRight:
			if(substate == SubState.Init){

				if(Vector3.Distance(ball.transform.position,rightHand.transform.position)>20)
					anim.CrossFade("BlockFar");
				if(Vector3.Distance(ball.transform.position,rightHand.transform.position)>minReflectDistance*0.5f && Vector3.Distance(ball.transform.position,rightHand.transform.position)<minReflectDistance)
					anim.CrossFade("BlockMedium");
				if(Vector3.Distance(ball.transform.position,rightHand.transform.position)>minReflectDistance*0.1f && Vector3.Distance(ball.transform.position,rightHand.transform.position)<=minReflectDistance*0.5f)
					anim.CrossFade("BlockNear");

				isJump = true;
			}
			break;

		
		}
	}
	

	// Update is called once per frame
	void Update ()
	{

		if(InGameUIManager.instance.inGameState == InGameUIManager.InGameState.PauseGame) return;

		UpdateState ();
		UpdateAnimation ();
	}
}

