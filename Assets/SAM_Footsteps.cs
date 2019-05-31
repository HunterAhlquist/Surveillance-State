using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SAM_Footsteps : MonoBehaviour {
    public AudioSource steps;

    private bool played;
    private void OnTriggerEnter(Collider other) {
        if (!played && other.gameObject.layer == 14) {
            //Debug.Log("Step! " + transform.name);
            steps.Play();
            played = true;
        }
            
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.layer == 14)
            played = false;
    }
}
