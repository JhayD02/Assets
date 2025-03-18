using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerMovement : NetworkBehaviour
{
    private float sprintSpeed;
    public float jumpForce = 5f;
    public static bool isGrounded;
    private Rigidbody2D rb;
    private Vector3 originalPosition; // Variable to store the original position
    [SyncVar] private Vector3 lastCheckpointPosition; // Variable to store the last checkpoint position
    private Health health; // Reference to the Health script

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalPosition = transform.position; // Store the original position
        lastCheckpointPosition = originalPosition; // Initialize the last checkpoint position
        health = GetComponent<Health>(); // Automatically find the Health component
        SetCameraTarget();
    }

    void Update()
    {
        if (!isLocalPlayer) return;

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
            sprintSpeed = 0.02f;
        }
        else
        {
            sprintSpeed = 0.01f;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isLocalPlayer) return;

        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Player has touched the ground.");
            isGrounded = true;
        }
        else if (collision.gameObject.CompareTag("SafetyNet"))
        {
            Debug.Log("Player has collided with the safety net.");
            CmdTakeDamage(10); // Take damage when colliding with the safety net
            Respawn();
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Player has collided with an enemy.");
            CmdTakeDamage(10); // Take damage when colliding with an enemy
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (!isLocalPlayer) return;

        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Player has left the ground.");
            isGrounded = false;
        }
    }

    [Command]
    void CmdTakeDamage(float amount)
    {
        if (health != null)
        {
            health.TakeDamage(amount);
        }
    }

    void Respawn()
    {
        transform.position = lastCheckpointPosition; // Respawn the player to the last checkpoint position
    }

    [Command]
    public void CmdSetCheckpoint(Vector3 checkpointPosition)
    {
        lastCheckpointPosition = checkpointPosition; // Update the last checkpoint position on the server
        RpcUpdateCheckpoint(checkpointPosition); // Update the last checkpoint position on all clients
    }

    [ClientRpc]
    void RpcUpdateCheckpoint(Vector3 checkpointPosition)
    {
        lastCheckpointPosition = checkpointPosition; // Update the last checkpoint position on the client
    }

    public void SetCheckpoint(Vector3 checkpointPosition)
    {
        if (isLocalPlayer)
        {
            CmdSetCheckpoint(checkpointPosition); // Send the checkpoint update to the server
        }
    }

    private void SetCameraTarget()
    {
        if (!isLocalPlayer) return;

        CameraSript cameraScript = Camera.main.GetComponent<CameraSript>();
        if (cameraScript != null)
        {
            cameraScript.SetTarget(transform);
        }
    }
}