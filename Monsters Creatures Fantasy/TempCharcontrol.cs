using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempCharcontrol : MonoBehaviour
{
    private float sprintSpeed;
    public float jumpForce = 5f;
    public static bool isGrounded;
    int damage = 5;
    private Rigidbody2D rb;
    private Vector3 originalPosition;

    // New fields for attack
    public float attackRadius = 1f;
    public Transform attackPoint;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        if (horizontalInput < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (horizontalInput > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        if (horizontalInput != 0)
        {
            transform.Translate(0.01f + sprintSpeed, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            sprintSpeed = 0.05f;
        }
        else
        {
            sprintSpeed = 0.01f;
        }

        // Check for attack input
        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        }
    }

    void Attack()
    {
        if (attackPoint == null)
        {
            Debug.LogError("Attack point is not set.");
            return;
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                Debug.Log("Hit " + enemy.name);
                EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damage);
                }
                else
                {
                    Debug.LogError("Enemy does not have an EnemyHealth component: " + enemy.name);
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Player has touched the ground.");
            isGrounded = true;
        }
        else if (collision.gameObject.CompareTag("SafetyNet"))
        {
            Debug.Log("Player has collided with the safety net.");
            Respawn();
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Player has left the ground.");
            isGrounded = false;
        }
    }

    void Respawn()
    {
        transform.position = originalPosition; // Respawn the player to the original position
    }

    // Draw the attack radius in the editor
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}