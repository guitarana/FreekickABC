using UnityEngine;
using System.Collections;

public class UINavigation : MonoBehaviour {
	public AudioClip sfx;
	public AudioSource aud;
	// Use this for initialization
	void Start () {
		aud = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void StartGame(){
		aud.PlayOneShot(sfx);
		GameObject go = GameObject.Find ("audioBGM");
		StartCoroutine(IEStartGame());
		Destroy(go);
	}

	public IEnumerator IEStartGame(){
		yield return new WaitForSeconds(0.5f); 
		Application.LoadLevel("MainGame");
	}

	public void GoArcade(){
		aud.PlayOneShot(sfx);
		UIManager.instance.state = UIManager.State.Arcade;
		StartCoroutine(IEGoGame());
	}

	public IEnumerator IEGoGame(){
		yield return new WaitForSeconds(0.5f); 
		Application.LoadLevel("locker_room");
	}
	
	public void GoTimeAttack(){
		aud.PlayOneShot(sfx);
		UIManager.instance.state = UIManager.State.TimeAttack;
		StartCoroutine(IEGoGame());
	}


	
	public void GoOneBall(){
		aud.PlayOneShot(sfx);
		UIManager.instance.state = UIManager.State.OneBall;
		StartCoroutine(IEGoGame());
	}
	
	public void GoShop(){
		aud.PlayOneShot(sfx);
		UIManager.instance.state = UIManager.State.Shop;
		StartCoroutine(IEGoShop());
	}

	public IEnumerator IEGoShop(){
		yield return new WaitForSeconds(0.5f); 
		Application.LoadLevel("ShopMenu");
	}

	
	public void GoTutorial(){
		UIManager.instance.state = UIManager.State.Tutorial;
		aud.PlayOneShot(sfx);
		StartCoroutine(IEGoTut());
	}

	public IEnumerator IEGoTut(){
		yield return new WaitForSeconds(0.5f); 
		//Application.LoadLevel("ShopMenu");
	}

	
	public void GoExit(){
		aud.PlayOneShot(sfx);
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
		aud.PlayOneShot(sfx);

	}

	public void GoBeginGame(){
		//GameManager.instance.Reset();
		Time.timeScale = 0.6f;
		InGameUIManager.instance.inGameState = InGameUIManager.InGameState.BeginGame;
		InGameUIManager.instance.substate = InGameUIManager.SubState.Init;
		aud.PlayOneShot(sfx);
	}

	public void GoHome(){
		Time.timeScale = 0.6f;
		UIManager.instance.state = UIManager.State.MainMenu;
		Application.LoadLevel("freeKick_menu");
		aud.PlayOneShot(sfx);
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
		aud.PlayOneShot(sfx);
	}

	public void GoPause(){
		Time.timeScale = 0;
		InGameUIManager.instance.inGameState = InGameUIManager.InGameState.PauseGame;
		InGameUIManager.instance.substate = InGameUIManager.SubState.Init;
		aud.PlayOneShot(sfx);
	}

}
