using UnityEngine;

public class Trampoline : MonoBehaviour
{
    public float bounceForce = 10f; // Adjust to control jump strength

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Ensure only players bounce
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0); // Reset Y velocity before bouncing
                rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse); // Apply bounce force
                Debug.Log(other.name + " bounced on the trampoline!");
            }
        }
    }
}
