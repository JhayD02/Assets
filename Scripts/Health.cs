using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 100f;
    private float currentHealth;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    [SerializeField]
    private Transform healthBar; // Reference to the health bar transform

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        SetInitialHealthBarScale();
        UpdateHealthBar();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();
        if (currentHealth <= 0)
        {
            // Handle death
            Debug.Log("Player is dead");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("SafetyNet"))
        {
            TakeDamage(10f); // Example damage amount
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
}