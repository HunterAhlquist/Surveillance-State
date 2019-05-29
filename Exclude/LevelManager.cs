using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    public PlayerProg progress;

    

    private void OnEnable() {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }
    private void OnDisable() {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }
    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
        ApplyEntityData();
        OnDisable();
    }

    private void Awake() {
        if (GameObject.FindGameObjectsWithTag("Dungeon Master").Length > 1)
            Destroy(this.gameObject);
        progress = GetComponent<PlayerProg>();
        DontDestroyOnLoad(this.gameObject);
    }

    

    public void LoadLevel(string sceneName, bool destroyDM = false) {
        Debug.Log(SceneManager.GetSceneByName(sceneName).IsValid());
        if (!SceneManager.GetSceneByName(sceneName).IsValid()) {
            Debug.Log("scene does not exist");
            SceneManager.LoadScene("null", LoadSceneMode.Single);
            Destroy(GameObject.Find("Dungeon Master"));
            return;
        }

        HoldEntityData();
        OnEnable();
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        if (destroyDM)
            Destroy(GameObject.Find("Dungeon Master"));
    }

    public void ReturnToMenu() {
        SceneManager.LoadScene("main_menu", LoadSceneMode.Single);
        Destroy(GameObject.Find("Dungeon Master"));
    }

    public void WakeUp() {
        HoldEntityData();
        OnEnable();
    }

    public void HoldEntityData() { //get all entities from the current level and put their headers into the playerprog
        string currentScene = SceneManager.GetActiveScene().name;
        LevelData new_levelData = new LevelData(currentScene); //this variable is used to replace the old level data with the new
        GameObject[] entities = GameObject.FindGameObjectsWithTag("Entity");//put all objects with the entity tag into a temp var
        if (entities.Length == 0)
            return;

        for (int i = 0; i < entities.Length; i++) {//put entities into a new level data variable
            new_levelData.entityData.Add(entities[i].GetComponent<EntityHeader>().entity);
        }

        //find the index of our current level and store it
        //if it is not found then add it to the list

        if (progress.levelData.Count == 0) { //check if PlayerProg levelData is empty
            progress.levelData.Add(new_levelData); //populate it with its first entry
            return; //end the function because all the level's data is now stored
        } else { //if PlayerProg already has levelData...
            LevelData[] levelDataClone = progress.levelData.ToArray(); //clone PlayerProg to an array
            for (int i = 0; i < levelDataClone.Length; i++) { //loop through all the entries in PlayerProg to find the current scene's entry
                if (levelDataClone[i].sceneName == currentScene) {//if the entry is found...
                    progress.levelData[i] = new_levelData;
                    return;
                }
            }
            progress.levelData.Add(new_levelData); //create an an entry for the level
            return; //level data is stored
        }
    }

    public void ApplyEntityData() {
        string currentScene = SceneManager.GetActiveScene().name;
        GameObject[] entities = GameObject.FindGameObjectsWithTag("Entity");
        if (entities.Length == 0)
            return;

        if (progress.levelData.Count == 0) {
            return; //this means that there is no level data making this method not needed
        } else {
            LevelData[] levelDataClone = progress.levelData.ToArray();
            for (int i = 0; i < levelDataClone.Length; i++) {
                if (levelDataClone[i].sceneName == currentScene) {
                    for (int j = 0; j < entities.Length; j++) {
                        entities[j].GetComponent<EntityHeader>().entity = progress.levelData[i].entityData[j];
                    }
                }
            }
            return;
        }
        
    }
}

[System.Serializable]
public class LevelData {
    public string sceneName;
    public List<Entity> entityData;

    public LevelData(string new_sceneName) {
        sceneName = new_sceneName;
        entityData = new List<Entity>();
    }
}
