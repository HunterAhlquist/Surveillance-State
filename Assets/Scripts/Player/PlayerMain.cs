using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMain : MonoBehaviour {
    [FormerlySerializedAs("stamima")]
    [Header("Player States")] 
    [Range(0,100)]
    public float stamina = 100;
    [Space]
    public bool onGround;
    public bool inAir;
    [FormerlySerializedAs("walking")] [Space]
    public bool moving;
    public bool running;
    public bool crouching;
    public bool crouched;
    
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
        switch (curGameState) {
            case GameState.Gameplay:
                ui.UpdateStaminaBars(stamina);
                break;
            case GameState.Pause:
                break;
            case GameState.Map:
                break;
            case GameState.Transition:
                break;
        }
        
    }
}


