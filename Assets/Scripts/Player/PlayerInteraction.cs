using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {
    [Header("Object pointers")] 
    public PlayerMain playerMain;
    public GameObject currentlyHeldObject;
    public Transform objectHoldPoint;
    
    [Header("States")] 
    public bool objectInHand;


    private void Update() {
        HoldOnToObjectVisual();
    }

    private void FixedUpdate() {
        InteractCheck();
        ThrowCheck();
    }

    //pickup/drop/throw/hold code
    private void PickupObject(GameObject objectToPickUp) {
        currentlyHeldObject = objectToPickUp;
        currentlyHeldObject.GetComponent<Entity>().pickedUp = true;
        objectInHand = true;
    }
    
    public void HoldOnToObjectVisual() { //update the position of the object in "currentlyHeldObject" to the "objectHoldPoint"
        if (currentlyHeldObject) {
            currentlyHeldObject.transform.position = objectHoldPoint.position;
        }
    }

    public void ThrowCheck() {
        if (currentlyHeldObject) {
            if (playerMain.input.use < 0) {
                ThrowObject();
            }
        }
    }
    public void ThrowObject() {
        //add force to object
        GameObject throwMe = currentlyHeldObject;
        currentlyHeldObject = null;
        throwMe.GetComponent<Rigidbody>().isKinematic = false;
        throwMe.GetComponent<Rigidbody>().AddForce(playerMain.view.camera.forward * 500);
        throwMe.GetComponent<Entity>().pickedUp = false;
        
    }

    public void DropObject() {
        
    }

    public void InteractCheck() {
        if (!currentlyHeldObject) {
            RaycastHit hit;
            int hitMask = LayerMask.GetMask("Entity");
            Ray raycast = new Ray(playerMain.view.camera.transform.position, playerMain.view.camera.forward);
            if (Physics.Raycast(raycast, out hit, 1.5f, hitMask, QueryTriggerInteraction.Ignore)) {//cast a ray for anything on the entity layer
                switch (hit.collider.GetComponent<Entity>().thisEntityType) {//check what kind of entity
                    case Entity.EntityType.Pickup:
                        //(insert method to change crosshair to the pickup graphic
                        if (playerMain.input.use > 0) {
                            PickupObject(hit.collider.gameObject);
                        }
                        break;
                    
                    case Entity.EntityType.Button:
                        //(insert method to change crosshair to the activate graphic
                        if (playerMain.input.use > 0) {
                            hit.collider.GetComponent<Entity>().ActivateEntity();
                        }
                        break;
                }
            }
        }
    }
}
