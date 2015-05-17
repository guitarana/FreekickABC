using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour
{
	public static GameState instance;

	public bool isAiming;
	public bool isShooting;
	public bool isFlyBall;
	public bool isEnableControl;
	public bool isGoal;
	public bool isEnableSwing;
	public bool isCameraDamping;
	public bool isCameraStatic;
	public bool isCelebrating;
	// Use this for initialization
	void Start ()
	{
		instance = this;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (isGoal) {
			isEnableControl = false;
			isEnableSwing = false;
			//isCameraDamping = false;
		}

		if (isFlyBall && !isGoal) {

				isCameraDamping = true;
				//isCameraStatic = true;

		}

		if (!isEnableSwing) {
			isEnableControl = false;
		}
	}
}

