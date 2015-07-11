using UnityEngine;
using System.Collections;

public class Debugger : MonoBehaviour {

	public UILabel label;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		label.text = InGameUIManager.instance.inGameState.ToString();
	}
}
