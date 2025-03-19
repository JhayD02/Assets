using UnityEngine;

public class Chest : MonoBehaviour
{
    private static string firstPlayer = ""; // Stores who picked the first chest
    private static int chestsCollected = 0;
    private static int totalChests = 2;

    public static bool canWin = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Ensure only players collect
        {
            if (firstPlayer == "") 
            {
                firstPlayer = other.gameObject.name; // Set first player
                Debug.Log(firstPlayer + " collected the first chest!");
            }
            else if (other.gameObject.name != firstPlayer) 
            {
                Debug.Log(other.gameObject.name + " collected the second chest!");
            }
            else
            {
                Debug.Log(other.gameObject.name + " cannot collect both chests!");
                return; // Prevent same player from collecting both
            }

            chestsCollected++;
            Destroy(gameObject);

            if (chestsCollected >= totalChests)
            {
                canWin = true;
                Debug.Log("Both chests collected! Proceeding...");
                // Add logic here to open a door, trigger an event, etc.
            }
        }
    }
}