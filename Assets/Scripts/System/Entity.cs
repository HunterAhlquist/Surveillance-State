using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public enum EntityType {Pickup, Button, Trigger}
    [Header("Parameters")]
    public EntityType thisEntityType;

    [Header("Pickup States")]
    public bool pickedUp;

    public bool thrown;

    [Header("Button States")] 
    public bool activate;
    
    [Header("Button Properties")] 
    public bool activePerm;

    [Header("Trigger States")] 
    public bool active;

    public int timesActive;

    [Header("Pointers")]
    public Rigidbody thisEntityRigidbody;
    
    //updates
    private void Start() {
        if (GetComponent<Rigidbody>()) {
            thisEntityRigidbody = GetComponent<Rigidbody>();
        }
    }
    private void FixedUpdate() {
        switch (thisEntityType)
        {
            case EntityType.Button:
                break;
            case EntityType.Trigger:
                break;
            case EntityType.Pickup:
                PickedUpCheck();
                ObjectThrownCheck();
                break;
        }
        
    }

    private void OnCollisionEnter(Collision hit) {
        if (thrown) {
            if (hit.gameObject.layer == 10) {//if the object hits another entity on the entity layer
                
            }
        }
    }

    //methods
    private void PickedUpCheck() {
        if (pickedUp) {
            thisEntityRigidbody.isKinematic = true;
        } else {
            thisEntityRigidbody.isKinematic = false;
        }
    }

    private void ObjectThrownCheck() {
        if (thisEntityRigidbody.velocity.magnitude > 0.5f) {
            thrown = true;
        }
        else {
            thrown = false;
        }
    }

    public void ActivateEntity() {
        if (!activate) {
            activate = true;
        } else if (activate && !activePerm) {
            activate = false;
        }
    }
    
}
