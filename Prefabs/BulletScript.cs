using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed = 5f;
    public float lifetime = 4f;
    private Rigidbody2D rb;
    public int damage = 10;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with an enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Check if the enemy is a mushroom
            if (collision.gameObject.GetComponent<MushroomBehavior>() != null)
            {
                Debug.Log("Bullet hit a mushroom enemy but did not deal damage.");
                return; // Do not deal damage to the mushroom enemy
            }

            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
                Debug.Log("Bullet hit " + collision.gameObject.name + " and dealt " + damage + " damage.");
            }
            else
            {
                Debug.LogError("Enemy does not have an EnemyHealth component: " + collision.gameObject.name);
            }

            Destroy(gameObject); // Destroy the bullet after hitting an enemy
        }
    }
}