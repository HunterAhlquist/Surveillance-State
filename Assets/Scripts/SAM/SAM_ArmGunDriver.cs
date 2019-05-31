using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SAM_ArmGunDriver : MonoBehaviour {

    public bool active;
    
    public bool inRange;
    public float viewConeSize;
    public float viewDistance;
    public float rotSpeed;
    public LineRenderer targeting;
    public PlayerMain playerMain;
    public SAMMain SamMain;
    public GameObject targetOfInterest;


    public float shotDelay;
    private float curShotDelay;

    // Update is called once per frame
    void FixedUpdate() {
        if (SamMain.hadIntro && SamMain.curDetectionLevel == SAMMain.SAMState.Alert)
            LookCheck();
    }

    private void ShootLoop() {
        if (curShotDelay > 0) {
            curShotDelay -= Time.deltaTime;
        } else {
            playerMain.stamina -= 100 / 3;
            curShotDelay = shotDelay;
        }
    }
    
    private void LookOverTime(Vector3 lookVector, float speed) {
        Quaternion rot = Quaternion.LookRotation(lookVector);
        transform.rotation = Quaternion.Slerp(transform.transform.rotation, rot, speed * Time.deltaTime);
    }
    
    private void LookCheck() {
        Vector3 toTargetVector = targetOfInterest.transform.position - transform.position;
        if (inRange && active && targetOfInterest) {
            LookOverTime(toTargetVector, rotSpeed);
            targeting.SetPositions(new Vector3[] {transform.position, targetOfInterest.transform.position});
        } else {
            LookOverTime(transform.parent.forward, rotSpeed * 2);
            targeting.SetPositions(new Vector3[] {Vector3.zero, Vector3.zero});
        }
        
        //Debug.Log(Vector3.Angle(transform.forward, toTargetVector));
        Debug.DrawRay(transform.position, toTargetVector);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, toTargetVector, out hit, Mathf.Infinity, ~LayerMask.GetMask("SAM"))) {
            //Debug.Log(hit.collider.name + ", " + hit.collider.tag);
            if (Vector3.Angle(transform.parent.transform.forward, toTargetVector) < viewConeSize && hit.collider.tag == "Player") {
                if (toTargetVector.magnitude < viewDistance) {
                    inRange = true;
                    ShootLoop();
                }
                else {
                    inRange = false;
                    curShotDelay = shotDelay;
                }
            }
            else {
                inRange = false;
                curShotDelay = shotDelay;
            }
        }
        else {
            inRange = false;
            curShotDelay = shotDelay;
        }
    }
}
