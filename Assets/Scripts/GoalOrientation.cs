using UnityEngine;
using System.Collections;

public class GoalOrientation : MonoBehaviour {
	public static GoalOrientation instance;
	public float angle;
	public GameObject target;
	// Use this for initialization
	void Start () {
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		if(InGameUIManager.instance.inGameState == InGameUIManager.InGameState.PauseGame) return;


		if(target)
			transform.LookAt(target.transform);
		else
			target = GameManager.instance.ball;
		angle = transform.localEulerAngles.y-92.5f;
	}
}
