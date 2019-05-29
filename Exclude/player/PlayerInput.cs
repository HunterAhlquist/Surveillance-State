using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {
    public PlayerMain playerMain;

    //Input Variables
    //movement
    [Header("Input Variables")]
    public float mv_forwardBack;
    public float mv_leftRight;
    public bool mv_flying;

    //world actions
    public bool act_airdash;
    public bool act_jump;
    public bool act_run;
    public bool act_altUse;
    public bool act_use;
    public bool act_crouch;

    //UI
    public bool UI_pause;
    public bool UI_menu;

    //Settings
    [Space]
    [Header("Settings")]
    public bool gamepadMode;

    public void InputUpdate() {
        //input updates
        if (!gamepadMode) {//keyboard
            ///axis
            mv_forwardBack = Input.GetAxis("Forward Back");
            mv_leftRight = Input.GetAxis("Left Right");
            mv_flying = Input.GetButton("Jump");
            ///buttons
            act_airdash = Input.GetButtonDown("Run");
            act_run = Input.GetButton("Run");
            act_jump = Input.GetButtonDown("Jump");
            if (Application.isFocused) {
                act_use = Input.GetButtonDown("Use");
                act_altUse = Input.GetButtonDown("Alt Use");
            }
            UI_pause = Input.GetButtonDown("Pause");
            UI_menu = Input.GetButtonDown("Menu");
            act_crouch = Input.GetButton("Crouch");

        } else {//gamepad

        }
    }


    // Update is called once per frame
    void Update() {
        //mouse
        if (playerMain.curGameState == PlayerMain.GameState.Gameplay || playerMain.curGameState == PlayerMain.GameState.Transition) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        } else if (playerMain.curGameState == PlayerMain.GameState.Map){

        } else {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (playerMain.curGameState != PlayerMain.GameState.Transition) {
            InputUpdate();
        }

    }
}
