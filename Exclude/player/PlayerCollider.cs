using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    public PlayerMain playerMain;
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "water") {
            playerMain.movement.underWater = true;
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.tag == "water") {
            playerMain.movement.underWater = true;
        }
    }

}
