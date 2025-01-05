using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Clips")]
    public AudioClip jumpSound;
    public AudioClip wallBounceSound;
    public AudioClip normalFallSound;
    public AudioClip finalFallSound;

    private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("AudioSource is not assigned to the AudioManager!");
        }
    }

    public void PlaySound(string soundName)
    {
        if (audioSource == null) return;

        AudioClip clipToPlay = null;

        switch (soundName)
        {
            case "Jump":
                clipToPlay = jumpSound;
                break;

            case "WallBounce":
                clipToPlay = wallBounceSound;
                break;

            case "NormalFall":
                clipToPlay = normalFallSound;
                break;
            case "FianlFall":
                clipToPlay = finalFallSound;
                break;

            default:
                Debug.LogWarning("Undefined sound: " + soundName);
                break;
        }

        if (clipToPlay != null)
        {
            audioSource.PlayOneShot(clipToPlay);
        }
    }
}

