using UnityEngine;

public class Torch : MonoBehaviour
{
    private static string firstPlayer = ""; // Stores who picked the first chest
    private static int torchCollected = 0;
    private static int totalChests = 2;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Ensure only players collect
        {
            if (firstPlayer == "") 
            {
                firstPlayer = other.gameObject.name; // Set first player
                Debug.Log(firstPlayer + " collected the first torch!");
            }
            else if (other.gameObject.name != firstPlayer) 
            {
                Debug.Log(other.gameObject.name + " collected the second torch!");
            }
            else
            {
                Debug.Log(other.gameObject.name + " cannot collect both torch!");
                return; // Prevent same player from collecting both
            }

            torchCollected++;
            Destroy(gameObject);

            if (torchCollected >= totalChests)
            {
                Debug.Log("Both torchs collected! Proceeding...");
                // Add logic here to open a door, trigger an event, etc.
            }
        }
    }
}