﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Player values")]
    public float speed = 2;
    public float jumpForce = 0.0f;
    public bool preJump = false;
    private int direction = 1;

    [Space(3)]
    [Header("GroundCheck")]
    public float rayLength = 0.6f;
    public bool grounded;
    public LayerMask groundLayer;
    public Transform checkPointLeft;
    public Transform checkPointRight;
    public Transform checkPointMiddle;

    [Space(3)]
    [Header("Materials")]
    public PhysicsMaterial2D bounceMat;
    public PhysicsMaterial2D normalMat;
    public PhysicsMaterial2D slipperyMat;

    [Space(3)]
    [Header("Sprites")]
    public Sprite idleSprite;
    public Sprite chargingSprite;
    public Sprite jumpingSprite;
    public Sprite bouncingSprite;
    public Sprite slidingSprite;

    public Sprite[] runningSprites;

    public Rigidbody2D rb;
    private GatherInput gI;

    public SpriteRenderer spriteRenderer;
    public bool isOnSlipperySurface = false;
    private Vector3 originalScale;

    private float resetTime = 2f;
    private float holdCounter = 0;

    public float jumpStartHeight;
    private bool hasPlayedFallSound = false;
    private bool hasPlayedJumpSound = false;
    private bool hasPlayedNormalFallSound = false;

    private int runningFrameIndex = 0;
    private float runningFrameTime = 0.1f;
    private float runningTimer = 0f;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gI = GetComponent<GatherInput>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        originalScale = transform.localScale;
    }

    public void FixedUpdate()
    {
        Flip();
        CheckStatus();
        UpdateSpriteState();
        PlayerMove();
        PlayerJump();
        CheckFall();
        RestartGame();
    }

    private void UpdateSpriteState()
    {
        if (!grounded)
        {
            if (rb.velocity.y > 0)
            {
                spriteRenderer.sprite = jumpingSprite;
            }
            else
            {
                spriteRenderer.sprite = slidingSprite;
            }
        }
        else if (isOnSlipperySurface)
        {
            spriteRenderer.sprite = slidingSprite;
        }
        else if (rb.velocity.x != 0)
        {
            AnimateRunning();
        }
        else
        {
            spriteRenderer.sprite = idleSprite;
            runningTimer = 0f;
        }
    }

    private void RestartGame()
    {
        if (Input.GetKey(KeyCode.R))
        {
            holdCounter += Time.deltaTime;

            if (holdCounter >= resetTime)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                holdCounter = 0f;
            }
        }
        else
        {
            holdCounter = 0f;
        }
    }

    public void PlayerMove()
    {
        if (!isOnSlipperySurface && jumpForce == 0.0f && grounded)
        {
            rb.velocity = new Vector2(speed * gI.valueX, rb.velocity.y);
        }
    }


    private void AnimateRunning()
    {
        runningTimer += Time.deltaTime;
        if (runningTimer >= runningFrameTime)
        {
            runningFrameIndex = (runningFrameIndex + 1) % runningSprites.Length;
            spriteRenderer.sprite = runningSprites[runningFrameIndex];
            runningTimer = 0f;
        }
    }

    public void PlayerJump()
    {
        if (gI.jumpInput && grounded && !isOnSlipperySurface)
        {
            jumpStartHeight = transform.position.y;

            jumpForce += 0.45f;
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
            rb.sharedMaterial = bounceMat;
            preJump = true;

            spriteRenderer.sprite = chargingSprite;
        }
        else if (!grounded)
        {
            ResetJump();
        }

        if (gI.jumpInput && grounded && jumpForce >= 15.0f || gI.jumpInput == false && jumpForce >= 0.1f)
        {
            float tempX = gI.valueX * speed;
            float tempY = jumpForce;
            rb.velocity = new Vector2(tempX, tempY);
            Invoke("ResetJump", 0.025f);


            if (!hasPlayedJumpSound)
            {
                AudioManager.instance.PlaySound("Jump");
                hasPlayedJumpSound = true;
            }
        }

        if (rb.velocity.y <= -1)
        {
            rb.sharedMaterial = normalMat;
        }
    }

    public void ResetJump()
    {
        jumpForce = 0.0f;
        hasPlayedJumpSound = false;
    }

    private void CheckStatus()
    {
        RaycastHit2D leftCheckHit = Physics2D.Raycast(checkPointLeft.position, Vector2.down, rayLength, groundLayer);
        RaycastHit2D rightCheckHit = Physics2D.Raycast(checkPointRight.position, Vector2.down, rayLength, groundLayer);
        RaycastHit2D centerCheckHit = Physics2D.Raycast(checkPointMiddle.position, Vector2.down, rayLength, groundLayer);

        if (leftCheckHit || rightCheckHit || centerCheckHit)
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }

        SeeRays(leftCheckHit, rightCheckHit, centerCheckHit);
    }

    private void SeeRays(RaycastHit2D leftCheckHit, RaycastHit2D rightCheckHit, RaycastHit2D centerCheckHit)
    {
        Color color1 = leftCheckHit ? Color.green : Color.red;
        Color color2 = rightCheckHit ? Color.green : Color.red;
        Color color3 = centerCheckHit ? Color.green : Color.red;

        Debug.DrawRay(checkPointLeft.position, Vector2.down * rayLength, color1);
        Debug.DrawRay(checkPointRight.position, Vector2.down * rayLength, color2);
        Debug.DrawRay(checkPointMiddle.position, Vector2.down * rayLength, color3);
    }

    private void Flip()
    {
        if (grounded && !isOnSlipperySurface && gI.valueX * direction < 0)
        {
            direction *= -1;
            float rotationY = direction == 1 ? 0f : 180f;
            transform.rotation = Quaternion.Euler(0f, rotationY, 0f);
        }
    }

    private void CheckFall()
    {
        if (grounded)
        {
            if (transform.position.y < jumpStartHeight - 7.0f)
            {
                if (!hasPlayedFallSound)
                {
                    AudioManager.instance.PlaySound("FianlFall");
                    hasPlayedFallSound = true;
                }
            }
            else if (!hasPlayedNormalFallSound)
            {
                AudioManager.instance.PlaySound("NormalFall");
                hasPlayedNormalFallSound = true;
            }

            jumpStartHeight = transform.position.y;
        }
        else
        {
            hasPlayedFallSound = false;
            hasPlayedNormalFallSound = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Slippery"))
        {
            isOnSlipperySurface = true;
            rb.sharedMaterial = slipperyMat;
            spriteRenderer.sprite = slidingSprite;
        }
        else
        {
            if (collision.contacts[0].normal.x != 0)
            {
                AudioManager.instance.PlaySound("WallBounce");
                spriteRenderer.sprite = bouncingSprite;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Slippery"))
        {
            isOnSlipperySurface = false;
            rb.sharedMaterial = normalMat;
        }
    }
}
