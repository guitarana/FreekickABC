using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour
{
	//DECLARING VARIABLES TO USE
	public static Controller instance;

	Vector3 mouseStart = Vector3.zero; //THE MOUSE POSITION WHEN IT IS FIRST PRESSED (MOUSEDOWN)
	Vector3 mouseEnd = Vector3.zero; //THE MOUSE POSITION WHEN IT IS RELEASED (MOUSEUP)
	Vector3 mouseStartFlick = Vector3.zero; //THE MOUSE POSITION WHEN IT IS FIRST PRESSED (MOUSEDOWN)
	Vector3 mouseEndFlick = Vector3.zero; //THE MOUSE POSITION WHEN IT IS RELEASED (MOUSEUP)

	public float forceValue;
	public float sensitivity;
	public GameObject ball;


	// Use this for initialization
	void Start () {
		instance = this;
	}
	
	
	// Update is called once per frame
	void Update () {
		if(!ball) ball = GameManager.instance.ball;
		if(InGameUIManager.instance.inGameState == InGameUIManager.InGameState.PauseGame || InGameUIManager.instance.inGameState == InGameUIManager.InGameState.GameOver) return;
		if(GameState.instance.isGoal || !GameState.instance.isEnableSwing){
			ball.GetComponent<ConstantForce> ().enabled= false;
		}


		if (GameState.instance.isEnableControl) {
			if (!GameState.instance.isFlyBall && !GameState.instance.isAiming) {

				if(!GameState.instance.isDisableFlick){
					Vector3 currentPos = Input.mousePosition; //THE CURRENT POSITION OF THE MOUSE
					currentPos.z = 5f; //ADDING DEPTH TO THE POSITION
					
					if (Input.GetMouseButtonDown (0)) { //IF MOUSE IS PRESSED FOR THE FIRST TIME
						
						mouseStart = Input.mousePosition; //SAVE THE MOUSEPOSITION IN MOUSESTART VECTOR
					}
					
					
					//SWIPE CODE
					if (Input.GetMouseButtonUp (0)) { //IF MOUSE IS RELEASED (SWIPE COMPLETE)
						mouseEnd = Input.mousePosition; //SAVE MOUSE END POSITION
						if (Vector3.Distance (mouseStart, mouseEnd) > 20) { //CHECK IF THE MOUSE WAS SWIPED A DISTANCE - NOT A CLICK
							GameState.instance.isAiming = true;
		
						}

						
					}
				
					
					if (Input.GetMouseButton (0)) { //WHILE MOUSE IS DOWN (MOVE THE TRACE LINE)

					}
				}

			}else if(GameState.instance.isShooting){
				ball.GetComponent<Rigidbody> ().isKinematic = false; 
				ball.GetComponent<Rigidbody> ().WakeUp ();
				forceValue =50;
				ball.transform.rotation = Quaternion.Euler (new Vector3 (-GetLength ()*0.4f, -(GetAngle ()), 0)) * Quaternion.Euler(0, CameraManager.instance.angle, 0); //ROTATE THE BALL TO FACE THE DIRECTION OF THE SWIPE - LENGTH = Y, ANGLE = X
				
				ball.GetComponent<Rigidbody> ().AddRelativeForce (Vector3.forward * forceValue, ForceMode.Impulse); //NOW THE BALL IS FACING THE DIRECTION OF SWIPE, ADD FORWARD FORCE SIMPLY
				ball.GetComponent<ConstantForce>().force = new Vector3(-forceValue/4F ,forceValue*1f,0);
				ball.GetComponent<ConstantForce> ().enabled = true;
				GameState.instance.isFlyBall = true;
				GameState.instance.isShooting = false;
				GameState.instance.isAiming = false;
			}
			else{
				ball.transform.Rotate(new Vector3(0,20,0));
				if (GameState.instance.isGoal) {
					ball.GetComponent<ConstantForce> ().enabled= false;
				}
				
				if (GameState.instance.isEnableSwing) {
					Vector3 currentPos = Input.mousePosition; //THE CURRENT POSITION OF THE MOUSE
					currentPos.z = 5f; //ADDING DEPTH TO THE POSITION
					
					if (Input.GetMouseButtonDown (0)) { //IF MOUSE IS PRESSED FOR THE FIRST TIME
						mouseStartFlick = Input.mousePosition; //SAVE THE MOUSEPOSITION IN MOUSESTART VECTOR
					}
					
					if (Input.GetMouseButton (0)) { //WHILE MOUSE IS DOWN (MOVE THE TRACE LINE)
						mouseEndFlick = Input.mousePosition;
						
						switch (GetDirection2 ()) {
//						case "UP ":
//
//							ball.GetComponent<ConstantForce> ().force = new Vector3 (0, GetLength2 ()*sensitivity , 0);
//							break;
//						case "DOWN ":
//
//							ball.GetComponent<ConstantForce> ().force = new Vector3 (0, -GetLength2 ()*sensitivity, 0);
//							break;
						case "RIGHT ":

							ball.GetComponent<ConstantForce> ().force = Quaternion.Euler(0, CameraManager.instance.angle/2, 0) * new Vector3 (0, 50, GetLength2 ()*sensitivity );
							break;
						case "LEFT ":
						
							ball.GetComponent<ConstantForce> ().force = Quaternion.Euler(0, CameraManager.instance.angle/2, 0) * new Vector3 (0,50, -GetLength2 ()*sensitivity );
							break;
							
							}
						}
					} else {
						ball.GetComponent<ConstantForce> ().enabled= false;
				}
			}
		}
	}
	
	float GetAngle () {
		//GET ANGLE BETWEEN TWO VECTORS OF THE SWIPE GESTURE
		Vector3 v2 ;
		if(mouseEnd.y<mouseStart.y)
			v2= mouseStart - mouseEnd;
		else
			v2 = mouseEnd - mouseStart;
		float angle = Mathf.Atan2(v2.y, v2.x)*Mathf.Rad2Deg;
		return angle;
	}


	float GetLength () {
		//GET LENGTH OF THE MOUSESTART AND MOUSEEND POSITIONS
		Vector3 v2 = (mouseEnd - mouseStart);

		return v2.magnitude/10f;
	}

	float GetAngle2 () {
		//GET ANGLE BETWEEN TWO VECTORS OF THE SWIPE GESTURE
		Vector3 v2 ;
		v2 = mouseEndFlick - mouseStartFlick;
		float angle = Mathf.Atan2(v2.y, v2.x)*Mathf.Rad2Deg;
		return angle;
	}

	float GetLength2 () {
		//GET LENGTH OF THE MOUSESTART AND MOUSEEND POSITIONS
		Vector3 v2 = (mouseEndFlick - mouseStartFlick);
		
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
	}

	string GetDirection2 () {
		
		//GET DIRECTION OF SWIPE
		bool isUp = false;
		bool isDown = false;
		bool isRight = false;
		bool isLeft= false;
		
		float angle = GetAngle2();
		
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
	}
		
}

