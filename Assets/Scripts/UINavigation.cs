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

}
