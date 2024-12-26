using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    private SaveManager saveManager;
    private ExitHandler exitHandler;
    private void Start()
    {
        saveManager = FindObjectOfType<SaveManager>();
        exitHandler = FindObjectOfType<ExitHandler>();
    }
    public void NewGame()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadSceneAsync(1);
        StartCoroutine(InitializeNewGame());
    }

    private IEnumerator InitializeNewGame()
    {
        yield return new WaitForSeconds(0.1f);
        var player = GameObject.FindWithTag("Player");
        var camera = FindObjectOfType<CinemachineVirtualCamera>();

        if (player != null)
        {
            player.transform.position = new Vector3(1.88f, 0f, 0f);
        }

        if (camera != null)
        {
            camera.transform.position = new Vector3(0f, 4.5f, -10f);
        }
    }

    public void LoadGame()
    {
        StartCoroutine(LoadGameWithSave());
    }

    private IEnumerator LoadGameWithSave()
    {
        SceneManager.LoadSceneAsync(1);
        yield return new WaitForSeconds(0.1f);
        saveManager = FindObjectOfType<SaveManager>();
        if (saveManager != null)
        {
            saveManager.LoadGame();
        }
        else
        {
            Debug.LogWarning("SaveManager not found in loaded scene.");
        }
    }

    public void ExitButton()
    {
        exitHandler.HandleExit();
    }
}
