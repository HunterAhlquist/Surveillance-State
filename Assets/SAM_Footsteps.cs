using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SAM_Footsteps : MonoBehaviour {
    public AudioSource steps;
    public SAMMain samMain;

    public AudioClip step;
    public AudioClip lift;

    private bool played;
    private void OnTriggerEnter(Collider other) {
        if (!played && other.gameObject.layer == 14 && samMain.isActive) {
            //Debug.Log("Step! " + transform.name);
            steps.clip = step;
            steps.Play();
            played = true;
        }
            
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.layer == 14 && played) {
            //steps.clip = lift;
            //steps.Play();
            played = false;
        }
    }
}
