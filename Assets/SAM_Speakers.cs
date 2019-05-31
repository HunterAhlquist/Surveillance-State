using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SAM_Speakers : MonoBehaviour
{
    //audio clips arrays
    [Header("Clips")]
    public AudioClip[] investigateClips;
    public AudioClip[] alertClips;
    public AudioClip[] patrolClips;

    [Header("Pointers")] 
    public SAMMain samMain;

    public AudioSource speakerA;
    public AudioSource speakerB;
    public AudioSource speakerC;

    public SAMMain.SAMState prevState;

    // Update is called once per frame
    void Update() {
        if (samMain.isActive) {
            SpeakerLogic(speakerA);
            SpeakerLogic(speakerB);
            SpeakerLogic(speakerC);
            
        }
    }

    public void SpeakerLogic(AudioSource speaker) {
        if (!speaker.isPlaying || prevState != samMain.curDetectionLevel) {
            speaker.pitch = Random.Range(0.5f, 1.5f);
            if (prevState != samMain.curDetectionLevel) {
                prevState = samMain.curDetectionLevel;
                speakerA.Stop();
                speakerB.Stop();
                speakerC.Stop();
            }
            
            switch (samMain.curDetectionLevel) {
                case SAMMain.SAMState.Alert:
                    speaker.clip = alertClips[Random.Range(0, alertClips.Length - 1)];
                    speaker.Play();
                    break;
                case SAMMain.SAMState.Investigate:
                    speaker.clip = investigateClips[Random.Range(0, investigateClips.Length - 1)];
                    speaker.Play();
                    break;
            }
        }
        
    }
}
