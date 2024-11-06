using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public Transform playerTransform;
    public Timer timer;

    private float startTime;

    private Vector3 cameraSavePosition;


    void Start()
    {
        LoadGame();
    }
    public void SaveGame()
    {
        PlayerPrefs.SetFloat("PlayerPositionX", playerTransform.position.x);
        PlayerPrefs.SetFloat("PlayerPositionY", playerTransform.position.y);
        PlayerPrefs.SetFloat("PlayerPositionZ", playerTransform.position.z);

        PlayerPrefs.SetFloat("CameraPosX", cameraSavePosition.x);
        PlayerPrefs.SetFloat("CameraPosY", cameraSavePosition.y);
        PlayerPrefs.SetFloat("CameraPosZ", cameraSavePosition.z);

        timer.SaveElapsedTime();

        PlayerPrefs.Save();
        Debug.Log("Gra zapisana!");
    }

    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("PlayerPositionX"))
        {
            float playerPosX = PlayerPrefs.GetFloat("PlayerPositionX");
            float playerPosY = PlayerPrefs.GetFloat("PlayerPositionY");
            playerTransform.position = new Vector3(playerPosX, playerPosY, playerTransform.position.z);

            float cameraPosX = PlayerPrefs.GetFloat("CameraPosX");
            float cameraPosY = PlayerPrefs.GetFloat("CameraPosY");
            float cameraPosZ = PlayerPrefs.GetFloat("CameraPosZ");
            FindObjectOfType<CinemachineVirtualCamera>().transform.position = new Vector3(cameraPosX, cameraPosY, cameraPosZ);

            Debug.Log("Gra wczytana!");
        }
        else
        {
            Debug.LogWarning("Brak zapisanego stanu gry.");
        }
    }

    public void UpdateCameraSavePosition(Vector3 newCameraPosition)
    {
        cameraSavePosition = newCameraPosition;
    }
}
