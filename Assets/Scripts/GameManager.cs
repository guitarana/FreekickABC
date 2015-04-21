using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	//DECLARING VARIABLES TO USE
	public static GameManager instance;
	public SmoothFollow mainCamera;
	public GameObject ball; //THE CURRENT BALL THAT IS BEING USED
	public GameObject ballTemp; 
	public GoalKeeperAI keeper;
	//public GameObject lineInitial; //THE LINE GAMEOBJECT THAT IS DUPLICATED AFTER EVERY SWIPE
	public GameObject lineTmp; //THE CURRENT LINE OBJECT TO USE
	public GameObject cameraTarget;
	public GUIStyle style; //STYLE FOR LABELS


	Vector3 mouseStart = Vector3.zero; //THE MOUSE POSITION WHEN IT IS FIRST PRESSED (MOUSEDOWN)
	Vector3 mouseEnd = Vector3.zero; //THE MOUSE POSITION WHEN IT IS RELEASED (MOUSEUP)

	public float forceValue;

	public Transform initialPos;

	public bool flyBall;

	public TriggerDetector goalDetector;
	public TriggerDetector swingOff;

	public bool enableControl;

	public GameObject camPos;

	// Use this for initialization
	void Start () {
		instance = this;
		Physics.IgnoreLayerCollision(8,8); //IGNORE BALLS COLLIDING WITH OTHER BALLS
		Reset ();
		//createAnotherBall(); //CREATE A BALL TO START WITH
	}


	// Update is called once per frame
	void Update () {

	
		if (enableControl) {
			if (!flyBall) {

				Vector3 currentPos = Input.mousePosition; //THE CURRENT POSITION OF THE MOUSE
				currentPos.z = 5f; //ADDING DEPTH TO THE POSITION

				if (Input.GetMouseButtonDown (0)) { //IF MOUSE IS PRESSED FOR THE FIRST TIME

					mouseStart = Input.mousePosition; //SAVE THE MOUSEPOSITION IN MOUSESTART VECTOR
					//lineTmp = GameObject.Instantiate(lineInitial,currentPos,Quaternion.identity) as GameObject; //CREATE A NEW LINE (TRACE)
				}


				//SWIPE CODE
				if (Input.GetMouseButtonUp (0)) { //IF MOUSE IS RELEASED (SWIPE COMPLETE)
					mouseEnd = Input.mousePosition; //SAVE MOUSE END POSITION
					if (Vector3.Distance (mouseStart, mouseEnd) > 20) { //CHECK IF THE MOUSE WAS SWIPED A DISTANCE - NOT A CLICK
						mainCamera.isDamping = true;
						ball.GetComponent<Rigidbody> ().isKinematic = false; 
						ball.GetComponent<Rigidbody> ().WakeUp ();
//						forceValue = GetLength () * 2;
//						if (forceValue < 40)
//							forceValue = 40; 
						forceValue = 90;
						Debug.Log ("LineLength : " + forceValue);
						Destroy (lineTmp); //DESTROY THE PREVIOUS LINE GAME OBJECT (TRACE LINE)
						ball.transform.rotation = Quaternion.Euler (new Vector3 (-GetLength ()*0.4f, -(GetAngle ()), 0)); //ROTATE THE BALL TO FACE THE DIRECTION OF THE SWIPE - LENGTH = Y, ANGLE = X
						//ball.transform.rotation = Quaternion.Euler(target);
						ball.GetComponent<Rigidbody> ().AddRelativeForce (Vector3.forward * forceValue, ForceMode.Impulse); //NOW THE BALL IS FACING THE DIRECTION OF SWIPE, ADD FORWARD FORCE SIMPLY
						ball.GetComponent<ConstantForce>().force = new Vector3(-forceValue/2F,forceValue*0.3f,0);
						ball.GetComponent<ConstantForce> ().enabled = true;//ENABLE WIND
						flyBall = true;
						//createAnotherBall(); //CREATE ANOTHER BALL AS THIS ONE HAS BEEN DISPATCHED
//					updateGUI (); //UPDATE GUI WITH SWIPE VALUES
					}

				}

				if (Input.GetMouseButton (0)) { //WHILE MOUSE IS DOWN (MOVE THE TRACE LINE)
					if (lineTmp != null) {
						//lineTmp.transform.position = Camera.main.ScreenToWorldPoint (currentPos); //MOVE THE TRACE LINE TO CURRENTPOS
					}
				}
			} else {
				//mainCamera.target = cameraTarget.transform;
				ball.transform.Rotate(new Vector3(0,10,0));
				if (goalDetector.ballIn) {
					ball.GetComponent<ConstantForce> ().force = new Vector3 (0, 0, 0);
					enableControl = false;
					Debug.Log ("goal!!!");
				}

				if (!swingOff.ballIn) {
		

					Vector3 currentPos = Input.mousePosition; //THE CURRENT POSITION OF THE MOUSE
					currentPos.z = 5f; //ADDING DEPTH TO THE POSITION
				
					if (Input.GetMouseButtonDown (0)) { //IF MOUSE IS PRESSED FOR THE FIRST TIME
						mouseStart = Input.mousePosition; //SAVE THE MOUSEPOSITION IN MOUSESTART VECTOR
						//lineTmp = GameObject.Instantiate(lineInitial,currentPos,Quaternion.identity) as GameObject; //CREATE A NEW LINE (TRACE)
					}

					if (Input.GetMouseButton (0)) { //WHILE MOUSE IS DOWN (MOVE THE TRACE LINE)
						mouseEnd = Input.mousePosition;

						switch (GetDirection ()) {
						case "UP ":
							Debug.Log ("UP");
							ball.GetComponent<ConstantForce> ().force = new Vector3 (0, GetLength ()*3f, 0);
							break;
						case "DOWN ":
							Debug.Log ("DOWN");
							ball.GetComponent<ConstantForce> ().force = new Vector3 (0, -GetLength ()*3f, 0);
							break;
						case "RIGHT ":
							Debug.Log ("RIGHT");
							ball.GetComponent<ConstantForce> ().force = new Vector3 (0, 0, GetLength ()*3f);
							break;
						case "LEFT ":
							Debug.Log ("LEFT");
							ball.GetComponent<ConstantForce> ().force = new Vector3 (0, 0, -GetLength ()*3f);
							break;

						}
					}
				} else {
					enableControl = false;
					ball.GetComponent<ConstantForce> ().force = new Vector3 (0, 0, 0);
				}
			}
		}
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
		mainCamera.transform.position = ball.transform.position + new Vector3 (10,7,0);
		ball.GetComponent<Rigidbody> ().isKinematic = true; 

		goalDetector.ballIn = false;
		swingOff.ballIn = false;
		flyBall = false;
		keeper.aiState = GoalKeeperAI.AIState.Idle;
		keeper.substate = GoalKeeperAI.SubState.Init;


		StartCoroutine (StartEnableControl ());

	}

	IEnumerator StartEnableControl(){
		yield return new WaitForSeconds (0.5f);
		enableControl = true;
		ball.SetActive (true);
	}


	float GetAngle () {
		//GET ANGLE BETWEEN TWO VECTORS OF THE SWIPE GESTURE
		Vector3 v2 = mouseEnd - mouseStart;
		float angle = Mathf.Atan2(v2.y, v2.x)*Mathf.Rad2Deg;
		return angle;
	}
	
	float GetLength () {

		//GET LENGTH OF THE MOUSESTART AND MOUSEEND POSITIONS
		Vector3 v2 = mouseEnd - mouseStart;
		return v2.magnitude/10f;
	}

	string GetDirection () {

		//GET DIRECTION OF SWIPE
		bool isUp = false;
		bool isDown = false;
		bool isRight = false;
		bool isLeft= false;

		float angle = GetAngle();

		if(angle>=45&&angle<=135)
			isUp=true;

		if(angle>=0&&angle<=80 || angle>=-80&&angle<=0)
			isRight=true;

		if(angle>=100&&angle<=180 || angle>=-180&&angle<=-100)
			isLeft=true;


		if(angle>=-180&&angle<=-0)
			isDown=true;


		string direction = "";

		if(isUp)
			direction+="UP ";
		if(isDown)
			direction+="DOWN ";
		if(isRight)
			direction+="RIGHT ";
		if(isLeft)
			direction+="LEFT ";


		
		return direction;

		//if(angle<45) direction
	}




