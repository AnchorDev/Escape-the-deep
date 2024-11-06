using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitHandler : MonoBehaviour
{
    private SaveManager saveManager;

    void Start()
    {
        saveManager = FindObjectOfType<SaveManager>();
    }

    public void HandleExit()
    {
        if (saveManager != null)
        {
            saveManager.SaveGame();
        }

        Application.Quit();
        Debug.Log("Game exited");
    }
}
