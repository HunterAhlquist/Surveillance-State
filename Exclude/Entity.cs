using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Entity { //This class holds varibles and methods used for defining an entity in game
    public enum Type { NPC, EventTrigger, Transition, Item, TPEye, None };

    [Header("Entity Header")]
    public string entityName;
    public Type entityType;

    public List<EntityFlag> flagData;

    public Entity(List<EntityFlag> flags) {
        flagData = flags;
    }

    public EntityFlag FindFlag(string flagName) {
        for (int i = 0; i < flagData.Count; i++) {
            if (flagData[i].name == flagName) {
                return flagData[i];
            }
        }
        return null;
    }

    public bool GetFlagValue(string flagName) {
        for (int i = 0; i < flagData.Count; i++) {
            if (flagData[i].name == flagName) {
                return flagData[i].state;
            }
        }
        return false;
    }

    public void SetFlag(string flagName, bool value) {//sets or creates a new flag with a name
        EntityFlag newFlag = new EntityFlag(flagName, value);
        for (int i = 0; i < flagData.Count; i++) {
            if (flagData[i].name == newFlag.name) {
                flagData[i] = newFlag;
                return;
            }
        }
        flagData.Add(newFlag);
        return;
    }
}

[System.Serializable]
public class EntityFlag { //This is an object for holding a boolean flag along with a name for what they're for and identification.
    public string name;
    public bool state;

    public EntityFlag(string new_name, bool new_state) {
        name = new_name;
        state = new_state;
    }
}
