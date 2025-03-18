using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float moveDistance = 5f; // How far the platform moves
    public float moveSpeed = 2f; // How fast the platform moves
    public bool isVertical = true; // If false, moves horizontally

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool isMovingToTarget = true;

    void Start()
    {
        startPosition = transform.position;
        targetPosition = isVertical
            ? startPosition + new Vector3(0, moveDistance, 0) // Move Up/Down
            : startPosition + new Vector3(moveDistance, 0, 0); // Move Left/Right
    }

    public void ToggleMovement()
    {
        StopAllCoroutines(); // Prevents overlap movement issues
        StartCoroutine(MovePlatform(isMovingToTarget ? targetPosition : startPosition));
        isMovingToTarget = !isMovingToTarget; // Swap direction
    }

    IEnumerator MovePlatform(Vector3 destination)
    {
        while (transform.position != destination)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
}