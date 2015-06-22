using UnityEngine;
using System.Collections;

public class OpponentBlockerAI : MonoBehaviour
{
	public GameObject ball;
	public float minReflectDistance= 5f;
	public float agility= 5f;
	public bool isJump=false;
	public Vector3 initPos;
	public float timer;


	public enum AIState{
		None,
		Idle,
		Jump1,
		Jump2,
		Jump3,
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
		anim["Jump1"].speed = 2;
		anim["Jump2"].speed = 2;
		anim["Jump3"].speed = 2;
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
			transform.rotation = GameManager.instance.ball.transform.rotation;
			transform.Rotate(0,180,0);

			gameObject.SetActive(true);
			isJump = false;
			substate = SubState.Active;
		}

		if (substate == SubState.Active) {
			if(!ball)
				ball = GameManager.instance.ball;

			if(GameState.instance.isFlyBall){
				substate = SubState.Deactive;
				agility = Random.Range(0.5f,1.5f);
			}
			anim.CrossFade("Idle",0);
		}

		if (substate == SubState.Deactive) {
		//	timer +=Time.deltaTime;
		//	if(timer > agility){
				substate = SubState.Finish;
		//		timer = 0;
		//	}

		}

		if (substate == SubState.Finish) {
			substate = SubState.Init;
			int i = Random.Range(0,2);
			switch(i){
			case 0:
				aiState = AIState.Jump1;
				break;
			case 1:
				aiState = AIState.Jump2;
				break;
			case 2:
				aiState = AIState.Jump3;
				break;
			}

		}
	}

	void DoJump3(){
		
		
		if (substate == SubState.Init) {
			isJump = true;
			substate = SubState.Active;
		}
		
		if (substate == SubState.Active) {
			
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

	void DoJump2(){
		
		
		if (substate == SubState.Init) {
			isJump = true;
			substate = SubState.Active;
		}
		
		if (substate == SubState.Active) {
			
		}
		
		if (substate == SubState.Deactive) {
			
		}
		
		if (substate == SubState.Finish) {
			
		}
	}

	void DoJump1(){
		
		if (substate == SubState.Init) {
			isJump = true;
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
		case AIState.Jump3 :
			DoJump3();
			break;
		case AIState.Jump1:
			DoJump1();
			break;
		case AIState.Jump2:
			DoJump2();
			break;
		case AIState.Catch:
			DoCatch();
			break;
		}
	}

	
	void UpdateAnimation(){
		switch (aiState) {
		case AIState.Idle :
			anim.CrossFade("Idle",0);
			break;
		case AIState.Jump3 :
			if(substate == SubState.Init){
				anim.CrossFade("Jump3",0.25f);
				
			}
			break;
		case AIState.Catch :
			if(substate == SubState.Init){
				if(ball.transform.position.y>1 && ball.transform.position.y <2)
					anim.CrossFade("Catch",0.25f);
				else
					anim.CrossFade("BlockNear",0.25f);
			}
			break;
		case AIState.Jump1:
			if(substate == SubState.Init){
				anim.CrossFade("Jump1",0.25f);

			}
			break;
		case AIState.Jump2:
			if(substate == SubState.Init){
				anim.CrossFade("Jump2",0.25f);
				
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

