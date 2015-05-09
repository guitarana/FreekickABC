using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public enum GameMode{
		None,
		Arcade,
		TimeAttack,
		OneBall
	}

	public GameMode gameMode = GameMode.None;

	public enum SubState{
		Init,
		Active,
		Deactive,
		Finish
	}
	
	public SubState substate = SubState.Init;

	//DECLARING VARIABLES TO USE
	public static GameManager instance;
	public CameraManager mainCamera;
	public GameObject ball; //THE CURRENT BALL THAT IS BEING USED
	public GameObject ballTemp; 
	public GoalKeeperAI keeper;
	public Bullseye bullseye;
	public Transform goal;

	public Transform initialPos;
	public TriggerDetector goalDetector;
	public TriggerDetector swingOff;
	public GameObject camPos;

	// Use this for initialization
	void Start () {
		instance = this;
		Physics.IgnoreLayerCollision(8,8); //IGNORE BALLS COLLIDING WITH OTHER BALLS
		Reset ();
		if(UIManager.instance.state == UIManager.State.Arcade) gameMode = GameMode.Arcade;
		if(UIManager.instance.state == UIManager.State.TimeAttack) gameMode = GameMode.TimeAttack;
		if(UIManager.instance.state == UIManager.State.OneBall) gameMode = GameMode.OneBall;
		//createAnotherBall(); //CREATE A BALL TO START WITH
	}

	void UpdateState(){
		GameState.instance.isGoal = goalDetector.ballIn;
		GameState.instance.isEnableSwing = !swingOff.ballIn;
		mainCamera.isDamping = GameState.instance.isCameraDamping;
		if(!GameState.instance.isFlyBall)
			ball.transform.LookAt(goal.transform.position);
	}

	public int shotCounter=0;
	public int goalCounter=0;
	public int blockedCounter=0;
	public int bonus=0;
	public float score;
	public int maxGoal;
	public int ballStock = 15;
	public float timer=0;
	private float timer2=0;
	public float maxTime = 60;

	void DoArcade(){

		if(substate == SubState.Init){
			keeper.gameObject.SetActive(true);
			bullseye.gameObject.SetActive(false);


			maxGoal = PlayerStatistic.instance.xpRemaining;
			substate = SubState.Active;
		}

		if(substate == SubState.Active){


			if(GameState.instance.isFlyBall){
				shotCounter +=1;
				ballStock -=1;
				substate = SubState.Deactive;
			}

		}

		if(substate == SubState.Deactive){

			if(GameState.instance.isGoal){
				ballStock +=1;
				goalCounter +=1;
				score = goalCounter*10;
				timer2 =0;
				substate = SubState.Finish;
			}

			timer2 +=Time.deltaTime;
			if(timer2>1){
				timer2 =0;
				substate = SubState.Finish;
			}

		}

		if(substate == SubState.Finish){
			if(goalCounter==maxGoal){
				PlayerStatistic.instance.xpGain = PlayerStatistic.instance.xpGain+goalCounter;
				goalCounter = 0;
				StartCoroutine(BeginLevelUp());
			}

			if(ballStock == 0){
				InGameUIManager.instance.inGameState = InGameUIManager.InGameState.GameOver;
			}
			
			timer2 +=Time.deltaTime;
			if(timer2>1){
				timer2 =0;
				Reset();
				substate = SubState.Init;
			}

		}

	}



	void DoTimeAttack(){

		if(timer>=maxTime){
			InGameUIManager.instance.inGameState = InGameUIManager.InGameState.GameOver;
			timer =0;
		}

		timer += Time.deltaTime;

		if(substate == SubState.Init){
			keeper.gameObject.SetActive(true);
			bullseye.gameObject.SetActive(true);
			bullseye.state = Bullseye.State.Static;
			bullseye.isWarp = true;
			maxTime = 300;//seconds
			substate = SubState.Active;
		}
		
		if(substate == SubState.Active){

			if(GameState.instance.isFlyBall){
				shotCounter +=1;
				substate = SubState.Deactive;
			}
		}
		
		if(substate == SubState.Deactive){
			if(GameState.instance.isGoal){
				goalCounter +=1;
				score = bullseye.multiplierScore + score;
				timer2 =0;
				substate = SubState.Finish;
			}
			
//			if(!GameState.instance.isFlyBall){
//				substate = SubState.Finish;
//			}

			timer2 +=Time.deltaTime;
			if(timer2>1){
				timer2 =0;
				substate = SubState.Finish;
			}

		}
		
		if(substate == SubState.Finish){

			timer2 +=Time.deltaTime;
			if(timer2>1){
				timer2 =0;
				Reset();
				substate = SubState.Init;
			}

		}
		
	}

	void DoOneBall(){
		
		if(substate == SubState.Init){
			keeper.gameObject.SetActive(true);
			bullseye.gameObject.SetActive(false);


		}
		
		if(substate == SubState.Active){
			
		}
		
		if(substate == SubState.Deactive){
			
		}
		
		if(substate == SubState.Finish){
			
		}
		
	}

	void UpdateGameMode(){
		switch (gameMode){

		case GameMode.Arcade:
			DoArcade();
			break;
		case GameMode.TimeAttack:
			DoTimeAttack();
			break;
		case GameMode.OneBall:
			DoOneBall();
			break;

		}

	}

	// Update is called once per frame
	void Update () {
		UpdateGameMode();
		UpdateState ();
		UpdateStat();
	
	}

	#region ienumerator

	public IEnumerator BeginLevelUp(){
		yield return new WaitForSeconds (1f);
		LevelUp();
	}

	IEnumerator StartEnableControl(){
		yield return new WaitForSeconds (0.5f);
		GameState.instance.isEnableControl = true;
		ball.SetActive (true);
	}

	#endregion

	#region function

	void UpdateStat(){
		InGameUIManager.instance.scoreText.text = score.ToString();
		if(gameMode == GameMode.Arcade)
			InGameUIManager.instance.timeText.text  = ballStock.ToString();
		if(gameMode == GameMode.TimeAttack)
			InGameUIManager.instance.timeText.text  = Mathf.RoundToInt(maxTime-timer).ToString();
		if(gameMode == GameMode.OneBall)
			InGameUIManager.instance.timeText.text  = Mathf.RoundToInt(maxTime-timer).ToString();

		InGameUIManager.instance.goalText.text  = goalCounter.ToString();
		InGameUIManager.instance.startLevelText        .text = PlayerStatistic.instance.chart.ToString();
		InGameUIManager.instance.startTargetText       .text = PlayerStatistic.instance.targetScore.ToString();
		InGameUIManager.instance.startYourScoreText    .text = score.ToString();
		InGameUIManager.instance.pauseLevelText        .text = PlayerStatistic.instance.chart.ToString();
		
		if(GameManager.instance.gameMode == GameMode.Arcade)
			InGameUIManager.instance.pauseHighScoreText    .text = PlayerStatistic.instance.highScoreArcade.ToString();
		if(GameManager.instance.gameMode == GameMode.TimeAttack)
			InGameUIManager.instance.pauseHighScoreText    .text = PlayerStatistic.instance.highScoreTimeAttack.ToString();
		if(GameManager.instance.gameMode == GameMode.OneBall)
			InGameUIManager.instance.pauseHighScoreText    .text = PlayerStatistic.instance.highScoreOneBall.ToString();
		
		InGameUIManager.instance.pauseYourScoreText    .text = score.ToString();
		InGameUIManager.instance.pauseShotsText        .text = shotCounter.ToString();
		InGameUIManager.instance.pauseGoalsText        .text = goalCounter.ToString();
		if(shotCounter!=0)
			InGameUIManager.instance.pauseSuccessText      .text = ((goalCounter/shotCounter)*100).ToString();
		InGameUIManager.instance.pauseGoalKeeperText   .text = blockedCounter.ToString();
		InGameUIManager.instance.winLevelText          .text = PlayerStatistic.instance.globalLevel.ToString();
		InGameUIManager.instance.winTargetText         .text = PlayerStatistic.instance.targetScore.ToString();
		InGameUIManager.instance.winYourScoreText      .text = score.ToString();
		InGameUIManager.instance.loseLevelText         .text = PlayerStatistic.instance.globalLevel.ToString();
		InGameUIManager.instance.loseTargetText        .text = PlayerStatistic.instance.targetScore.ToString();
		InGameUIManager.instance.loseYourScoreText     .text = score.ToString();
		InGameUIManager.instance.gameOverLevelText     .text = PlayerStatistic.instance.chart.ToString();
		
		if(GameManager.instance.gameMode == GameMode.Arcade)
			InGameUIManager.instance.gameOverHighScoreText .text = PlayerStatistic.instance.highScoreArcade.ToString();
		if(GameManager.instance.gameMode == GameMode.TimeAttack)
			InGameUIManager.instance.gameOverHighScoreText .text = PlayerStatistic.instance.highScoreTimeAttack.ToString();
		if(GameManager.instance.gameMode == GameMode.OneBall)
			InGameUIManager.instance.gameOverHighScoreText .text = PlayerStatistic.instance.highScoreOneBall.ToString();
		
		InGameUIManager.instance.gameOverYourScoreText .text = score.ToString();
		InGameUIManager.instance.gameOverShotsText     .text = shotCounter.ToString();
		InGameUIManager.instance.gameOverGoalsText     .text = goalCounter.ToString();
		if(shotCounter!=0)
			InGameUIManager.instance.gameOverSuccessText   .text = ((goalCounter/shotCounter)*100).ToString();
		InGameUIManager.instance.gameOverBonusText     .text = bonus.ToString();
		InGameUIManager.instance.gameOverGoalKeeperText.text = blockedCounter.ToString();
		InGameUIManager.instance.gameOverXPGained      .text = PlayerStatistic.instance.xpGain.ToString();

	}

	void LevelUp(){
		//play animation & play cinematic...
	}

	void Reset(){
		if(ball){
			Destroy (ball);
		}
		ball = Instantiate(ballTemp);
		ball.GetComponent<Rigidbody> ().isKinematic = true; 

		mainCamera.target = ball.transform;
		mainCamera.isDamping = false;
		mainCamera.isStop = false;

		ball.GetComponent<Rigidbody> ().isKinematic = true; 
		ball.transform.position = initialPos.position;
		ball.GetComponent<Rigidbody> ().isKinematic = true; 


		goalDetector.ballIn = false;
		swingOff.ballIn = false;
		GameState.instance.isFlyBall = false;
		GameState.instance.isCameraDamping = false;

		keeper.aiState = GoalKeeperAI.AIState.Idle;
		keeper.substate = GoalKeeperAI.SubState.Init;

		Controller.instance.ball = ball;


		StartCoroutine (StartEnableControl ());

	}

	#endregion

}
