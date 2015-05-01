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
		//createAnotherBall(); //CREATE A BALL TO START WITH
	}

	void UpdateState(){
		GameState.instance.isGoal = goalDetector.ballIn;
		GameState.instance.isEnableSwing = !swingOff.ballIn;
		mainCamera.isDamping = GameState.instance.isCameraDamping;
		if(!GameState.instance.isFlyBall)
			ball.transform.LookAt(goal.transform.position);
	}


	public int goalCounter=0;
	public float score;
	public int maxGoal;
	public int ballStock = 15;
	public float timer=0;
	public float maxTime;

	void DoArcade(){

		if(substate == SubState.Init){
			keeper.gameObject.SetActive(true);
			bullseye.gameObject.SetActive(false);

			maxGoal = 15;
			substate = SubState.Active;
		}

		if(substate == SubState.Active){


			if(GameState.instance.isFlyBall){
				ballStock -=1;
				substate = SubState.Deactive;
			}

		}

		if(substate == SubState.Deactive){

			if(GameState.instance.isGoal){
				ballStock +=1;
				goalCounter +=1;
				substate = SubState.Finish;
			}

			if(!GameState.instance.isFlyBall){
				substate = SubState.Finish;
			}


		}

		if(substate == SubState.Finish){
			if(goalCounter==maxGoal){
				StartCoroutine(BeginLevelUp());
				goalCounter = 0;
			}

			if(ballStock == 0){
				//gameover
			}
			
			if(GameState.instance.isEnableControl){
				substate = SubState.Init;
			}

		}

	}



	void DoTimeAttack(){

		if(timer>=maxTime){
			//gameover
			timer =0;
		}

		timer += Time.deltaTime;

		if(substate == SubState.Init){
			keeper.gameObject.SetActive(false);
			bullseye.gameObject.SetActive(true);
			bullseye.state = Bullseye.State.Static;
			bullseye.isWarp = true;
			maxTime = 300;//seconds
			substate = SubState.Active;
		}
		
		if(substate == SubState.Active){

			if(GameState.instance.isFlyBall){
				substate = SubState.Deactive;
			}
		}
		
		if(substate == SubState.Deactive){
			if(GameState.instance.isGoal){
				goalCounter +=1;
				score = bullseye.multiplierScore + score;
				substate = SubState.Finish;
			}
			
			if(!GameState.instance.isFlyBall){
				substate = SubState.Finish;
			}

		}
		
		if(substate == SubState.Finish){
			if(GameState.instance.isEnableControl){
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

	void LevelUp(){
		//play animation & play cinematic...
	}

	void Reset(){
		if(ball)
			Destroy (ball);
		ball = Instantiate(ballTemp);
		ball.GetComponent<Rigidbody> ().isKinematic = true; 

		mainCamera.target = ball.transform;
		mainCamera.isDamping = false;

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
