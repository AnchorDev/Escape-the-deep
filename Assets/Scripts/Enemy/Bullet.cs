using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;
    public float lifetime = 3f;
    public float pushForce = 10f;

    private int direction = 1;

    void Start()
    {
        direction = transform.localScale.x > 0 ? 1 : -1;

        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * direction * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                Vector2 pushDirection = collision.transform.position - transform.position;

                playerRb.AddForce(pushDirection.normalized * pushForce, ForceMode2D.Impulse);
            }

            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
