using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    public Transform nextCameraPositionUp;
    public Transform nextCameraPositionDown;
    private CinemachineVirtualCamera virtualCamera;
    private Rigidbody2D playerRb;

    private bool playerInsideTrigger = false;

    void Start()
    {
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerRb == null) playerRb = other.GetComponent<Rigidbody2D>();

            if (playerRb != null)
            {
                playerInsideTrigger = true;
                UpdateCameraPosition();
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideTrigger = false;
        }
    }

    void Update()
    {
        if (playerInsideTrigger && playerRb != null)
        {
            UpdateCameraPosition();
        }
    }

    private void UpdateCameraPosition()
    {
        if (playerRb.velocity.y > 0)
        {
            SwitchCameraPosition(nextCameraPositionUp);
        }
        else if (playerRb.velocity.y < 0)
        {
            SwitchCameraPosition(nextCameraPositionDown);
        }
    }

    private void SwitchCameraPosition(Transform newCameraPosition)
    {
        if (newCameraPosition != null)
        {
            virtualCamera.transform.position = new Vector3(
                newCameraPosition.position.x,
                newCameraPosition.position.y,
                virtualCamera.transform.position.z
            );
        }
    }
}
