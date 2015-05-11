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
			if(GameManager.instance.gameMode == GameManager.GameMode.Arcade)
				isCameraDamping = true;
			if(GameManager.instance.gameMode == GameManager.GameMode.TimeAttack){
				isCameraDamping = false;
				isCameraStatic = true;
			}
		}

		if (!isEnableSwing) {
			isEnableControl = false;
		}
	}
}

