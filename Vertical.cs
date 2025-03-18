using System.Collections;
using UnityEngine;

public class Vertical : MonoBehaviour
{
    public GameObject platform; // Assign platform in Inspector
    public float moveDistance = 10f; // Distance to move
    public float moveSpeed = 2f; // Movement speed
    public float cooldownTime = 5f; // Time before lever can be used again
    private bool isMovingDown = true; // Tracks platform state
    private bool isCooldown = false; // Prevents spam activation
    private Vector3 startPosition;
    private Vector3 targetPosition;

    void Start()
    {
        if (platform != null)
        {
            startPosition = platform.transform.position;
            targetPosition = startPosition + new Vector3(0, -moveDistance, 0);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isCooldown && other.CompareTag("Player")) // Ensure Player has "Player" tag
        {
            StartCoroutine(ActivateLever());
        }
    }

    IEnumerator ActivateLever()
    {
        isCooldown = true; // Prevent spam activation
        StartCoroutine(MovePlatform(isMovingDown));
        isMovingDown = !isMovingDown; // Toggle movement state
        yield return new WaitForSeconds(cooldownTime); // Wait for cooldown
        isCooldown = false; // Re-enable lever activation
    }

    IEnumerator MovePlatform(bool movingDown)
    {
        Vector3 destination = movingDown ? targetPosition : startPosition;

        while (platform.transform.position != destination)
        {
            platform.transform.position = Vector3.MoveTowards(platform.transform.position, destination, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
}