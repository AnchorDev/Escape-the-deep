using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindController : MonoBehaviour
{
    public Vector2 windForceLeft = new Vector2(-6f, 0f);
    public Vector2 windForceRight = new Vector2(6f, 0f);
    public float windDuration = 6f;
    public ParticleSystem windParticles;

    private Rigidbody2D playerRb;
    private bool playerInsideZone = false;
    private Vector2 currentWindForce = Vector2.zero;

    private ParticleSystem.VelocityOverLifetimeModule velocityModule;

    private void Start()
    {
        if (windParticles != null)
        {
            velocityModule = windParticles.velocityOverLifetime;
        }
        StartCoroutine(WindCycle());
    }

    private IEnumerator WindCycle()
    {
        while (true)
        {
            currentWindForce = windForceLeft;
            yield return StartCoroutine(AnimateParticles(-1));

            currentWindForce = windForceRight;
            yield return StartCoroutine(AnimateParticles(1));
        }
    }

    private IEnumerator AnimateParticles(float direction)
    {
        float halfDuration = windDuration / 2f;
        float elapsedTime = 0f;

        while (elapsedTime < halfDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / halfDuration;
            UpdateParticleDirection(direction, t);
            yield return null;
        }

        elapsedTime = 0f;
        while (elapsedTime < halfDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = 1f - (elapsedTime / halfDuration);
            UpdateParticleDirection(direction, t);
            yield return null;
        }

        windParticles.Stop();
    }

    private void UpdateParticleDirection(float direction, float intensity)
    {
        if (windParticles != null)
        {
            windParticles.Play();
            velocityModule.x = 5f * direction * intensity;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerRb = other.GetComponent<Rigidbody2D>();
            playerInsideZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerRb != null && other.GetComponent<Rigidbody2D>() == playerRb)
            {
                playerRb = null;
                playerInsideZone = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (playerInsideZone && playerRb != null && !IsGrounded())
        {
            playerRb.AddForce(currentWindForce);
        }
    }

    private bool IsGrounded()
    {
        PlayerController playerController = playerRb.GetComponent<PlayerController>();
        return playerController != null && playerController.grounded;
    }
}
