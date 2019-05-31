using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SAM_CameraArray : MonoBehaviour {
    public SAM_CameraDriver[] cameras;
    public SAM_CameraDriver armCamera;

    public GameObject pointOfInterest;
    
    // Start is called before the first frame update
    void Start() {
        cameras = FindAllCameras();
        LookAtTarget(true);
    }

    public SAM_CameraDriver[] FindAllCameras() {
        List<SAM_CameraDriver> camerasList = new List<SAM_CameraDriver>();

        foreach (Transform child in transform) {
            if (child.GetComponent<SAM_CameraDriver>()) {
                camerasList.Add(child.GetComponent<SAM_CameraDriver>());
            }
        }
        
        camerasList.Add(armCamera);
        return camerasList.ToArray();
    }

    public void LookAtTarget(bool active) {
        foreach (SAM_CameraDriver cam in cameras) {
            cam.lookAtTarget = active;
        }
    }

    public void ChangeInterest(GameObject lookAt = null) {
        foreach (SAM_CameraDriver cam in cameras) {
            cam.targetOfInterest = lookAt;
        }

        pointOfInterest = lookAt;
    }

    public bool GetInRangeAll() {
        foreach (SAM_CameraDriver cam in cameras) {
            if (cam.inRange) {
                return true;
            }
        }

        return false;
    }
}
