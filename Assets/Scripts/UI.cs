using UnityEngine;
using System.Collections;

public class UI : MonoBehaviour
{
	public Texture2D btnTexture;
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnGui(){
		if (GUI.Button (new Rect (10, 10, 50, 50), btnTexture))
			GameManager.instance.SendMessage ("Reset");
	}
}

