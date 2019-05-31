using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

public class PlayerMain : MonoBehaviour {
    [FormerlySerializedAs("stamima")]
    [Header("Player States")] 
    [Range(0,100)]
    public float stamina = 100;
    public bool gameOver;
    [Space]
    public bool onGround;
    public bool inAir;
    [FormerlySerializedAs("walking")] [Space]
    public bool moving;
    public bool running;
    public bool crouching;
    public bool crouched;

    public GameObject pauseUI;

    //Movement component
    public PlayerMovement movement;

    //Interaction component
    public PlayerInteraction interaction;
    
    //Input component
    public PlayerInput input;
    
    //Player View component
    public PlayerLook view;
    
    //UI component
    public PlayerUI ui;

    //System interfaces
    public enum GameState {Gameplay, Pause, Map, Transition}

    public GameState curGameState;

    private void Update() {
        if (stamina <= 0) {
            gameOver = true;
            SceneManager.LoadScene("lose", LoadSceneMode.Single);
        }
        switch (curGameState) {
            case GameState.Gameplay:
                pauseUI.SetActive(false);
                ui.UpdateStaminaBars(stamina);
                if (input.UI_pause) {
                    curGameState = GameState.Pause;
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    Time.timeScale = 0;
                    
                }
                break;
            case GameState.Pause:
                pauseUI.SetActive(true);
                if (input.UI_pause) {
                    curGameState = GameState.Gameplay;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    Time.timeScale = 1;
                }

                if (input.UI_Map) {
                    Debug.Log("quit game");
                    Application.Quit();
                }
                break;
            case GameState.Map:
                break;
            case GameState.Transition:
                break;
        }
        
    }
}


