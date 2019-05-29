using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [Header("Objects")] public PlayerMain playerMain;
    [Space] public Rigidbody playerRbody;
    public BoxCollider collider;

    [Header("Mechanical Stats")] public float movementSpeed;
    public float curMovementSpeed;
    public float jumpStrength;
    public float airdashPower;
    public float flightSustain;

    [Header("Conditional Values")] public float dragNormal;
    public float dragUnderWater;
    [Space] public float normalJumpStrength;
    public float underwaterJumpStrength;

    [Header("Player States")] public bool crouching;
    public bool onGround;
    public bool underWater;
    public bool running;
    public bool walking;
    public bool inAir;

    [Header("Audio")] public float maxTick;
    public float curTick;

    [Header("Surfaces")] public PhysicMaterial[] surfaces;
    public PhysicMaterial curSurface;


    [Header("Phys Materials")] public PhysicMaterial sticky;
    public PhysicMaterial nonSticky;

    //landing vectors
    readonly Vector3 BOTTOM_PLAYER_RAY_F = new Vector3(0.45f, 0.05f, 0);
    readonly Vector3 BOTTOM_PLAYER_RAY_B = new Vector3(-0.45f, 0.05f, 0);
    readonly Vector3 BOTTOM_PLAYER_RAY_L = new Vector3(0, 0.05f, 0.45f);
    readonly Vector3 BOTTOM_PLAYER_RAY_R = new Vector3(0, 0.05f, -0.45f);

    readonly Vector3 BOTTOM_PLAYER_RAY_FL = new Vector3(0.45f, 0.05f, -0.45f);
    readonly Vector3 BOTTOM_PLAYER_RAY_FR = new Vector3(-0.45f, 0.05f, -0.45f);
    readonly Vector3 BOTTOM_PLAYER_RAY_BL = new Vector3(0.45f, 0.05f, 0.45f);
    readonly Vector3 BOTTOM_PLAYER_RAY_BR = new Vector3(-0.45f, 0.05f, 0.45f);

    public Mesh test;


    public void WarpToNode(string nodeName = "default") {
        Transform location = GameObject.Find("nodes").transform.Find(nodeName).transform;
        playerRbody.MovePosition(location.position);
        playerMain.view.camera.rotation = location.rotation;
    }

    private void FixedUpdate() {
        if (playerMain.curGameState == PlayerMain.GameState.Gameplay) {
            AlterSpeed();
            MoveUpdate();
            MoveSounds();
            if (!playerMain.isAwake)
                FlyForward();
        }
    }

    private void Update() {
        if (playerMain.curGameState == PlayerMain.GameState.Gameplay) {
            if (inAir)
                collider.material = nonSticky;

            if (!onGround)
                inAir = true;

            playerMain.playerAudio.UpdateSoundPack();

            if (inAir && onGround) {
                playerMain.playerAudio.PlayLanding();
                collider.material = sticky;
                inAir = false;
            }

            onGround = IsGrounded();
        }

        if (underWater) {
            playerMain.view.postEffect.mat = playerMain.view.postEffect.underwater;
            playerRbody.drag = dragUnderWater;
            jumpStrength = underwaterJumpStrength;
        }
        else {
            playerMain.view.postEffect.mat = playerMain.view.postEffect.normal;
            playerRbody.drag = dragNormal;
            jumpStrength = normalJumpStrength;
        }

        //StepCheck();
    }

    private void FlyForward() {
        if (inAir && playerMain.input.mv_flying && playerMain.prog.abilityFlight) {
            if (playerRbody.velocity.y < 0) { //check if the velocity is downward
                playerRbody.velocity = Vector3.zero;
                playerRbody.velocity += new Vector3(0, -playerRbody.velocity.y * flightSustain, 0);
                playerRbody.AddForce(playerMain.view.cameraRot.transform.forward * 10, ForceMode.Impulse);
            }
        }
    }

    private void StepCheck() {
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "water") {
            playerMain.movement.underWater = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "water") {
            playerMain.movement.underWater = false;
        }
    }

    public void DoAirdash() {
        if (playerMain.action.airdashLvl2) {
            playerRbody.AddForce(playerMain.view.camera.transform.forward * airdashPower, ForceMode.Impulse);
            return;
        }

        if (playerMain.action.airdash)
            playerRbody.AddForce(playerMain.view.cameraRot.transform.forward * airdashPower, ForceMode.Impulse);
    }

    public void AlterSpeed() {
        if (crouching) {
            curMovementSpeed = movementSpeed / 2.5f;
            return;
        }

        if (running) {
            curMovementSpeed = movementSpeed * 2;
            return;
        }

        curMovementSpeed = movementSpeed;
    }


    private void MoveUpdate() {
        if (onGround)
            collider.material = sticky;
        walking = true;
        if (playerMain.input.mv_forwardBack == 0 && playerMain.input.mv_leftRight == 0)
            walking = false;
        playerRbody.MovePosition(playerRbody.position + playerMain.view.cameraRot.transform.forward *
                                 (playerMain.input.mv_forwardBack * curMovementSpeed) * Time.deltaTime);
        playerRbody.MovePosition(playerRbody.position + playerMain.view.cameraRot.transform.right *
                                 (playerMain.input.mv_leftRight * curMovementSpeed) * Time.deltaTime);
    }

    private void MoveSounds() {
        if (walking && onGround) {
            if (running) {
                curTick -= 2;
            }
            else {
                curTick -= 1;
            }
        }

        if (curTick <= 0) { //subtract with curTickSub
            playerMain.playerAudio.PlayFootstep();
            curTick = maxTick;
        }
    }

    public void DoJump() {
        collider.material = nonSticky;
        onGround = false;
        playerRbody.velocity = Vector3.zero;


        if (walking) {
            playerRbody.AddForce((transform.up * jumpStrength) + playerMain.view.cameraRot.transform.forward * 15,
                ForceMode.Impulse);
        }
        else {
            playerRbody.AddForce((transform.up * jumpStrength), ForceMode.Impulse);
        }
    }

    public void DoCrouch() {
        collider.material = nonSticky;
        if (collider.size.y > 1) {
            collider.size -= new Vector3(0, 0.05f, 0);
        }
        else if (collider.size.y != 1) {
            collider.size = new Vector3(1, 1, 1);
        }
        else {
            crouching = true;
            collider.material = sticky;
        }

        if (collider.center.y < 0.5f) {
            collider.center += new Vector3(0, 0.05f, 0);
        }
        else if (collider.center.y != 0.5f) {
            collider.center = new Vector3(0, 0.5f, 0);
        }
    }

    public void UndoCrouch() {
        RaycastHit hitInfo;
        Debug.DrawRay(transform.position + new Vector3(0, 1, 0), Vector3.up, Color.white);
        if (!Physics.Raycast(transform.position + new Vector3(0, 1, 0), Vector3.up, out hitInfo, 0.5f)) {
            if (collider.size.y < 2) {
                collider.size += new Vector3(0, 0.05f, 0);
            }
            else if (collider.size.y != 2) {
                collider.size = new Vector3(1, 2, 1);
            }
            else {
                crouching = false;
            }

            if (collider.center.y > 0) {
                collider.center -= new Vector3(0, 0.05f, 0);
            }
            else if (collider.center.y != 0) {
                collider.center = new Vector3(0, 0, 0);
            }
        }
    }

    private bool IsGrounded() {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position - BOTTOM_PLAYER_RAY_F + new Vector3(0, 0.05f, 0), Vector3.down,
            out hitInfo, 1.01f, ~0, QueryTriggerInteraction.Ignore)) {
            if (hitInfo.collider != null) {
                if (hitInfo.collider.material == null) {
                    curSurface = surfaces[0];
                }
                else {
                    curSurface = hitInfo.collider.material;
                }

                return true;
            }
            else {
                return false;
            }
        }
        else if (Physics.Raycast(transform.position - BOTTOM_PLAYER_RAY_B + new Vector3(0, 0.05f, 0), Vector3.down,
            out hitInfo, 1.01f, ~0, QueryTriggerInteraction.Ignore)) {
            if (hitInfo.collider != null) {
                if (hitInfo.collider.material == null) {
                    curSurface = surfaces[0];
                }
                else {
                    curSurface = hitInfo.collider.material;
                }

                return true;
            }
            else {
                return false;
            }
        }
        else if (Physics.Raycast(transform.position - BOTTOM_PLAYER_RAY_L + new Vector3(0, 0.05f, 0), Vector3.down,
            out hitInfo, 1.01f, ~0, QueryTriggerInteraction.Ignore)) {
            if (hitInfo.collider != null) {
                if (hitInfo.collider.material == null) {
                    curSurface = surfaces[0];
                }
                else {
                    curSurface = hitInfo.collider.material;
                }

                return true;
            }
            else {
                return false;
            }
        }
        else if (Physics.Raycast(transform.position - BOTTOM_PLAYER_RAY_R + new Vector3(0, 0.05f, 0), Vector3.down,
            out hitInfo, 1.01f, ~0, QueryTriggerInteraction.Ignore)) {
            if (hitInfo.collider != null) {
                if (hitInfo.collider.material == null) {
                    curSurface = surfaces[0];
                }
                else {
                    curSurface = hitInfo.collider.material;
                }

                return true;
            }
            else {
                return false;
            }
        }
        else if (Physics.Raycast(transform.position - BOTTOM_PLAYER_RAY_FR + new Vector3(0, 0.05f, 0), Vector3.down,
            out hitInfo, 1.01f, ~0, QueryTriggerInteraction.Ignore)) {
            if (hitInfo.collider != null) {
                if (hitInfo.collider.material == null) {
                    curSurface = surfaces[0];
                }
                else {
                    curSurface = hitInfo.collider.material;
                }

                return true;
            }
            else {
                return false;
            }
        }
        else if (Physics.Raycast(transform.position - BOTTOM_PLAYER_RAY_FL + new Vector3(0, 0.05f, 0), Vector3.down,
            out hitInfo, 1.01f, ~0, QueryTriggerInteraction.Ignore)) {
            if (hitInfo.collider != null) {
                if (hitInfo.collider.material == null) {
                    curSurface = surfaces[0];
                }
                else {
                    curSurface = hitInfo.collider.material;
                }

                return true;
            }
            else {
                return false;
            }
        }
        else if (Physics.Raycast(transform.position - BOTTOM_PLAYER_RAY_BR + new Vector3(0, 0.05f, 0), Vector3.down,
            out hitInfo, 1.01f, ~0, QueryTriggerInteraction.Ignore)) {
            if (hitInfo.collider != null) {
                if (hitInfo.collider.material == null) {
                    curSurface = surfaces[0];
                }
                else {
                    curSurface = hitInfo.collider.material;
                }

                return true;
            }
            else {
                return false;
            }
        }
        else if (Physics.Raycast(transform.position - BOTTOM_PLAYER_RAY_BL + new Vector3(0, 0.05f, 0), Vector3.down,
            out hitInfo, 1.01f, ~0, QueryTriggerInteraction.Ignore)) {
            if (hitInfo.collider != null) {
                if (hitInfo.collider.material == null) {
                    curSurface = surfaces[0];
                }
                else {
                    curSurface = hitInfo.collider.material;
                }

                return true;
            }
            else {
                return false;
            }
        }

        return false;
    }
}