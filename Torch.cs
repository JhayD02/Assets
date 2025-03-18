using UnityEngine;

public class Torch : MonoBehaviour
{
    public GameObject[] smokes; // Assign 5 smoke GameObjects in the Inspector
    private static int torchesCollected = 0;
    private static int totalTorches = 2; // Change this if needed

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Ensure only players can collect
        {
            torchesCollected++;
            Debug.Log(gameObject.name + " collected! Total: " + torchesCollected + "/" + totalTorches);

            Destroy(gameObject); // Remove the torch

            if (torchesCollected >= totalTorches)
            {
                RemoveSmoke();
            }
        }
    }

    void RemoveSmoke()
    {
        Debug.Log("All torches collected! Removing smoke...");
        foreach (GameObject smoke in smokes)
        {
            if (smoke != null)
            {
                smoke.SetActive(false); // Disable smoke
            }
        }
    }
}
