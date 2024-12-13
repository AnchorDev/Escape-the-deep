﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2.0f;

    private Vector3 targetPosition;
    private bool movingToPointB = true;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    void Start()
    {
        targetPosition = pointB.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (spriteRenderer == null)
        {
            Debug.LogWarning("SpriteRenderer not found! Please attach a SpriteRenderer component to the platform.");
        }
        if (animator == null)
        {
            Debug.LogWarning("Animator not found! Please attach an Animator component to the platform.");
        }
    }

    void Update()
    {
        MovePlatform();
        UpdateSpriteDirection();
    }

    void MovePlatform()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            if (movingToPointB)
            {
                targetPosition = pointA.position;
                movingToPointB = false;
            }
            else
            {
                targetPosition = pointB.position;
                movingToPointB = true;
            }
        }
    }

    void UpdateSpriteDirection()
    {
        if (spriteRenderer != null)
        {
            if (targetPosition.x > transform.position.x)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(this.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log($"OnCollisionExit2D: {collision.gameObject.name} leaving {this.gameObject.name}");

        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.transform.parent == this.transform)
            {
                collision.transform.SetParent(null);
                Debug.Log("Parent set to null");
            }
            else
            {
                Debug.LogWarning("Player was not parented to platform");
            }
        }
    }
}
