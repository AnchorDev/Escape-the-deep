using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Space(3)]
    [Header("Materials")]
    public PhysicsMaterial2D bounceMat;
    public PhysicsMaterial2D normalMat;
    public PhysicsMaterial2D slipperyMat;

    private Rigidbody2D rb;
    private GatherInput gI;

    private bool isOnSlipperySurface = false;
    private Vector3 originalScale;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gI = GetComponent<GatherInput>();

        originalScale = transform.localScale;
    }

    private void FixedUpdate()
    {
        Flip();
        PlayerJump();
        CheckStatus();
        PlayerMove();
    }

    private void PlayerMove()
    {
        if (!isOnSlipperySurface && jumpForce == 0.0f && grounded)
        {
            rb.velocity = new Vector2(speed * gI.valueX, rb.velocity.y);
        }
    }

    private void PlayerJump()
    {
        if (gI.jumpInput && grounded && !isOnSlipperySurface)
        {
            jumpForce += 0.45f;
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
            rb.sharedMaterial = bounceMat;
            preJump = true;
        }
        else
        {
            preJump = false;
        }

        if (gI.jumpInput && grounded && jumpForce >= 15.0f || gI.jumpInput == false && jumpForce >= 0.1f)
        {
            float tempX = gI.valueX * speed;
            float tempY = jumpForce;
            rb.velocity = new Vector2(tempX, tempY);
            Invoke("ResetJump", 0.025f);
        }

        if (rb.velocity.y <= -1)
        {
            rb.sharedMaterial = normalMat;
        }
    }

    private void ResetJump()
    {
        jumpForce = 0.0f;
    }

    private void CheckStatus()
    {
        RaycastHit2D leftCheckHit = Physics2D.Raycast(checkPointLeft.position, Vector2.down, rayLength, groundLayer);
        RaycastHit2D rightCheckHit = Physics2D.Raycast(checkPointRight.position, Vector2.down, rayLength, groundLayer);

        if (leftCheckHit || rightCheckHit)
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }

        SeeRays(leftCheckHit, rightCheckHit);
    }

    private void SeeRays(RaycastHit2D leftCheckHit, RaycastHit2D rightCheckHit)
    {
        Color color1 = leftCheckHit ? Color.green : Color.red;
        Color color2 = rightCheckHit ? Color.green : Color.red;

        Debug.DrawRay(checkPointLeft.position, Vector2.down * rayLength, color1);
        Debug.DrawRay(checkPointRight.position, Vector2.down * rayLength, color2);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Slippery"))
        {
            isOnSlipperySurface = true;
            rb.sharedMaterial = slipperyMat;
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
