using UnityEngine;
using System.Collections;

public class AnimMirror : MonoBehaviour {

	public Animation anim;
	private Animation thisAnim;
	// Use this for initialization
	void Start () {
		thisAnim = this.GetComponent<Animation>();
	}
	
	// Update is called once per frame
	void Update () {
		if(anim.isPlaying)
			thisAnim.Play();
		thisAnim.clip = anim.clip;
	}
}
