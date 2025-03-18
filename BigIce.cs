using UnityEngine;

public class BigIce : MonoBehaviour
{
    public GameObject[] lavas; // Assign multiple lava GameObjects in the Inspector

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Ensure only the player can pick up the ice
        {
            Debug.Log("Big Ice collected! All lavas disappear.");
            
            foreach (GameObject lava in lavas)
            {
                if (lava != null)
                {
                    lava.SetActive(false); // Disable each lava
                }
            }
            
            Destroy(gameObject); // Remove the ice after pickup
        }
    }
}
