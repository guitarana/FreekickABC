using UnityEngine;
using System.Collections;

public class ControlManager : MonoBehaviour
{
	public GameObject ball;

	public GameObject lineInitial; //THE LINE GAMEOBJECT THAT IS DUPLICATED AFTER EVERY SWIPE
	public GameObject lineTmp; //THE CURRENT LINE OBJECT TO USE
	Vector3 mouseStart = Vector3.zero; //THE MOUSE POSITION WHEN IT IS FIRST PRESSED (MOUSEDOWN)
	Vector3 mouseEnd = Vector3.zero; //THE MOUSE POSITION WHEN IT IS RELEASED (MOUSEUP)
	public float maxTime= 2f;
	public float timer = 0f;
	public bool enableControl = true;
	public bool countStart = false;
	public bool haveBall;
	public bool flyBall = false;

	public Vector3 throwSpeed;
	public float distance;

	public float speed = 1;
	public float time;
	Vector3 target;
	TouchState touchState;

	public bool drawGizmo;

	private Vector3 initialPos;

	public GameObject posTempDebug;
	// Use this for initialization
	void Start ()
	{
		initialPos = ball.transform.position;
		touchState = GetComponent<TouchState> ();
	}

	void RayClickPosition(Vector3 pos){
		Ray ray = Camera.main.ScreenPointToRay(pos);
		RaycastHit _rayHit;
		if(Physics.Raycast (ray, out _rayHit)){
			target = _rayHit.point + new Vector3 (0.0f,0.0f,0.0f);
		}
		
	}

	void RayTouchPosition(Vector2 pos){
		Ray ray = Camera.main.ScreenPointToRay(pos);
		RaycastHit _rayHit;
		if(Physics.Raycast (ray, out _rayHit)){
			target = _rayHit.point + new Vector3 (0.0f,0.0f,0.0f);
		}

	}

	bool RayClickBallInit(Vector3 pos){
		Ray ray = Camera.main.ScreenPointToRay(pos);
		RaycastHit _rayHit;
		if(Physics.Raycast (ray, out _rayHit)){
			if(_rayHit.collider.tag =="Ball")
				return true;
		}

		return false;
	}

	bool RayTouchBallInit(Vector2 pos){
		Ray ray = Camera.main.ScreenPointToRay(pos);
		RaycastHit _rayHit;
		if(Physics.Raycast (ray, out _rayHit)){
			if(_rayHit.collider.tag =="Ball")
				return true;
		}
		
		return false;
	}


	// Update is called once per frame
	void Update ()
	{
		if (enableControl) {
			Vector3 currentPos = Input.mousePosition;
			if(touchState.touch.phase == TouchPhase.Moved){
				Ray ray = Camera.main.ScreenPointToRay(touchState.touch.position);
				RaycastHit _rayhit;
				if(Physics.Raycast(ray, out _rayhit))
					currentPos = _rayhit.point + new Vector3(0.0f,0.0f,0.0f); //THE CURRENT POSITION OF THE MOUSE
			}else
				currentPos = Input.mousePosition; //THE CURRENT POSITION OF THE MOUSE

			currentPos.z = 5f; //ADDING DEPTH TO THE POSITION

			if (
				(Input.GetMouseButtonDown (0) && RayClickBallInit (Input.mousePosition)) 
			    || ((touchState.touch.phase == TouchPhase.Began) && RayTouchBallInit(touchState.touch.position))
				){

				//mouseStart = ball.transform.position;
				mouseStart = Input.mousePosition;
				lineTmp = GameObject.Instantiate (lineInitial, currentPos, Quaternion.identity) as GameObject; //CREATE A NEW LINE (TRACE)
				posTempDebug.transform.position = mouseStart;
				haveBall = true;
			}

	

			if(Application.platform == RuntimePlatform.Android){
				if (
					 ((touchState.touch.phase == TouchPhase.Began) && !RayTouchBallInit(touchState.touch.position))
					){
					
					haveBall = false;
				}

			}else{
				if (
					(Input.GetMouseButtonDown (0) && !RayClickBallInit (Input.mousePosition)) 
					){
					
					haveBall = false;
				}
			}

			if (haveBall) {
		
				if (Input.GetMouseButton (0) 
				    || touchState.touch.phase == TouchPhase.Moved
				    ) { //WHILE MOUSE IS DOWN (MOVE THE TRACE LINE)
					if (lineTmp != null) {
						mouseEnd = Camera.main.ScreenToWorldPoint (currentPos);
						if(drawGizmo)
							lineTmp.transform.position = mouseEnd; //MOVE THE TRACE LINE TO CURRENTPOS

						if (Vector3.Distance (mouseStart, mouseEnd) > 4.7f) {
							countStart = true;
						}
					
					}
				}



				if (countStart) {
					if (Input.GetMouseButtonUp (0)
					//    || touchState.touch.phase == TouchPhase.Ended
					    ) { //IF MOUSE IS RELEASED (SWIPE COMPLETE)
						//SAVE MOUSE END POSITION
						flyBall = true;
						RayTouchPosition (touchState.touch.position);
						RayClickPosition (Input.mousePosition);
					}

					timer += Time.deltaTime;
					if (timer >= maxTime) {
						flyBall = true;
						RayTouchPosition (touchState.touch.position);
						RayClickPosition (Input.mousePosition);

					}
				}


			

			}
		}

		if(flyBall){

			enableControl = false;

			countStart = false;
			Destroy (lineTmp); //DESTROY THE PREVIOUS LINE GAME OBJECT (TRACE LINE)
			updateGUI (); //UPDATE GUI WITH SWIPE VALUES
			posTempDebug.transform.position = target; 
			FlyBall2();
		}





	}

