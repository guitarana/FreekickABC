using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

	public static UIManager _instance;

	public static UIManager instance {
		get {
			if (_instance == null) {
				GameObject go = FindObjectOfType(typeof(UIManager)) as GameObject;
				if (go == null)
					go = new GameObject("@UIMANAGER", typeof(UIManager));
				if (go != null) {
					DontDestroyOnLoad(go);
					_instance = go.GetComponent<UIManager>();
					Debug.Log("GET UIMANAGER");
				}
				if (_instance == null) {
					#if UNITY_EDITOR
					Debug.Log("/*UIMGR*/ ERROR: Can't create LevelManager object");
					Debug.Break();
					#else
					// = fatal error in release build
					Debug.LogError("/*UIMGR*/ ERROR: Can't create LevelManager object");
					Application.Quit();
					#endif
				}
			}
			return _instance;
		}
	}

	public void Create()
	{
		/* SHOULD BE EMPTY */
	}
	
	public void Init()
	{
		/* Initialization */
	}

	public enum State{
		MainMenu,
		Arcade,
		TimeAttack,
		OneBall,
		Shop,
		Tutorial
	}	

	public State state = State.MainMenu;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}


}
