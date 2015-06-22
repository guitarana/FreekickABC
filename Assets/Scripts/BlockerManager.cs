using UnityEngine;
using System.Collections;

public class BlockerManager : MonoBehaviour
{
	public GameObject[] blocker;
	public int totalBlocker;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void Init(){
		for(int i=0;i<blocker.Length;i++){
			blocker[i].SetActive(false);
		}  
		for(int i=0;i<totalBlocker;i++){
			blocker[i].GetComponent<OpponentBlockerAI>().aiState = OpponentBlockerAI.AIState.Idle;
			blocker[i].GetComponent<OpponentBlockerAI>().substate = OpponentBlockerAI.SubState.Init;
			blocker[i].SetActive(true);
		}
	}
}

