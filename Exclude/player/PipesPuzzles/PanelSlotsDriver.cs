using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSlotsDriver : MonoBehaviour
{

}

public class PanelSlot {
    
}

[CreateAssetMenu(fileName = "NewPanel", menuName = "Fractured Mind/Pipe Panel", order = 3)]
public class PanelPipe : ScriptableObject {
    public enum PanelType {StraightShape, LShape, CrossShape, Heart, Sizer}
    
    public Mesh panelMesh;
    
    public bool TwoByTwo;

    public PanelType type;

    [Range(1, 4)]
    public int panelRotation;

}