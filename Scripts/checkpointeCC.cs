using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpointeCC : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.SetCheckpoint(transform.position);
                Debug.Log("Checkpoint reached at position: " + transform.position);
            }
        }
    }
}
