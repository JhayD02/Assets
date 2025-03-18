using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror; // For networking

public class Health : NetworkBehaviour
{
    [SerializeField]
    private float maxHealth = 100f;

    [SyncVar(hook = nameof(OnHealthChanged))]
    private float currentHealth;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    [SerializeField]
    public HealthBar healthBar; // Reference to the health bar transform
    private Animator animator; // Reference to the Animator component
    private WinConScript winConScript; // Reference to the WinConScript component
    private JhayAnimation jhayAnimation; // Reference to the JhayAnimation component

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        SetInitialHealthBarScale();
        UpdateHealthBar();
        animator = GetComponent<Animator>(); // Get the Animator component
        jhayAnimation = GetComponent<JhayAnimation>(); // Get the JhayAnimation component

        // Find the WinConScript in the scene
        GameObject winConObject = GameObject.Find("wincond");
        if (winConObject != null)
        {
            winConScript = winConObject.GetComponent<WinConScript>();
        }

        if (winConScript == null)
        {
            Debug.LogError("WinConScript component not found in the 'wincond' GameObject.");
        }

        if (jhayAnimation == null)
        {
            Debug.LogError("JhayAnimation component not found on the player.");
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
        RpcPlayHitAnimation(); // Trigger the hit animation on all clients

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

    private void SetInitialHealthBarScale()
    {
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }
    }
        private void OnHealthChanged(float oldHealth, float newHealth)
    {
        UpdateHealthBar();
    }

    [ClientRpc]
    private void RpcPlayDeathAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("Death"); // Assumes you have a "Death" trigger in your Animator
        }
    }

    [ClientRpc]
    private void RpcPlayHitAnimation()
    {
        if (jhayAnimation != null)
        {
            jhayAnimation.TriggerHit(); // Trigger the hit animation
        }
    }
}