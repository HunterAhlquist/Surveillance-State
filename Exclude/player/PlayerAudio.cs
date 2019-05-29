using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [Header("Script Pointers and Objects")]
    public PlayerMain playerMain;
    public SoundTrackManager music;

    [Header("Audio Sources")]
    public AudioSource footstep;
    public AudioSource landing;

    [Space]
    [Header("Content")]
    public Soundtrack[] soundtracks;
    public EnvSoundsPacks[] soundPacks;
    ///states
    public EnvSoundsPacks curSoundPack;

    private void Start() {
        music = playerMain.prog.gameObject.transform.Find("audSoundtrack").GetComponent<SoundTrackManager>();
    }

    private void Update() {
        switch (playerMain.curGameState) {
            case PlayerMain.GameState.Menu:
                if (Time.timeScale != 0)
                    Time.timeScale = 0;
                playerMain.playerAudio.music.PauseMusic(true);
                break;
            case PlayerMain.GameState.Gameplay:
                if (Time.timeScale != 1)
                    Time.timeScale = 1;
                playerMain.playerAudio.music.ResumeMusic(true);
                break;
        }
    }

    public void UpdateSoundPack() {
        if (playerMain.movement.curSurface != null) {
            for (int i = 0; i < soundPacks.Length; i++) {
                if (playerMain.movement.curSurface.name == soundPacks[i].name || playerMain.movement.curSurface.name == soundPacks[i].name + " (Instance)") {
                    curSoundPack = soundPacks[i];
                    return;
                }
            }
        }
        
        curSoundPack = soundPacks[0];
    }

    public void PlayFootstep() {
        if (curSoundPack != null) {
            footstep.clip = curSoundPack.footsteps[Random.Range(0, curSoundPack.footsteps.Length)];
            footstep.Play();
        }
            
    }
    public void PlayLanding() {
        if (curSoundPack != null) {
            landing.clip = curSoundPack.landings[Random.Range(0, curSoundPack.landings.Length)];

            landing.volume = FMMath.Remap(playerMain.movement.playerRbody.velocity.magnitude, 
                                          2, 12, 0.1f, 1);

            landing.Play();
        }
    }
}
