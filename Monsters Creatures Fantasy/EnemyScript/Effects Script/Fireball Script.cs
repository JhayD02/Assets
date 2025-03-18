using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class FireballScript : NetworkBehaviour
{
    public float speed = 7.5f;
    public float life = 5f;
    private Vector3 targetPosition;
    public float damage = 10f;

    public void SetTargetPosition(Vector3 position)
    {
        targetPosition = position;
    }

    void Update()
    {
        if (!isServer) return;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (transform.position == targetPosition)
        {
            NetworkServer.Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Health playerHealth = collision.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log("Player health after fireball hit: " + playerHealth.CurrentHealth);
            }
            else
            {
                Debug.LogError("Player does not have a Health component: " + collision.name);
            }
            NetworkServer.Destroy(gameObject); // Destroy the fireball after hitting the player
        }
    }

    [ServerCallback]
    private IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        NetworkServer.Destroy(gameObject);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        StartCoroutine(DestroyAfterTime(life));
    }
}