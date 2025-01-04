using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    private SaveManager saveManager;
    private ExitHandler exitHandler;

    private void Start()
    {
        saveManager = FindObjectOfType<SaveManager>();
        exitHandler = FindObjectOfType<ExitHandler>();
    }

    private void Update()
    {
        //if (Input.GetKeyUp(KeyCode.P))
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (pauseMenuPanel.activeSelf)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void PauseGame()
    {
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        saveManager.SaveGame();
    }

    public void MenuButton()
    {
        StartCoroutine(SaveAndReturnToMenu());
    }

    public void QuitButton()
    {
        exitHandler.HandleExit();
    }
    private IEnumerator SaveAndReturnToMenu()
    {
        saveManager.SaveGame();
        yield return new WaitForEndOfFrame();
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

}
