using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuMenager : MonoBehaviour
{
    public GameObject pauseMenuPanel;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (pauseMenuPanel.activeSelf)
            {
                pauseMenuPanel.SetActive(false);
                Time.timeScale = 1f;
            }
            else
            {
                pauseMenuPanel.SetActive(true);
                Time.timeScale = 0f;
            }
        }
    }

    public void MenuButton()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitButton()
    {
        Application.Quit();
        Debug.Log("Quit");
    }


}
