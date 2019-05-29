using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour {
    [Header("PlayerMain")]
    public PlayerMain playerMain;

    [Header("Jumping")]
    ///progress
    public bool jump;
    public bool jumpLvl2;
    ///states 
    private byte maxJumps;
    private byte jumpsRemain;
    [Space]

    [Header("Airdashing")]
    ///progress
    public bool airdash;
    public bool airdashLvl2;
    ///states
    private bool didAirdash;

    private void FixedUpdate() {
        switch (playerMain.curGameState) {
            case PlayerMain.GameState.Gameplay:
                if (!playerMain.isAwake) {
                    CheckJump();
                    CheckAirdash();
                    CheckRun();
                    CheckCrouch();
                }
                break;
            case PlayerMain.GameState.Dialog:

                break;
            case PlayerMain.GameState.Cutscene:

                break;
        }
    }
    private void Update() {
        switch (playerMain.curGameState) {
            case PlayerMain.GameState.Gameplay:
                AbilityUpdate();
                CheckUse();
                CheckAltUse();
                StateSync();
                break;
            case PlayerMain.GameState.Dialog:

                break;
            case PlayerMain.GameState.Cutscene:

                break;
        }
    }

    public Entity.Type LookingAt() {
        bool lookingAtObject = false;
        RaycastHit hitInfo;

        if (Physics.Raycast(playerMain.view.camera.transform.position, playerMain.view.camera.transform.forward, out hitInfo, Mathf.Infinity)) {
            if (hitInfo.collider != null) {
                if (hitInfo.collider.GetComponent<EntityHeader>()) {
                    lookingAtObject = true;
                }
                
            }
        }

        if (lookingAtObject)
            return hitInfo.collider.GetComponent<EntityHeader>().entity.entityType;
        return Entity.Type.None;
    }
    public Entity.Type LookingAt(out Collider selectedObj) {
        bool lookingAtObject = false;
        RaycastHit hitInfo;

        if (Physics.Raycast(playerMain.view.camera.transform.position, playerMain.view.camera.transform.forward, out hitInfo, Mathf.Infinity)) {
            if (hitInfo.collider != null) {
                if (hitInfo.collider.GetComponent<EntityHeader>()) {
                    lookingAtObject = true;
                }

            }
        }

        if (lookingAtObject) {
            selectedObj = hitInfo.collider;
            return hitInfo.collider.GetComponent<EntityHeader>().entity.entityType;
        }
        selectedObj = null;
        return Entity.Type.None;
    }

    public void StateSync() {
        if (!jump && !jumpLvl2) {
            maxJumps = 0;
        } else if (jump && !jumpLvl2) {
            maxJumps = 1;
        } else if (jump && jumpLvl2) {
            maxJumps = 2;
        }
    }

    public void CheckJump() {
        if (playerMain.input.act_jump) {
            if (jumpsRemain > 0) {
                playerMain.movement.DoJump();
                playerMain.movement.inAir = true;
                playerMain.movement.onGround = false;
                jumpsRemain -= 1;
                return;
            }
        } else if (!playerMain.movement.inAir && playerMain.movement.onGround) {
                jumpsRemain = maxJumps;
        }
    }

    public void CheckAirdash() {
        if (playerMain.movement.onGround && didAirdash)
            didAirdash = false;

        if (playerMain.input.act_airdash && airdash) {
            if (!didAirdash && !playerMain.movement.onGround) {
                didAirdash = true;
                playerMain.movement.DoAirdash();
            }
        }
    }

    public void CheckRun() {
        if (playerMain.input.act_run) {
            playerMain.movement.running = true;
        } else {
            playerMain.movement.running = false;
        }
    }

    public void CheckUse() {
        if (playerMain.input.act_use) {
            RaycastHit info;
            if (Physics.Raycast(playerMain.view.camera.transform.position, playerMain.view.camera.forward, out info, 1)) {
                if (info.collider != null) {
                    if (info.collider.tag == "Entity")
                        info.collider.gameObject.GetComponent<EntityHeader>().beingUsed = true;
                }
            }
        }
    }

    public void CheckAltUse() {
        Collider selected;
        Entity.Type entType = LookingAt(out selected);
        if (playerMain.input.act_altUse) {
            switch (entType) {
                case Entity.Type.TPEye:
                    if (playerMain.prog.abilityEye)
                        selected.GetComponent<EntityHeader>().TPPlayerToSelf();
                    break;
            }
        }
    }

    public void CheckCrouch() {
        if (playerMain.input.act_crouch) {
            playerMain.movement.DoCrouch();
        } else {
            playerMain.movement.UndoCrouch();
        }
    }

    private void AbilityUpdate() {
        //sync with PlayerProg
        jump = playerMain.prog.abilityJump;
        jumpLvl2 = playerMain.prog.abilityJumpLvl2;

        airdash = playerMain.prog.abilityAirdash;
        airdashLvl2 = playerMain.prog.abilityAirdashLvl2;

    }

}
