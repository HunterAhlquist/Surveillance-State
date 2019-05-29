using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SAM_CameraDriver : MonoBehaviour {
    private GameObject playerCam;
    private PlayerMain playerMain;
    public GameObject targetOfInterest;
    [FormerlySerializedAs("lookAtPlayer")] public bool lookAtTarget;

    public Transform cameraRot;
    private Vector3 initialCameraForward;
    public bool isArmCamera;
    
    public float viewConeSize;
    public float viewDistance;
    public float rotSpeed;
    public bool inRange;
    
    // Start is called before the first frame update
    void Start() {
        if (transform.Find("Camera")) {
            cameraRot = transform.Find("Camera");
        } else if (transform.Find("ArmCamera")) {
            cameraRot = transform.Find("ArmCamera");
        }
        if (!isArmCamera)
            initialCameraForward = cameraRot.forward;
    }

    // Update is called once per frame
    void Update() {
        if (targetOfInterest != null && lookAtTarget) {
            LookCheck();
        }
        
    }

    private void LookCheck() {
        Vector3 toTargetVector = targetOfInterest.transform.position - transform.position;
        if (inRange) {
            LookOverTime(toTargetVector, rotSpeed);
        } else {
            if (isArmCamera) {
                LookOverTime(transform.parent.forward, rotSpeed * 2);
            }
            else {
                LookOverTime(initialCameraForward, rotSpeed);
            }
            
        }
        
        //Debug.Log(Vector3.Angle(transform.forward, toTargetVector));
        RaycastHit hit;
        Debug.DrawRay(transform.position, toTargetVector, Color.green);
        if (Physics.Raycast(transform.position, toTargetVector, out hit, Mathf.Infinity, ~LayerMask.GetMask("SAM", "Player"))) {
            Debug.DrawRay(transform.position, toTargetVector, Color.red);
            //Debug.Log(hit.collider.name + ", " + hit.collider.tag);
                if (Vector3.Angle(transform.forward, toTargetVector) < viewConeSize) {
                    if (toTargetVector.magnitude < viewDistance) {
                        inRange = true;
                    }
                    else {
                        inRange = false;
                    }
                }
                else {
                    inRange = false;
                }
        }
        else {
            inRange = false;
        }
    }
    
    private void LookOverTime(Vector3 lookVector, float speed) {
        Quaternion rot = Quaternion.LookRotation(lookVector);
        cameraRot.rotation = Quaternion.Slerp(cameraRot.transform.rotation, rot, speed * Time.deltaTime);
    }
    
}
