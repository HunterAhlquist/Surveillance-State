﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVAnimator : MonoBehaviour {
    public MeshRenderer renderer;

    public Texture[] textures;
    public Texture warning;
    private int texIndex;

    public SAMMain samMain;

    public bool active;
    public float speed;

    private float countdown = 2;

    private void Start() {
        samMain = GameObject.Find("Sam").GetComponent<SAMMain>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (samMain.isActive && !active)
            active = true;

        if (active) {
            renderer.material.SetTexture("_BaseMap", warning);
            renderer.material.SetTexture("_EmissionMap", warning);
        } else {
            if (countdown > 0) {
                countdown -= Time.deltaTime * speed;
            } else {
                if (texIndex + 1 > textures.Length - 1) {
                    texIndex = 0;
                    renderer.material.SetTexture("_BaseMap", textures[texIndex]);
                    renderer.material.SetTexture("_EmissionMap", textures[texIndex]);
                    countdown = 2;
                } else {
                    texIndex++;
                    renderer.material.SetTexture("_BaseMap", textures[texIndex]);
                    renderer.material.SetTexture("_EmissionMap", textures[texIndex]);
                    countdown = 2;
                }
            }
        }
        
    }
}
