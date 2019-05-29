using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Environment Soundpack", menuName = "Fractured Mind/Environment Sound Pack", order = 1)]
public class EnvSoundsPacks : ScriptableObject {
    public AudioClip[] footsteps;
    public AudioClip[] landings;
    public PhysicMaterial type;
}
