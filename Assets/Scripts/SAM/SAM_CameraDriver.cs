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


    public SAMMain SamMain;
    public SAM_CameraArray CameraArray;
    
    // Start is called before the first frame update
    void Start() {
        if (transform.Find("Camera")) {
            cameraRot = transform.Find("Camera");
        } else if (transform.Find("ArmCamera")) {
            cameraRot = transform.Find("ArmCamera");
        }

        if (!isArmCamera) {
            initialCameraForward = cameraRot.forward;
        }
        
        CameraArray = GameObject.Find("Sam").transform.Find("Body").transform.Find("Cameras").GetComponent<SAM_CameraArray>();
        SamMain = GameObject.Find("Sam").GetComponent<SAMMain>();
        playerMain = GameObject.Find("Player").GetComponent<PlayerMain>();
        playerCam = GameObject.Find("PlayerView");

    }

    // Update is called once per frame
    void Update() {
        if (SamMain.isActive && SamMain.hadIntro)
            LookCheck();
        
    }

    private void LookCheck() {
        if (!targetOfInterest) {
            PlayerCheck();
            return;
        }
        Vector3 toTargetVector = targetOfInterest.transform.position - transform.position;
        if (inRange) {
            LookOverTime(toTargetVector, rotSpeed);
            if (SamMain.curDetectionLevel != SAMMain.SAMState.Alert && targetOfInterest.name == "Player" && !isArmCamera)
                SamMain.SawPlayer();
        } else {
            if (isArmCamera) {
                LookOverTime(transform.parent.forward, rotSpeed * 2);
            }
            else {
                LookOverTime(transform.forward, rotSpeed);
            }
            
        }
        
        //Debug.Log(Vector3.Angle(transform.forward, toTargetVector));
        RaycastHit hit;
        if (Physics.Raycast(transform.position, toTargetVector, out hit, Mathf.Infinity, ~LayerMask.GetMask("SAM", "Player"))) {
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

        PlayerCheck();
    }

    public void PlayerCheck() {
        Vector3 toTargetVector = playerMain.gameObject.transform.position - transform.position;
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position, toTargetVector, out hit, Mathf.Infinity, ~LayerMask.GetMask("SAM"))) {
            if (hit.collider.tag == "Player") {
                inRange = true;
                targetOfInterest = playerMain.gameObject;
            }
        }

    }
    
    private void LookOverTime(Vector3 lookVector, float speed) {
        Quaternion rot = Quaternion.LookRotation(lookVector);
        cameraRot.rotation = Quaternion.Slerp(cameraRot.transform.rotation, rot, speed * Time.deltaTime);
    }
    
}
