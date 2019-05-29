using UnityEngine;
using System.Collections;

public class MouseDragRotate : MonoBehaviour {

    public int speed = 12;
    public float friction = 0.5f;
    public float lerpSpeed = 1.5f;
    float xDeg;
    float yDeg;
    Quaternion fromRotation;
    Quaternion toRotation;

    public Vector2 defaultRot;

    void Update() {
        if (Input.GetMouseButton(0)) {
            xDeg -= Input.GetAxis("Mouse X") * speed * friction;
            yDeg += Input.GetAxis("Mouse Y") * speed * friction;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        } else {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            xDeg = defaultRot.x;
            yDeg = defaultRot.y;
        }
        fromRotation = transform.rotation;
        toRotation = Quaternion.Euler(-yDeg, xDeg, 0);
        transform.rotation = Quaternion.Lerp(fromRotation, toRotation, Time.deltaTime * lerpSpeed);
    }
}