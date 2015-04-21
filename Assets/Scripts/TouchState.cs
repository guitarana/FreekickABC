using UnityEngine;
using System.Collections;

public class TouchState : MonoBehaviour
{
	public Touch touch;
		
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Application.platform == RuntimePlatform.Android){
			if (Input.touchCount > 0) {
				touch = Input.GetTouch(0);
				
			}

			if(Input.GetKeyDown(KeyCode.Escape))
				Application.Quit();
		}
	}
}

