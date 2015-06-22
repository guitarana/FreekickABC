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

	public GameObject[] pos;

	public float goalDistance;
	public ParticleSystem grassFX;

	public BlockerManager block;
	public int totalBlocker =2;


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
	public float succesRatio=0;
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
		
			if(ballStock == 0){
				InGameUIManager.instance.inGameState = InGameUIManager.InGameState.GameOver;
				InGameUIManager.instance.substate = InGameUIManager.SubState.Init;
			}

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
				PlayerStatistic.instance.xpGain = goalCounter;
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

				goalCounter = 0;
				StartCoroutine(BeginLevelUp());
				substate = SubState.Init;
				GameState.instance.isEnableControl = false;
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
			substate = SubState.Active;
		}
		
		if(substate == SubState.Active){
			if(GameState.instance.isFlyBall){
				shotCounter +=1;
				timer2 =0;
				substate = SubState.Deactive;
			}
		}
		
		if(substate == SubState.Deactive){
			if(GameState.instance.isGoal){
				goalCounter +=1;
				score = goalCounter*10;
				substate = SubState.Finish;
			}

			timer2 +=Time.deltaTime;
			if(timer2>1){
				timer2 =0;
				InGameUIManager.instance.inGameState = InGameUIManager.InGameState.GameOver;
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
		if(InGameUIManager.instance.inGameState == InGameUIManager.InGameState.PauseGame) return;
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
	public void EnableControl(){
		StartCoroutine(StartEnableControl());
	}

	void UpdateStat(){
		InGameUIManager.instance.scoreText.text = score.ToString();
		if(gameMode == GameMode.Arcade)
			InGameUIManager.instance.timeText.text  = ballStock.ToString();
		if(gameMode == GameMode.TimeAttack)
			InGameUIManager.instance.timeText.text  = Mathf.RoundToInt(maxTime-timer).ToString();
		if(gameMode == GameMode.OneBall)
			InGameUIManager.instance.timeText.text  = Mathf.RoundToInt(maxTime-timer).ToString();

		InGameUIManager.instance.distanceText.text = ((int)(goalDistance)-37).ToString();

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
		InGameUIManager.instance.pauseSuccessText      .text = succesRatio.ToString("F1") + " %";
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
		InGameUIManager.instance.gameOverSuccessText   .text = succesRatio.ToString("F1") +" %";
		InGameUIManager.instance.gameOverBonusText     .text = bonus.ToString();
		InGameUIManager.instance.gameOverGoalKeeperText.text = blockedCounter.ToString();
		InGameUIManager.instance.gameOverXPGained      .text = PlayerStatistic.instance.xpGain.ToString();

		if(shotCounter!=0)
			succesRatio =((float)goalCounter/(float)shotCounter)*100;

	}


	void SetPos(){
		initialPos = pos[Random.Range(0,pos.Length-1)].transform;
		PlayerAvatar.instance.gameObject.transform.position = initialPos.GetChild(0).transform.position;
		PlayerAvatar.instance.gameObject.transform.rotation = initialPos.GetChild(0).transform.rotation;
	}

	void LevelUp(){
		InGameUIManager.instance.inGameState = InGameUIManager.InGameState.WinGame;
		InGameUIManager.instance.substate = InGameUIManager.SubState.Init;
		PlayerAvatar.instance.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0,90,0));
		mainCamera.target = PlayerAvatar.instance.gameObject.transform;
		GameState.instance.isCelebrating = true;
		int r = Random.Range(0,2);
		switch(r){
		case 0:
			PlayerAvatar.instance.aiState = PlayerAvatar.AIState.Celeb1;
			break;
		case 1:
			PlayerAvatar.instance.aiState = PlayerAvatar.AIState.Celeb2;
			break;
		case 2:
			PlayerAvatar.instance.aiState = PlayerAvatar.AIState.Celeb3;
			break;
		}
		PlayerAvatar.instance.substate = PlayerAvatar.SubState.Init;
		GameManager.instance.substate = SubState.Init;
		goalCounter = 0;
	}

	public void Reset(){
		if(ball){
			Destroy (ball);
		}
		ball = Instantiate(ballTemp);
		ball.GetComponent<Rigidbody> ().isKinematic = true; 
		GameState.instance.isCelebrating = false;
		Controller.instance.ball = ball;
		mainCamera.target = ball.transform;
		mainCamera.isDamping = false;
		mainCamera.isStop = false;
		SetPos();
		ball.GetComponent<Rigidbody> ().isKinematic = true; 
		ball.transform.position = initialPos.position;
		ball.GetComponent<Rigidbody> ().isKinematic = true; 
		grassFX.transform.position = initialPos.position;

		PlayerAvatar.instance.aiState = PlayerAvatar.AIState.Idle;
		PlayerAvatar.instance.substate = PlayerAvatar.SubState.Init;

		goalDetector.ballIn = false;
		swingOff.ballIn = false;
		GameState.instance.isFlyBall = false;
		GameState.instance.isCameraDamping = false;


		keeper.aiState = GoalKeeperAI.AIState.Idle;
		keeper.substate = GoalKeeperAI.SubState.Init;
		keeper.transform.position = new Vector3(keeper.initPos.x + Random.Range(0,1),keeper.initPos.y,keeper.initPos.z +Random.Range(-2,2));

		block.totalBlocker = totalBlocker;
		block.gameObject.transform.position = PlayerAvatar.instance.gameObject.transform.position;
		block.gameObject.transform.rotation = PlayerAvatar.instance.gameObject.transform.rotation;
		block.gameObject.transform.Translate(Vector3.forward * 25);

		//block.gameObject.transform.rotation = PlayerAvatar.instance.gameObject.transform.rotation;
		if(PlayerStatistic.instance.globalLevel < 2)
			block.totalBlocker = 0;
		else if(PlayerStatistic.instance.globalLevel >= 2 && PlayerStatistic.instance.globalLevel < 5 )
			block.totalBlocker = Random.Range(0,2);
		else if(PlayerStatistic.instance.globalLevel >= 5 && PlayerStatistic.instance.globalLevel < 10 )
			block.totalBlocker = Random.Range(0,3);
		else if(PlayerStatistic.instance.globalLevel >= 10 && PlayerStatistic.instance.globalLevel < 20 )
			block.totalBlocker = Random.Range(0,4);
		else if(PlayerStatistic.instance.globalLevel >= 20)
			block.totalBlocker = Random.Range(0,5);

		block.Init();
		goalDistance = Vector3.Distance(ball.transform.position,goal.transform.position);

		StartCoroutine (StartEnableControl ());

	}

	#endregion

}
