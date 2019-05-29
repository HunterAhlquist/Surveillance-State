using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(PlayerInput))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerAction))]
[RequireComponent(typeof(PlayerUI))]
[RequireComponent(typeof(PlayerLook))]
[RequireComponent(typeof(PlayerAudio))]

public class PlayerMain : MonoBehaviour {
    public enum GameState { Gameplay, Dialog, Cutscene, Menu, Map, Transition}
    

    [Header("Script Pointers")]
    public PlayerInput input;
    public PlayerProg prog;
    public PlayerMovement movement;
    public PlayerAction action;
    public PlayerUI UI;
    public PlayerLook view;
    public PlayerAudio playerAudio;
    [Space]
    public LevelManager levelManager;
    public OptionsManager optionsManager;

    [Header("Game States")]
    public GameState curGameState;
    public bool isAwake;

    private void Awake() {
        prog = GameObject.FindGameObjectWithTag("Dungeon Master").GetComponent<PlayerProg>();
        levelManager = GameObject.FindGameObjectWithTag("Dungeon Master").GetComponent<LevelManager>();
        optionsManager = GameObject.FindGameObjectWithTag("Dungeon Master").GetComponent<OptionsManager>();
    }
    private void Start() {
        action.StateSync();
        optionsManager.ApplyPlayerSettingsOnly();
        movement.WarpToNode("new game");
    }

    public bool PassiveLookCheck(out RaycastHit raycastHit) {
        RaycastHit hitInfo;
        if (Physics.Raycast(view.transform.position, view.transform.forward, out hitInfo, Mathf.Infinity)) {
            if (hitInfo.collider != null) {
                raycastHit = hitInfo;
                return true;
            }
        }
        raycastHit = hitInfo;
        return false;
    }
}