using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyYRot : MonoBehaviour {

    Camera cam;

    private void Start()
    {
        cam = transform.parent.Find("PlayerView").gameObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update () {
        transform.eulerAngles = new Vector3(0, cam.transform.localRotation.eulerAngles.y, 0);
	}
}
