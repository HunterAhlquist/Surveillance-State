using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBarsDriver : MonoBehaviour {
    public Slider[] bars = new Slider[3];
    
    [Range(0, 1)] 
    public float totalStamina;


    private void Update() {
        UpdateBars();
    }

    public void RemoveBar() {
        
    }

    public void AddBar() {
        
    }

    public void UpdateBars() {
        float scaledStamina = totalStamina * bars.Length;
        float fillBarsTotal = scaledStamina;
            
        foreach (Slider curBar in bars) {
            if (fillBarsTotal > 1) {
                curBar.value = 1;
                fillBarsTotal -= 1;
            }
            else {
                curBar.value = fillBarsTotal;
                fillBarsTotal -= fillBarsTotal;
            }
            
        }
    }
}
