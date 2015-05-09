using UnityEngine;
using System.Collections;

public class UINavigation : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void StartGame(){
		Application.LoadLevel("MainGame");
	}

	public void GoArcade(){
		UIManager.instance.state = UIManager.State.Arcade;
		Application.LoadLevel("locker_room");
	}
	
	public void GoTimeAttack(){
		UIManager.instance.state = UIManager.State.TimeAttack;
		Application.LoadLevel("locker_room");
	}
	
	public void GoOneBall(){
		UIManager.instance.state = UIManager.State.OneBall;
		Application.LoadLevel("locker_room");
	}
	
	public void GoShop(){
		UIManager.instance.state = UIManager.State.Shop;
	}
	
	public void GoTutorial(){
		UIManager.instance.state = UIManager.State.Tutorial;
	}
	
	public void GoExit(){
		
	}

	public void GoStartGame(){
		InGameUIManager.instance.inGameState = InGameUIManager.InGameState.BeginGame;
		InGameUIManager.instance.substate = InGameUIManager.SubState.Init;

	}

	public void GoHome(){
		UIManager.instance.state = UIManager.State.MainMenu;
		Application.LoadLevel("freeKick_menu");
	}

	public void GoRestart(){
		GameManager.instance.score = 0;
		GameManager.instance.timer = 0;
		GameManager.instance.goalCounter = 0;
		InGameUIManager.instance.inGameState = InGameUIManager.InGameState.BeginGame;
		InGameUIManager.instance.substate = InGameUIManager.SubState.Init;
	}

	public void GoPause(){
		InGameUIManager.instance.inGameState = InGameUIManager.InGameState.PauseGame;
		InGameUIManager.instance.substate = InGameUIManager.SubState.Init;
	}

}