//	float angleGUI;
//	string directionGUI;
//	float lengthGUI;

//	void updateGUI() //UPDATE GUI WITH THE LAST SWIPE VALUES
//	{
//		angleGUI = GetAngle();
//		directionGUI = GetDirection();
//		lengthGUI = GetLength();
//	}

	void OnGUI()
	{

		if(true)
		{
		//DRAW ON GUI

//		GUI.Label(new Rect(10,120f,400,80),"Swipe Angle: " + angleGUI);
//		GUI.Label(new Rect(10f,140f,400,80),"Swipe Direction: " + directionGUI);
//		GUI.Label(new Rect(10f,160f,400,80),"Swipe Length: " + lengthGUI);

		//GUI.HorizontalSlider(new Rect(0,10,150,30),0.1f,1,0.1f,1,style,style,true,1);

//		GUI.Label(new Rect(Screen.width-110,10,100,30),"Speed/Force");
//		hSliderValue = GUI.HorizontalSlider(new Rect(Screen.width-110,40,100,30),hSliderValue,25,50);
//
//		GUI.Label(new Rect(Screen.width-110,80,100,30),"Wind");
//		hWindValue = GUI.HorizontalSlider(new Rect(Screen.width-110,110,100,30),hWindValue,-25,25);
		}
	}

}
