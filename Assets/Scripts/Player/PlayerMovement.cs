using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [Header("Pointers")] 
    public PlayerMain playerMain;
    public Rigidbody playerRbody;
    public BoxCollider collider;
    
    
    //landing vectors
    readonly Vector3 BOTTOM_PLAYER_RAY_F = new Vector3(0.45f, 0.05f, 0);
    readonly Vector3 BOTTOM_PLAYER_RAY_B = new Vector3(-0.45f, 0.05f, 0);
    readonly Vector3 BOTTOM_PLAYER_RAY_L = new Vector3(0, 0.05f, 0.45f);
    readonly Vector3 BOTTOM_PLAYER_RAY_R = new Vector3(0, 0.05f, -0.45f);

    readonly Vector3 BOTTOM_PLAYER_RAY_FL = new Vector3(0.45f, 0.05f, -0.45f);
    readonly Vector3 BOTTOM_PLAYER_RAY_FR = new Vector3(-0.45f, 0.05f, -0.45f);
    readonly Vector3 BOTTOM_PLAYER_RAY_BL = new Vector3(0.45f, 0.05f, 0.45f);
    readonly Vector3 BOTTOM_PLAYER_RAY_BR = new Vector3(-0.45f, 0.05f, 0.45f);
    
    

    [Header("Mechanical Stats")]
    public float movementSpeed;

    private void FixedUpdate() {
        playerMain.onGround = IsGrounded();
        MoveUpdate();
        if (playerMain.crouching) {
            DoCrouch();
        }
        else {
            UndoCrouch();
        }

        if (playerMain.moving && playerMain.running) {
            if (playerMain.stamina > 5)
                playerMain.stamina -= Time.deltaTime * 20;
            if (playerMain.stamina < 5)
                playerMain.stamina = 5;
        } else if (playerMain.moving) {
            if (playerMain.stamina < 100)
                playerMain.stamina += Time.deltaTime * 10;
            if (playerMain.stamina > 100)
                playerMain.stamina = 100;
        }
        else {
            if (playerMain.stamina < 100)
                playerMain.stamina += Time.deltaTime * 16;
            if (playerMain.stamina > 100)
                playerMain.stamina = 100;
        }
    }

    private void MoveUpdate() {
        if (playerMain.input.mv_forwardBack == 0 && playerMain.input.mv_leftRight == 0) {
            playerMain.moving = false;
        }
        else {
            playerMain.moving = true;
        }

        if (playerMain.input.act_run && !playerMain.crouching) {
            playerMain.running = true;
            playerRbody.MovePosition(playerRbody.position + playerMain.view.cameraRot.transform.forward *
                                     (playerMain.input.mv_forwardBack * movementSpeed * 2) * Time.deltaTime);
            playerRbody.MovePosition(playerRbody.position + playerMain.view.cameraRot.transform.right *
                                     (playerMain.input.mv_leftRight * movementSpeed * 2) * Time.deltaTime);
        } else if (playerMain.crouching) {
            playerMain.running = false;
            playerRbody.MovePosition(playerRbody.position + playerMain.view.cameraRot.transform.forward *
                                     (playerMain.input.mv_forwardBack * movementSpeed / 2) * Time.deltaTime);
            playerRbody.MovePosition(playerRbody.position + playerMain.view.cameraRot.transform.right *
                                     (playerMain.input.mv_leftRight * movementSpeed / 2) * Time.deltaTime);
        } else {
            playerMain.running = false;
            playerRbody.MovePosition(playerRbody.position + playerMain.view.cameraRot.transform.forward *
                                     (playerMain.input.mv_forwardBack * movementSpeed) * Time.deltaTime);
            playerRbody.MovePosition(playerRbody.position + playerMain.view.cameraRot.transform.right *
                                     (playerMain.input.mv_leftRight * movementSpeed) * Time.deltaTime);
        }
        
    }
    
    private void DoCrouch() {
        //shrink size
        if (collider.size.y > 1) {
            collider.size -= new Vector3(0, 0.1f, 0);
        }
        else if (collider.size.y != 1) {
            collider.size = new Vector3(1, 1, 1);
        }
        else {
            playerMain.crouched = true;
        }
        //shrink center
        if (collider.center.y > 0.5f) {
            collider.center -= new Vector3(0, 0.1f, 0);
        }
        else if (collider.center.y != 1) {
            collider.center = new Vector3(0, 0.5f, 0);
        }
        
        //lower player view
        if (playerMain.view.camera.transform.localPosition.y > 0.7f) {
            playerMain.view.camera.transform.localPosition -= new Vector3(0, 0.1f, 0);
        } else if (playerMain.view.camera.transform.localPosition.y != 0.7f) {
            playerMain.view.camera.transform.localPosition = new Vector3(playerMain.view.camera.transform.localPosition.x, 0.7f, playerMain.view.camera.transform.localPosition.z);
        }
    }
    public void UndoCrouch() {
        RaycastHit hitInfo;
        Debug.DrawRay(transform.position + new Vector3(0, 1, 0), Vector3.up, Color.white);
        if (!Physics.Raycast(transform.position + new Vector3(0, 1, 0), Vector3.up, out hitInfo, 0.5f, ~0, QueryTriggerInteraction.Ignore)) {
            //return to normal size
            if (collider.size.y < 2) {
                collider.size += new Vector3(0, 0.05f, 0);
            }
            else if (collider.size.y != 2) {
                collider.size = new Vector3(1, 2, 1);
            }
            else {
                playerMain.crouched = false;
            }
            //return to normal center
            if (collider.center.y < 1) {
                collider.center += new Vector3(0, 0.05f, 0);
            }
            else if (collider.center.y != 0) {
                collider.center = new Vector3(0, 1, 0);
            }
            
            //raise player view
            if (playerMain.view.camera.transform.localPosition.y < 1.7f) {
                playerMain.view.camera.transform.localPosition += new Vector3(0, 0.05f, 0);
            } else if (playerMain.view.camera.transform.localPosition.y != 1.7f) {
                playerMain.view.camera.transform.localPosition = new Vector3(playerMain.view.camera.transform.localPosition.x, 1.7f, playerMain.view.camera.transform.localPosition.z);
            }
        }
    }

    public void PlayerLean() { 
        //find the middle of the player collider
        Vector3 midPoint = transform.position + new Vector3(0, collider.size.y / 2, 0);

        //move along the
    }
    
    private bool IsGrounded() {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position - BOTTOM_PLAYER_RAY_F + new Vector3(0, 0.05f, 0), Vector3.down,
            out hitInfo, 1.01f, ~0, QueryTriggerInteraction.Ignore)) {
            if (hitInfo.collider != null) {

                return true;
            }
            else {
                return false;
            }
        }
        else if (Physics.Raycast(transform.position - BOTTOM_PLAYER_RAY_B + new Vector3(0, 0.05f, 0), Vector3.down,
            out hitInfo, 1.01f, ~0, QueryTriggerInteraction.Ignore)) {
            if (hitInfo.collider != null) {

                return true;
            }
            else {
                return false;
            }
        }
        else if (Physics.Raycast(transform.position - BOTTOM_PLAYER_RAY_L + new Vector3(0, 0.05f, 0), Vector3.down,
            out hitInfo, 1.01f, ~0, QueryTriggerInteraction.Ignore)) {
            if (hitInfo.collider != null) {

                return true;
            }
            else {
                return false;
            }
        }
        else if (Physics.Raycast(transform.position - BOTTOM_PLAYER_RAY_R + new Vector3(0, 0.05f, 0), Vector3.down,
            out hitInfo, 1.01f, ~0, QueryTriggerInteraction.Ignore)) {
            if (hitInfo.collider != null) {

                return true;
            }
            else {
                return false;
            }
        }
        else if (Physics.Raycast(transform.position - BOTTOM_PLAYER_RAY_FR + new Vector3(0, 0.05f, 0), Vector3.down,
            out hitInfo, 1.01f, ~0, QueryTriggerInteraction.Ignore)) {
            if (hitInfo.collider != null) {

                return true;
            }
            else {
                return false;
            }
        }
        else if (Physics.Raycast(transform.position - BOTTOM_PLAYER_RAY_FL + new Vector3(0, 0.05f, 0), Vector3.down,
            out hitInfo, 1.01f, ~0, QueryTriggerInteraction.Ignore)) {
            if (hitInfo.collider != null) {

                return true;
            }
            else {
                return false;
            }
        }
        else if (Physics.Raycast(transform.position - BOTTOM_PLAYER_RAY_BR + new Vector3(0, 0.05f, 0), Vector3.down,
            out hitInfo, 1.01f, ~0, QueryTriggerInteraction.Ignore)) {
            if (hitInfo.collider != null) {

                return true;
            }
            else {
                return false;
            }
        }
        else if (Physics.Raycast(transform.position - BOTTOM_PLAYER_RAY_BL + new Vector3(0, 0.05f, 0), Vector3.down,
            out hitInfo, 1.01f, ~0, QueryTriggerInteraction.Ignore)) {
            if (hitInfo.collider != null) {

                return true;
            }
            else {
                return false;
            }
        }

        return false;
    }
}
