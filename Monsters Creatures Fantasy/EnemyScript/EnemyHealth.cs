using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EnemyHealth : NetworkBehaviour
{
    public float health;
    [SyncVar(hook = nameof(OnHealthChanged))] public float currentHealth;
    GoblinAnim GoblinAnim;
    SkeletonAnim SkeletonAnim;
    FlyingAnim FlyingAnim;
    MushroomAnim MushroomAnim;
    BossAnimation bossAnim;
    [SyncVar] public bool isDead = false;
    
    public HealthBar healthBar;
    private bool hitTriggered = false; // Flag to control hit animation trigger

    void Start()
    {
        currentHealth = health;
        GoblinAnim = GetComponent<GoblinAnim>();
        SkeletonAnim = GetComponent<SkeletonAnim>();
        FlyingAnim = GetComponent<FlyingAnim>();
        MushroomAnim = GetComponent<MushroomAnim>();
        bossAnim = GetComponent<BossAnimation>();
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, health);
        }
    }

    [Server]
    public void TakeDamage(float damage, bool triggerHitAnimation = true)
    {
        if (isDead) return;

        float oldHealth = currentHealth;
        currentHealth -= damage;

        if (healthBar != null)
        {
            Debug.Log($"Taking damage: {damage}, new health: {currentHealth}");
            healthBar.UpdateHealthBar(currentHealth, health);
        }

        if (currentHealth <= 0)
        {
            isDead = true;
            RpcHandleDeath();
        }
        else if (triggerHitAnimation && !hitTriggered) // Only trigger if not already hit
        {
            hitTriggered = true;
            RpcHandleHit();
            StartCoroutine(ResetHitTrigger()); // Reset hit trigger after a short delay
        }
    }

    [ClientRpc]
    void RpcHandleHit()
    {
        if (isDead) return; // Prevent hit animation if already dead

        // JUST TO PLAY ANIMATION
        Debug.Log("Enemy hit, triggering animations");
        if (GoblinAnim != null) GoblinAnim.RpcsethitTrigger();
        if (SkeletonAnim != null) SkeletonAnim.RpcsethitTrigger();
        if (FlyingAnim != null) FlyingAnim.RpcsethitTrigger();
        if (MushroomAnim != null) MushroomAnim.RpcsethitTrigger();
        if (bossAnim != null) bossAnim.RpcSetHitTrigger();
    }

    [ClientRpc]
    void RpcHandleDeath()
    {
        if (GoblinAnim != null) GoblinAnim.RpcsetDeathTrigger();
        if (SkeletonAnim != null) SkeletonAnim.RpcsetDeathTrigger();
        if (FlyingAnim != null) FlyingAnim.RpcsetDeathTrigger();
        if (MushroomAnim != null) MushroomAnim.RpcsetDeathTrigger();
        if (bossAnim != null) bossAnim.RpcSetDeathTrigger();
        
        Debug.Log("Enemy is dead");
        Destroy(gameObject, 1f);
    }

    void OnHealthChanged(float oldHealth, float newHealth)
    {
        if (healthBar != null)
        {
            Debug.Log($"Health changed: {oldHealth} -> {newHealth}");
            healthBar.UpdateHealthBar(newHealth, health);
        }
    }

    private IEnumerator ResetHitTrigger()
    {
        yield return new WaitForSeconds(0.5f); // Adjust delay as needed
        hitTriggered = false;
    }
}
