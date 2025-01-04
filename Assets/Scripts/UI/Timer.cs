using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    float elapsedTime;
    private float savedTime;

    void Start()
    {
        if (PlayerPrefs.HasKey("ElapsedTime"))
        {
            savedTime = PlayerPrefs.GetFloat("ElapsedTime");
            elapsedTime = savedTime;
        }
        else
        {
            savedTime = 0f;
            elapsedTime = 0f;
        }
    }


    void Update()
    {
        elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

    }

    public void SaveElapsedTime()
    {
        PlayerPrefs.SetFloat("ElapsedTime", elapsedTime);
    }

    public string GetFormattedTime()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
