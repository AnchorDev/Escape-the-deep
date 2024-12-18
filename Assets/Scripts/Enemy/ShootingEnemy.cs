using System.Collections;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour
{
    [Header("Shooting Settings")]
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float shootInterval = 2f;
    private bool IsShooting = false;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        StartCoroutine(ShootRoutine());
    }

    IEnumerator ShootRoutine()
    {
        while (true)
        {
            if (animator != null)
            {
                animator.SetBool("IsShooting", true);
                IsShooting = true;
            }

            yield return new WaitForSeconds(0.1f);

            Shoot();

            if (animator != null) {
            animator.SetBool("IsShooting", false);
                IsShooting = false;
            }

            yield return new WaitForSeconds(shootInterval);
        }
    }

    void Shoot()
    {
        if (bulletPrefab != null && shootPoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);

            int direction = spriteRenderer.flipX ? -1 : 1;

            bullet.transform.localScale = new Vector3(direction, 1, 1);
        }
    }
}