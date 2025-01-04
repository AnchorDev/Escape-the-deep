using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class VictoryTrigger : MonoBehaviour
{
    public GameObject victoryUI;
    public TextMeshProUGUI victoryText;
    public Image fadeImage;
    public Timer timer;

    private bool gameEnded = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !gameEnded)
        {
            gameEnded = true;
            StartVictorySequence();
        }
    }

    private void StartVictorySequence()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.velocity = Vector2.zero;
                playerRb.bodyType = RigidbodyType2D.Static;
            }
        }

        PlayerPrefs.DeleteAll();

        victoryUI.SetActive(true);

        string finalTime = timer.GetFormattedTime();
        victoryText.text = $"Thanks for playing!\nYour time is {finalTime}\n\npress SPACE to return to the menu";

        StartCoroutine(FadeToBlack());
    }

    private IEnumerator FadeToBlack()
    {
        float fadeDuration = 2f;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }

    void Update()
    {
        if (gameEnded && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(0);
        }
    }
}
