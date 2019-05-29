using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerUI : MonoBehaviour {
    public StaminaBarsDriver stamina;

    public void UpdateStaminaBars(float newValue) {
        stamina.totalStamina = newValue / 100;
        stamina.UpdateBars();
    }
}
