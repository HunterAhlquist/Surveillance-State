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

    //world actions
    public bool act_run;
    public float use;

    //UI
    public bool UI_pause;
    public bool UI_Map;

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
            ///buttons
            act_run = Input.GetButton("Run");

            if (Application.isFocused) {
                use = Input.GetAxis("Use");
            }
            UI_pause = Input.GetButtonDown("Pause");
            UI_Map = Input.GetButtonDown("Map");
            if (Input.GetButtonDown("Crouch") && !playerMain.crouching) {
                playerMain.crouching = true;
            } else if (Input.GetButtonDown("Crouch") && playerMain.crouching) {
                playerMain.crouching = false;
            }

        } else {//gamepad

        }
    }


    // Update is called once per frame
    void Update() {
        //mouse
        if (playerMain != null) {
            if (playerMain.curGameState == PlayerMain.GameState.Gameplay || playerMain.curGameState == PlayerMain.GameState.Transition) {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            } else if (playerMain.curGameState == PlayerMain.GameState.Map) {

            } else {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        InputUpdate();

    }
}
