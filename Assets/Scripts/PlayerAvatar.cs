using UnityEngine;
using System.Collections;

public class PlayerAvatar : MonoBehaviour
{

	public static PlayerAvatar instance;

	public enum AIState{
		None,
		Idle,
		Shoot,
		Celeb1,
		Celeb2,
		Celeb3
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
		instance = this;
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
			if(GameState.instance.isAiming){
				substate = SubState.Deactive;
			}
		}
		
		if (substate == SubState.Deactive) {
			substate = SubState.Finish;
			
		}
		
		if (substate == SubState.Finish) {
			substate = SubState.Init;
			aiState = AIState.Shoot;
		}
	}
	float timer;
	void DoShoot(){
		
		if (substate == SubState.Init) {
			substate = SubState.Active;
		}
		
		if (substate == SubState.Active) {
			timer += Time.deltaTime;
			if(timer >= anim.GetClip("Shoot").length -1.4f){
				GameManager.instance.grassFX.Emit(10);
				timer = 0;
				GameState.instance.isShooting = true;
				substate = SubState.Deactive;
			}

		}
		
		if (substate == SubState.Deactive) {
			timer += Time.deltaTime;
			if(timer >= 0.5f){
				timer = 0;
				substate = SubState.Finish;
			}
		}
		
		if (substate == SubState.Finish) {

			aiState = AIState.Idle;
			substate = SubState.Init;
		}
	}
	
	void DoCeleb1(){
		
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

	void DoCeleb2(){
		
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

	void DoCeleb3(){
		
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

	
	void UpdateState(){
		switch (aiState) {
		case AIState.None :
			DoNone();
			break;
		case AIState.Idle :
			DoIdle();
			break;
		case AIState.Shoot :
			DoShoot();
			break;
		case AIState.Celeb1:
			DoCeleb1();
			break;
		case AIState.Celeb2:
			DoCeleb2();
			break;
		case AIState.Celeb3:
			DoCeleb3();
			break;
		}
	}
	
	
	void UpdateAnimation(){
		switch (aiState) {
		case AIState.Idle :
			anim.CrossFade("Idle",0.25f);
			anim.clip = anim["Idle"].clip;
			break;
		case AIState.Shoot :
			anim.CrossFade("Shoot",0.25f);
			anim.clip = anim["Shoot"].clip;
			break;
		case AIState.Celeb1 :
			anim.CrossFade("Seleb_berdiri",0.25f);
			anim.clip = anim["Seleb_berdiri"].clip;
			break;
		case AIState.Celeb2 :
			anim.CrossFade("Seleb_tepuk tangan",0.25f);
			anim.clip = anim["Seleb_tepuk tangan"].clip;
			break;
		case AIState.Celeb3 :
			anim.CrossFade("Seleb_duduk",0.25f);
			anim.clip = anim["Seleb_duduk"].clip;
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

