using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Entity : MonoBehaviour
{
    public enum EntityType {Pickup, Button, Trigger}
    [Header("Parameters")]
    public EntityType thisEntityType;

    [Header("Pickup States")]
    public bool pickedUp;

    public bool thrown;

    public float SAMThreshhold;

    [Header("Button States")] 
    public bool activate;
    
    [Header("Button Properties")] 
    public bool activePerm;

    public byte SAMLevel;

    [Header("Trigger States")] 
    public bool active;

    public int timesActive;

    [Header("Pointers")]
    public Rigidbody thisEntityRigidbody;

    public AudioSource objSounds;

    public Light buttonLight;
    public MeshRenderer renderer;
    public Texture buttonOnTex;
    public Texture buttonOffTex;

    public SAMMain samMain;
    
    //updates
    private void Start() {
        if (GetComponent<Rigidbody>()) {
            thisEntityRigidbody = GetComponent<Rigidbody>();
        }
        samMain = GameObject.FindWithTag("SAM").GetComponent<SAMMain>();

    }
    private void FixedUpdate() {
        switch (thisEntityType)
        {
            case EntityType.Button:
                ButtonCheck();
                break;
            case EntityType.Trigger:
                break;
            case EntityType.Pickup:
                PickedUpCheck();
                ObjectThrownCheck();
                break;
        }
        
    }

    private void ButtonCheck() {
        if (activate) {
            buttonLight.color = Color.green;
            renderer.materials[0].SetTexture("_BaseMap", buttonOnTex);
            renderer.materials[1].SetTexture("_BaseMap", buttonOnTex);
            if (samMain.accessLevel < SAMLevel) {
                samMain.accessLevel = SAMLevel;
            }
        }
        else {
            buttonLight.color = Color.red;
            renderer.materials[0].SetTexture("_BaseMap", buttonOffTex);
            renderer.materials[1].SetTexture("_BaseMap", buttonOffTex);
        }
    }

    private void OnTriggerEnter(Collider hit) {
        if (hit.gameObject.layer == 10) {//if the object hits another entity on the entity layer
            if (hit.GetComponent<Entity>().thrown) {
                activate = true;
            }
        }
            
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.layer == 9 || other.gameObject.layer == 14) {
            if (thisEntityRigidbody.velocity.magnitude > 2) {
                objSounds.Play();
                if (samMain.hadIntro)
                    samMain.HeardNoise(gameObject);
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
        if (thisEntityRigidbody.velocity.magnitude > 0.2f) {
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
