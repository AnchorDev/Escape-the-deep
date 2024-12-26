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

#if UNITY_EDITOR
        Debug.Log("Application.Quit() symulowane w edytorze Unity.");
#else
    Application.Quit();
#endif
    }

}
