using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMenager : MonoBehaviour
{
    public void PlayButton()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void ExitButton()
    {
        Application.Quit();
        Debug.Log("Exit");
    }
}
