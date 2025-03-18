using UnityEngine;

public class Ice : MonoBehaviour
{
    public GameObject lava; // Assign the lava GameObject in the Inspector

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Ensure only the player can pick up the ice
        {
            Debug.Log("Ice collected! Lava disappears.");
            lava.SetActive(false); // Disable the lava
            Destroy(gameObject); // Remove the ice after pickup
        }
    }
}
