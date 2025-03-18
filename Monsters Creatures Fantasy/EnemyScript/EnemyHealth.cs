using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EnemyHealth : NetworkBehaviour
{
    public float health;
    [SyncVar] public float currentHealth;
    GoblinAnim GoblinAnim;
    SkeletonAnim SkeletonAnim;
    FlyingAnim FlyingAnim;
    MushroomAnim MushroomAnim;
    BossAnimation bossAnim;
    [SyncVar] public bool isDead = false;
    
    public HealthBar healthBar;

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

    void Update()
    {
        if (isDead) return;

        if (currentHealth < health && currentHealth > 0)
        {
            // JUST TO PLAY ANIMATION
            Debug.Log("Enemy hit, triggering animations");
            if (GoblinAnim != null) GoblinAnim.RpcsethitTrigger();
            if (SkeletonAnim != null) SkeletonAnim.RpcsetDeathTrigger();
            if (FlyingAnim != null) FlyingAnim.RpcsethitTrigger();
            if (MushroomAnim != null) MushroomAnim.RpcsethitTrigger();
            if (bossAnim != null) bossAnim.RpcSetHitTrigger();
            health = currentHealth; 
        }
    }

    [Server]
    public void TakeDamage(float damage, bool triggerHitAnimation = true)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, health);
        }

        if (currentHealth <= 0)
        {
            isDead = true;
            RpcHandleDeath();
        }
        else if (triggerHitAnimation && currentHealth < health)
        {
            RpcHandleHit();
            health = currentHealth;
        }
    }

    [ClientRpc]
    void RpcHandleHit()
    {
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
        Destroy(gameObject, 1f);
        Debug.Log("Enemy is dead");
    }
}