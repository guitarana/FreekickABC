/// <summary>
/// Animation event based sound. Created by Guitarana . Use for DreadOut Development Only.
/// this class is used for adding several sound into an animation using Animation Event.
/// </summary>

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AnimationEventBasedSound : MonoBehaviour
{
    [System.Serializable]
    public class AnimEvent
    {
        public string animationName;
        public AudioClip sfx;
        public float[] frames;
    }

    public List<AnimEvent> animEvent = new List<AnimEvent>(1);


    void Start()
    {
        AddEvent();
    }

    void AddEvent()
    {
        AnimationEvent e = new AnimationEvent();

        foreach (AnimEvent ae in animEvent)
        {
            e.functionName = "PlaySFX";
            e.objectReferenceParameter = ae.sfx;

            for (int i = 0; i < ae.frames.Length; i++)
            {
				try{
                e.time = ae.frames[i];
                GetComponent<Animation>()[ae.animationName].clip.AddEvent(e);
				}catch(NullReferenceException){
					Debug.Log("AnimationEventBasedSound.cs: Can't find " + ae.animationName);
				}
            }
        }
    }

    void PlaySFX(AudioClip aud)
    {
        GetComponent<AudioSource>().PlayOneShot(aud);
    }

}

