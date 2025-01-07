using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Button loadGameButton;
    private SaveManager saveManager;
    private ExitHandler exitHandler;

    private void Start()
    {
        saveManager = FindObjectOfType<SaveManager>();
        exitHandler = FindObjectOfType<ExitHandler>();

        if (PlayerPrefs.HasKey("PlayerPositionX"))
        {
            loadGameButton.gameObject.SetActive(true);
        }
        else
        {
            loadGameButton.gameObject.SetActive(false);
        }
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

        var saveManager = FindObjectOfType<SaveManager>();
        if (saveManager != null)
        {
            saveManager.UpdateCameraSavePosition(camera.transform.position);
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
