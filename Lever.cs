using System.Collections;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public MovingPlatform platform; // Assign the shared platform script
    public float cooldownTime = 3f; // Cooldown before lever can be reused

    private bool isCooldown = false;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isCooldown && collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(ActivateLever());
        }
    }

    IEnumerator ActivateLever()
    {
        isCooldown = true;
        platform.ToggleMovement(); // Tell the platform to move or return
        yield return new WaitForSeconds(cooldownTime);
        isCooldown = false;
    }
}
