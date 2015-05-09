using UnityEngine;
using System.Collections;

public class InGameUIManager : MonoBehaviour
{
	public static InGameUIManager instance;
	public enum InGameState{
		StartGame,
		BeginGame,
		PauseGame,
		WinGame,
		LoseGame,
		GameOver,
		Splash
	}

	public InGameState inGameState = InGameState.StartGame;

	public enum SubState{
		Init,
		Active,
		Deactive,
		Finish
	}

	public SubState substate = SubState.Init;

	public GameObject panelSplash;
	public GameObject panelInGame;
	public GameObject panelStartGame;
	public GameObject panelPauseGame;
	public GameObject panelWinGame;
	public GameObject panelLoseGame;
	public GameObject panelGameOver;

	//InGameHUD
	public UILabel goalText;
	public UILabel timeText;
	public UILabel scoreText;
	public UILabel kreditText;
	public UILabel labelText;

	//StartHUD
	public UILabel startLevelText;
	public UILabel startTargetText;
	public UILabel startYourScoreText;

	//PauseHUD
	public UILabel pauseLevelText;
	public UILabel pauseHighScoreText;
	public UILabel pauseYourScoreText;
	public UILabel pauseShotsText;
	public UILabel pauseGoalsText;
	public UILabel pauseSuccessText;
	public UILabel pauseGoalKeeperText;

	//WinsHUD
	public UILabel winLevelText;
	public UILabel winTargetText;
	public UILabel winYourScoreText;

	//LoseHUD
	public UILabel loseLevelText;
	public UILabel loseTargetText;
	public UILabel loseYourScoreText;

	//GameOverHUD
	public UILabel gameOverLevelText;
	public UILabel gameOverHighScoreText;
	public UILabel gameOverYourScoreText;
	public UILabel gameOverShotsText;
	public UILabel gameOverGoalsText;
	public UILabel gameOverSuccessText;
	public UILabel gameOverBonusText;
	public UILabel gameOverGoalKeeperText;
	public UILabel gameOverXPGained;

	// Use this for initialization
	void Start ()
	{
		instance = this;
	}

	void DoSplash(){

		if(substate == SubState.Init){
			DisableAllPanel();
			panelSplash.SetActive(true);
			substate = SubState.Active;
		}

		if(substate == SubState.Active){
			substate = SubState.Deactive;
		}

		if(substate == SubState.Deactive){
			substate = SubState.Finish;
		}

		if(substate == SubState.Finish){
			
		}

	}

	void DoStartGame(){
		
		if(substate == SubState.Init){
			DisableAllPanel();
			if(UIManager.instance.state == UIManager.State.Arcade)
				labelText.text = "Ball";
			if(UIManager.instance.state == UIManager.State.TimeAttack)
				labelText.text = "Time";
			if(UIManager.instance.state == UIManager.State.OneBall){
			
			}

			PlayerStatistic.instance.SaveGame();
			panelStartGame.SetActive(true);
			substate = SubState.Active;
		}
		
		if(substate == SubState.Active){
			substate = SubState.Deactive;
		}
		
		if(substate == SubState.Deactive){
			substate = SubState.Finish;
		}
		
		if(substate == SubState.Finish){
			
		}
		
	}

	void DoBeginGame(){
		
		if(substate == SubState.Init){
			DisableAllPanel();
			PlayerStatistic.instance.SaveGame();
			panelInGame.SetActive(true);
		}
		
		if(substate == SubState.Active){
			substate = SubState.Deactive;
		}
		
		if(substate == SubState.Deactive){
			substate = SubState.Finish;
		}
		
		if(substate == SubState.Finish){
			
		}
		
	}

	void DoPauseGame(){
		
		if(substate == SubState.Init){
			DisableAllPanel();
			PlayerStatistic.instance.SaveGame();
			panelPauseGame.SetActive(true);
		}
		
		if(substate == SubState.Active){
			substate = SubState.Deactive;
		}
		
		if(substate == SubState.Deactive){
			substate = SubState.Finish;
		}
		
		if(substate == SubState.Finish){
			
		}
		
	}

	void DoWinGame(){
		
		if(substate == SubState.Init){
			DisableAllPanel();
			PlayerStatistic.instance.SaveGame();
			panelWinGame.SetActive(true);
		}
		
		if(substate == SubState.Active){
			substate = SubState.Deactive;
		}
		
		if(substate == SubState.Deactive){
			substate = SubState.Finish;
		}
		
		if(substate == SubState.Finish){
			
		}
		
	}

	void DoLoseGame(){
		
		if(substate == SubState.Init){
			DisableAllPanel();
			PlayerStatistic.instance.SaveGame();
			panelLoseGame.SetActive(true);
		}
		
		if(substate == SubState.Active){
			substate = SubState.Deactive;
		}
		
		if(substate == SubState.Deactive){
			substate = SubState.Finish;
		}
		
		if(substate == SubState.Finish){
			
		}
		
	}

	void DoGameOver(){
		
		if(substate == SubState.Init){
			DisableAllPanel();
			PlayerStatistic.instance.SaveGame();
			panelGameOver.SetActive(true);
		}
		
		if(substate == SubState.Active){
			substate = SubState.Deactive;
		}
		
		if(substate == SubState.Deactive){
			substate = SubState.Finish;
		}
		
		if(substate == SubState.Finish){
			
		}
		
	}

	void DisableAllPanel(){
		panelSplash.SetActive(false);
		panelInGame.SetActive(false);
		panelStartGame.SetActive(false);
		panelPauseGame.SetActive(false);
		panelWinGame.SetActive(false);
		panelLoseGame.SetActive(false);
		panelGameOver.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
	{
		switch(inGameState){
		case InGameState.StartGame:
			DoStartGame();
			break;
		case InGameState.PauseGame:
			DoPauseGame();
			break;
		case InGameState.WinGame:
			DoWinGame();
			break;
		case InGameState.LoseGame:
			DoLoseGame();
			break;
		case InGameState.GameOver:
			DoGameOver();
			break;
		case InGameState.Splash:
			DoSplash();
			break;
		case InGameState.BeginGame:
			DoBeginGame();
			break;
		}
	}
}

