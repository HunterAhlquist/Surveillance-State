using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateDriver : MonoBehaviour {
    public Entity button;

    public float gateTime;
    private float curGateTime;

    public float targetLift = 9.25f;

    public AudioSource aud;

    private void Start() {
        curGateTime = gateTime;
        aud = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (button.activate) {
            if (curGateTime > 0) {
                if (!aud.isPlaying) {
                    aud.Play();
                }
            } else {
                aud.Stop();
            }
                
            transform.localPosition = new Vector3(transform.localPosition.x, 
                                                Mathf.Lerp(targetLift, 0, Remap(curGateTime, 0, gateTime, 0, 1)), 
                                                    transform.localPosition.z);
            if (curGateTime > 0)
                curGateTime -= Time.deltaTime;
        }
    }
    
    public static float Remap(float value, float oldMin, float oldMax, float newMin, float newMax) {
        return (value - oldMin) / (oldMax - oldMin) * (newMax - newMin) + newMin;
    }
}
