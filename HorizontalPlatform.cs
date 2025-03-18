using System.Collections;
using UnityEngine;

public class HorizontalPlatform : MonoBehaviour
{
    public GameObject platform; // Assign platform in Inspector
    public float moveDistance = 5f; // Distance to move horizontally
    public float moveSpeed = 2f; // Movement speed

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool isPlayerOnLever = false;

    void Start()
    {
        if (platform != null)
        {
            startPosition = platform.transform.position;
            targetPosition = startPosition + new Vector3(moveDistance, 0, 0); // Move horizontally
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Ensure Player has "Player" tag
        {
            isPlayerOnLever = true;
            StopAllCoroutines(); // Stop previous movement
            StartCoroutine(MovePlatform(targetPosition));
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // When player leaves the lever
        {
            isPlayerOnLever = false;
            StopAllCoroutines(); // Stop current movement
            StartCoroutine(MovePlatform(startPosition));
        }
    }

    IEnumerator MovePlatform(Vector3 destination)
    {
        while (platform.transform.position != destination)
        {
            platform.transform.position = Vector3.MoveTowards(platform.transform.position, destination, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
}