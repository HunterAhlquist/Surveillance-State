using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public PlayerInput input;
    // Update is called once per frame
    void Update()
    {
        if (input.UI_Map) {
            SceneManager.LoadScene("Level", LoadSceneMode.Single);
        }
        if (input.UI_pause) {
            Debug.Log("quit");
            Application.Quit();
        }
    }
}
