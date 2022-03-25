using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHandler : MonoBehaviour
{
    public string firstLevel = "World_1-1";

    public void NextLevel()
    {
        if (DBFunctions.getLevelProgress() != -1)
        {
            Application.LoadLevel(DBFunctions.getLevelProgress());
        }
        else
        {
            Debug.Log("Error retreiving level from the database");
            Application.LoadLevel(0);
        }
    }

    public void ReloadLevel()
    {
        Application.LoadLevel(Application.loadedLevel);
        Time.timeScale = 1;
    }

    public void StartNewGame()
    {
        if (firstLevel != "")
        {
            Application.LoadLevel(firstLevel);
            Time.timeScale = 1;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
