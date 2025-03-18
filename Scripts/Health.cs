using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror; // For networking

public class Health : NetworkBehaviour
{
    [SerializeField]
    private float maxHealth = 100f;
    [SyncVar]
    private float currentHealth;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    [SerializeField]
    private Transform healthBar; // Reference to the health bar transform
    private Animator animator; // Reference to the Animator component
    private WinConScript winConScript; // Reference to the WinConScript component

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        SetInitialHealthBarScale();
        UpdateHealthBar();
        animator = GetComponent<Animator>(); // Get the Animator component
        winConScript = GetComponent<WinConScript>(); // Get the WinConScript component

        if (winConScript == null)
        {
            Debug.LogError("WinConScript component not found on the GameObject.");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    [Server]
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();
        if (currentHealth <= 0)
        {
            // Handle death
            Debug.Log("Player is dead");
            RpcPlayDeathAnimation(); // Trigger the death animation on all clients

            // Call the lose game command
            if (winConScript != null)
            {
                winConScript.CmdLoseGame();
            }
            else
            {
                Debug.LogError("WinConScript component is null.");
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("SafetyNet"))
        {
            if (isServer)
            {
                TakeDamage(20f); // Example damage amount
            }
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            Vector3 healthBarScale = healthBar.localScale;
            healthBarScale.x = currentHealth / maxHealth;
            healthBar.localScale = healthBarScale;
        }
    }

    private void SetInitialHealthBarScale()
    {
        if (healthBar != null)
        {
            Vector3 healthBarScale = healthBar.localScale;
            healthBarScale.x = 1f; // Set the initial scale to 1
            healthBar.localScale = healthBarScale;
        }
    }

    [ClientRpc]
    private void RpcPlayDeathAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("Death"); // Assumes you have a "Die" trigger in your Animator
        }
    }
}