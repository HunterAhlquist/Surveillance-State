using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SAM_ArmGunDriver : MonoBehaviour {

    public bool inRange;
    public float viewConeSize;
    public float viewDistance;
    public float rotSpeed;
    
    public GameObject targetOfInterest;
    

    // Update is called once per frame
    void FixedUpdate() {
        LookCheck();
    }
    
    private void LookOverTime(Vector3 lookVector, float speed) {
        Quaternion rot = Quaternion.LookRotation(lookVector);
        transform.rotation = Quaternion.Slerp(transform.transform.rotation, rot, speed * Time.deltaTime);
    }
    
    private void LookCheck() {
        Vector3 toTargetVector = targetOfInterest.transform.position - transform.position;
        if (inRange) {
            LookOverTime(toTargetVector, rotSpeed);
        } else {
            LookOverTime(transform.parent.forward, rotSpeed * 2);
        }
        
        //Debug.Log(Vector3.Angle(transform.forward, toTargetVector));
        RaycastHit hit;
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
}