	void Reset(){
		ball.GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, 0);
		ball.transform.position = initialPos;
		maxTime= 2f;
		timer = 0f;
		enableControl = true;
		countStart = false;
		
		flyBall = false;
	}

	void FlyBall2(){
		if (timer >= 0.5)
			speed = 20;
		else
			speed = 10/timer;
		timer = 0;
		flyBall = false;
		ball.transform.rotation=Quaternion.Euler(new Vector3(-GetLength(),-(GetAngle()-90),0)); //ROTATE THE BALL TO FACE THE DIRECTION OF THE SWIPE - LENGTH = Y, ANGLE = X
		ball.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward*speed,ForceMode.Impulse); //NOW THE BALL IS FACING THE DIRECTION OF SWIPE, ADD FORWARD FORCE SIMPLY
		ball.GetComponent<ConstantForce>().enabled = true;//ENABLE WIND
	}

	void FlyBall(){
		distance = Vector3.Distance (ball.transform.position, target);
		if (timer >= 0.5)
			speed = 20;
		else
			speed = 10/timer;
		time = distance / speed;
		throwSpeed = calculateBestThrowSpeed(ball.transform.position,target,time);
		ball.GetComponent<Rigidbody>().AddForce(throwSpeed, ForceMode.VelocityChange);
		timer = 0;
		flyBall = false;
	}

	private Vector3 calculateBestThrowSpeed(Vector3 origin, Vector3 target, float timeToTarget) {
		// calculate vectors
		Vector3 toTarget = target - origin;
		Vector3 toTargetXZ = toTarget;
		toTargetXZ.y = 0;
		
		// calculate xz and y
		float y = toTarget.y;
		float xz = toTargetXZ.magnitude;
		
		// calculate starting speeds for xz and y. Physics forumulase deltaX = v0 * t + 1/2 * a * t * t
		// where a is "-gravity" but only on the y plane, and a is 0 in xz plane.
		// so xz = v0xz * t => v0xz = xz / t
		// and y = v0y * t - 1/2 * gravity * t * t => v0y * t = y + 1/2 * gravity * t * t => v0y = y / t + 1/2 * gravity * t
		float t = timeToTarget;
		float v0y = y / t + 0.5f * Physics.gravity.magnitude * t;
		float v0xz = xz / t;
		
		// create result vector for calculated starting speeds
		Vector3 result = toTargetXZ.normalized;        // get direction of xz but with magnitude 1
		result *= v0xz;                                // set magnitude of xz to v0xz (starting speed in xz plane)
		result.y = v0y;                                // set y to v0y (starting speed of y plane)
		
		return result;
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

	float angleGUI;
	string directionGUI;
	float lengthGUI;

	void updateGUI() //UPDATE GUI WITH THE LAST SWIPE VALUES
	{
		angleGUI = GetAngle();
		//directionGUI = GetDirection();
		lengthGUI = GetLength();
	}

	void OnGUI()
	{

		if(true)
		{
			//DRAW ON GUI

//			GUI.Label(new Rect(10f,10f,300f,100f),"This is a simple swipe scene. You can change the action of different axis in the code easily" +
//			          "All the code is commented and easy to modify - the swipe functionality works as follows: angle = x axis, length = elevation/height of the ball" +
//			          ",the power is constant. Try hitting the boxes!"
//			          );
//			GUI.Label(new Rect(10,120f,400,80),"Swipe Angle: " + angleGUI);
//			GUI.Label(new Rect(10f,140f,400,80),"Swipe Direction: " + directionGUI);
//			GUI.Label(new Rect(10f,160f,400,80),"Swipe Length: " + lengthGUI);
//
//			//GUI.HorizontalSlider(new Rect(0,10,150,30),0.1f,1,0.1f,1,style,style,true,1);
//
//			GUI.Label(new Rect(Screen.width-110,10,100,30),"Speed/Force");
//			hSliderValue = GUI.HorizontalSlider(new Rect(Screen.width-110,40,100,30),hSliderValue,25,50);
//
//			GUI.Label(new Rect(Screen.width-110,80,100,30),"Wind");
//			hWindValue = GUI.HorizontalSlider(new Rect(Screen.width-110,110,100,30),hWindValue,-25,25);
		}
	}

}

