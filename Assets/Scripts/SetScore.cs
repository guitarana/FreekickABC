using UnityEngine;
using System.Collections;

public class SetScore : MonoBehaviour
{
	public int score;
	public string name;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void SetScores(){
		StartCoroutine(CloudDataController.instance.PostScores(name,score));
	}
}

