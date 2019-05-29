using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeballMotion : MonoBehaviour {

    public float viewConeSize;
    public float viewDistance;

    public bool inRange;

    public Transform frontTransform;

    Vector3 originalForward;

    GameObject playerObject;
    // Start is called before the first frame update
    void Awake() {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        originalForward = transform.forward;
    }

    // Update is called once per frame
    void Update() {
        Vector3 toTargetVector = playerObject.transform.position - transform.position;
        if (inRange) {
            LookOverTime(toTargetVector, viewDistance);
        } else {
            LookOverTime(originalForward, viewDistance);
        }
        
        //Debug.Log(Vector3.Angle(frontTransform.forward, toTargetVector));
        if (Vector3.Angle(frontTransform.forward, toTargetVector) < viewConeSize) {
            if (toTargetVector.magnitude < viewDistance) {
                inRange = true;
                Debug.DrawRay(transform.position, toTargetVector, Color.green);
                
            } else {
                inRange = false;
            }
        } else {
            inRange = false;
        }
    }

    private void EyelidProcess(bool topLid) {
        if (topLid) {

        } else {

        }
    }

    private void LookOverTime(Vector3 lookAtMe, float speed) {
        Quaternion rot = Quaternion.LookRotation(lookAtMe);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, speed * Time.deltaTime);
    }
}
