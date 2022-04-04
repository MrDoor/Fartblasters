using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    public string firstLevel = "World_1-1";

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        PlayerPrefs.SetInt("died", 0);
        Time.timeScale = 1;
        // if (DBFunctions.getLevelProgress() != -1)
        // {
        //     // Application.LoadLevel(DBFunctions.getLevelProgress());
        //     // SceneManager.LoadScene(DBFunctions.getLevelProgress());
        //     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        // }
        // else
        // {
        //     Debug.Log("Error retreiving level from the database");
        //     // Application.LoadLevel(0);
        //     SceneManager.LoadScene(0);
        // }
    }

    public void ReloadLevel()
    {
        // Application.LoadLevel(Application.loadedLevel);
        SceneManager.LoadScene(Application.loadedLevel);

        PlayerPrefs.SetInt("died", 0);
        Time.timeScale = 1;
    }

    public void StartNewGame()
    {
        if (firstLevel != "")
        {
            // Application.LoadLevel(firstLevel);
            // New Scene Management Way
            SceneManager.LoadScene(firstLevel);

            PlayerPrefs.SetInt("died", 0);
            Time.timeScale = 1;
        }
    }

    public void QuitGame()
    {
        var scene = SceneManager.GetActiveScene();
        if (scene.name != Constants.MAIN_MENU)
        {
            SceneManager.LoadScene(Constants.MAIN_MENU);
        }

        Application.Quit();
    }
}
