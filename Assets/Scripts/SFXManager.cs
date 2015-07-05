using UnityEngine;
using System.Collections;

public class SFXManager : MonoBehaviour {

	public AudioClip crowdSFXStart;
	public AudioClip crowdSFXHigh;
	public AudioClip crowdSFXLow;
	public AudioClip crowdSFXEnd;
	public AudioClip crowdSFXGoal;

	public AudioSource aud1;
	public AudioSource aud2;
	public AudioSource aud3;
	
	public float timer1;
	public float timer2;

	// Use this for initialization
	void Start () {
		StartCoroutine(StartCrowd());
	}
	
	// Update is called once per frame
	void Update () {
		LoopAMBSFX(aud1,ref timer1);
		LoopAMBSFX(aud2,ref timer2);
		if(GameState.instance.isGoal){
			aud3.clip = crowdSFXGoal;
			aud3.Play();
		}
	}

	public IEnumerator StartCrowd(){
		aud1.clip = crowdSFXStart;
		aud1.Play();
		yield return new WaitForSeconds(10);
		aud2.clip = crowdSFXStart;
		aud2.Play();
		yield break;
	}

	void LoopAMBSFX(AudioSource aud,ref float timer){
		timer += Time.deltaTime;
		if(aud.clip == crowdSFXStart){
			if(timer >= crowdSFXStart.length-0.5f){
				aud.clip = crowdSFXHigh;
				aud.Play();
				timer = 0;
			}
		}
		if(aud.clip == crowdSFXHigh){
			if(timer >= crowdSFXHigh.length-0.5f){
				aud.clip = crowdSFXEnd;
				aud.Play();
				timer = 0;
			}
		}
		if(aud.clip == crowdSFXEnd){
			if(timer >= crowdSFXEnd.length-0.5f){
				aud.clip = crowdSFXStart;
				aud.Play();
				timer = 0;
			}
		}
	}
}
