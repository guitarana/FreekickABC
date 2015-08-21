using UnityEngine;
using System.Collections;

public class UINavigation : MonoBehaviour {
	public AudioClip sfx;
	public GameObject PanelMain;
	public GameObject PanelPlay;
	public GameObject PanelCustomize;
	public GameObject PanelTutorial;
	public GameObject PanelLanguage;


	// Use this for initialization
	void Start () {
		NGUITools.SetActive(PanelMain,true);
		NGUITools.SetActive(PanelPlay,false);
		NGUITools.SetActive(PanelCustomize,false);
		NGUITools.SetActive(PanelLanguage,false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ButtonPlay(){
		NGUITools.SetActive(PanelMain,false);
		NGUITools.SetActive(PanelPlay,true);
	}

	public void ButtonPlay_Back(){
		NGUITools.SetActive(PanelMain,true);
		NGUITools.SetActive(PanelPlay,false);
	}



	public void ButtonCustomize(){
		NGUITools.SetActive(PanelMain,false);
		NGUITools.SetActive(PanelCustomize,true);
	}

	public void ButtonCustomize_Back(){
		NGUITools.SetActive(PanelMain,true);
		NGUITools.SetActive(PanelCustomize,false);
	}

	public void ButtonLanguage(){
		NGUITools.SetActive(PanelCustomize,false);
		NGUITools.SetActive(PanelLanguage,true);
	}
	
	public void ButtonLanguage_Back(){
		NGUITools.SetActive(PanelCustomize,true);
		NGUITools.SetActive(PanelLanguage,false);
	}

	public void ButtonEnglish(){
		NGUITools.SetActive(PanelMain,false);
		NGUITools.SetActive(PanelLanguage,true);
		Localization.language="English";
	}

	public void ButtonIndonesia(){
		NGUITools.SetActive(PanelMain,false);
		NGUITools.SetActive(PanelLanguage,true);
		Localization.language="Indonesia";
	}
	public void ButtonSunda(){
		NGUITools.SetActive(PanelMain,false);
		NGUITools.SetActive(PanelLanguage,true);
		Localization.language="Sunda";
	}

	public void StartGame(){
		GameObject go = GameObject.Find ("audioBGM");
		StartCoroutine(IEStartGame());
		Destroy(go);
	}

	public IEnumerator IEStartGame(){
		yield return new WaitForSeconds(0.5f); 
		Application.LoadLevel("MainGame");
	}

	public void GoArcade(){
		UIManager.instance.state = UIManager.State.Arcade;
		StartCoroutine(IEStartGame());
	}

	public void GoLocker(){
		StartCoroutine(IEGoGame());
	}

	public IEnumerator IEGoGame(){
		yield return new WaitForSeconds(0.5f); 
		Application.LoadLevel("locker_room");
	}
	
	public void GoTimeAttack(){
		UIManager.instance.state = UIManager.State.TimeAttack;
		StartCoroutine(IEStartGame());
	}
	
	public void GoOneBall(){
		UIManager.instance.state = UIManager.State.OneBall;
		StartCoroutine(IEStartGame());
	}
	
	public void GoShop(){
		UIManager.instance.state = UIManager.State.Shop;
		StartCoroutine(IEGoShop());
	}

	public IEnumerator IEGoShop(){
		yield return new WaitForSeconds(0.5f); 
		Application.LoadLevel("ShopMenu");
	}

	
	public void GoTutorial(){
		UIManager.instance.state = UIManager.State.Tutorial;
		StartCoroutine(IEGoTut());
	}

	public IEnumerator IEGoTut(){
		yield return new WaitForSeconds(0.5f); 
		//Application.LoadLevel("ShopMenu");
	}

	
	public void GoExit(){
		StartCoroutine(IEGoExit());

	}

	public IEnumerator IEGoExit(){
		yield return new WaitForSeconds(0.5f); 
		Application.Quit();
	}

	public void GoStartGame(){
		Time.timeScale = 0.6f;
		GameManager.instance.Reset();
		InGameUIManager.instance.inGameState = InGameUIManager.InGameState.BeginGame;
		InGameUIManager.instance.substate = InGameUIManager.SubState.Init;

	}

	public void GoBeginGame(){
		//GameManager.instance.Reset();
		Time.timeScale = 0.6f;
		InGameUIManager.instance.inGameState = InGameUIManager.InGameState.BeginGame;
		InGameUIManager.instance.substate = InGameUIManager.SubState.Init;
	}

	public void GoHome(){
		Time.timeScale = 0.6f;
		UIManager.instance.state = UIManager.State.MainMenu;
		Application.LoadLevel("freeKick_menu");
		GameObject bgm = GameObject.Find("audioBGM");
		if(bgm)
			Destroy(bgm);
	}

	public void GoRestart(){
		Time.timeScale = 0.6f;
		GameManager.instance.score = 0;
		GameManager.instance.timer = 0;
		GameManager.instance.goalCounter = 0;
		GameManager.instance.Reset();
		InGameUIManager.instance.inGameState = InGameUIManager.InGameState.BeginGame;
		InGameUIManager.instance.substate = InGameUIManager.SubState.Init;
	}

	public void GoPause(){
		Time.timeScale = 0;
		InGameUIManager.instance.inGameState = InGameUIManager.InGameState.PauseGame;
		InGameUIManager.instance.substate = InGameUIManager.SubState.Init;
	}

}
