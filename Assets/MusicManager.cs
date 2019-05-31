using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public SAMMain samMain;
    public AudioSource snd;

    public AudioClip amb;
    public AudioClip music;

    // Update is called once per frame
    void Update()
    {
        if (samMain.hadIntro && snd.clip == amb) {
            snd.Stop();
            snd.clip = music;
            snd.Play();
        } else if (samMain.isActive && snd.clip == amb) {
            snd.Stop();
        }

        if (samMain.hadIntro) {
            if (samMain.curDetectionLevel == SAMMain.SAMState.Alert || samMain.curDetectionLevel == SAMMain.SAMState.Investigate) {
                snd.volume = 0.2f;
            } else {
                snd.volume = 0.75f;
            }
        }
    }
}
