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
    [SyncVar] public bool isDead = false;
    private bool hitTriggered = false;

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
        jhayAnimation = GetComponentInChildren<JhayAnimation>(); // Get the JhayAnimation component from children

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
    public void TakeDamage(float amount, bool triggerHitAnimation = true)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            isDead = true;
            RpcHandleDeath();
            if (winConScript != null)
            {
                winConScript.CmdLoseGame();
            }
            else
            {
                Debug.LogError("WinConScript component is null.");
            }
        }
        else if (triggerHitAnimation && !hitTriggered) // Only trigger if not already hit
        {
            hitTriggered = true;
            RpcHandleHit();
            StartCoroutine(ResetHitTrigger()); // Reset hit trigger after a short delay
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
    void RpcHandleHit()
    {
        if (isDead) return; // Prevent hit animation if already dead

        // JUST TO PLAY ANIMATION
        Debug.Log("Player hit, triggering animations");
        if (jhayAnimation != null) jhayAnimation.TriggerHit();
    }

    [ClientRpc]
    void RpcHandleDeath()
    {
        if (jhayAnimation != null)
        {
            Debug.Log("Triggering death animation");
            jhayAnimation.TriggerDeath(); // Assumes you have a "Death" trigger in your Animator
        }
        Debug.Log("Player is dead");
    }

    private IEnumerator ResetHitTrigger()
    {
        yield return new WaitForSeconds(0.5f); // Adjust delay as needed
        hitTriggered = false;
    }
}