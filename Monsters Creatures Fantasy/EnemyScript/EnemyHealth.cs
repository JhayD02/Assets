using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health;
    public float currentHealth;
    GoblinAnim GoblinAnim;
    SkeletonAnim SkeletonAnim;
    FlyingAnim FlyingAnim;
    MushroomAnim MushroomAnim;
    BossAnimation bossAnim;
    public bool isDead = false;
    
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

    // Update is called once per frame
    void Update()
    {
        // To not play the hit animation if it's the last hit
        if (isDead) return;

        if (currentHealth < health && currentHealth > 0)
        {
            // JUST TO PLAY ANIMATION
            Debug.Log("Enemy hit, triggering animations");
            if (GoblinAnim != null) GoblinAnim.sethitTrigger();
            if (SkeletonAnim != null) SkeletonAnim.sethitTrigger();
            if (FlyingAnim != null) FlyingAnim.sethitTrigger();
            if (MushroomAnim != null) MushroomAnim.sethitTrigger();
            if (bossAnim != null) bossAnim.sethitTrigger();
            health = currentHealth; 
        }
    }

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
            if (GoblinAnim != null) GoblinAnim.setDeathTrigger();
            if (SkeletonAnim != null) SkeletonAnim.setDeathTrigger();
            if (FlyingAnim != null) FlyingAnim.setDeathTrigger();
            if (MushroomAnim != null) MushroomAnim.setDeathTrigger();
            if (bossAnim != null) bossAnim.setDeathTrigger();
            Destroy(gameObject, 1f);
            Debug.Log("Enemy is dead");
        }
        else if (triggerHitAnimation && currentHealth < health)
        {
            // JUST TO PLAY ANIMATION
            Debug.Log("Enemy hit, triggering animations");
            if (GoblinAnim != null) GoblinAnim.sethitTrigger();
            if (SkeletonAnim != null) SkeletonAnim.sethitTrigger();
            if (FlyingAnim != null) FlyingAnim.sethitTrigger();
            if (MushroomAnim != null) MushroomAnim.sethitTrigger();
            if (bossAnim != null) bossAnim.sethitTrigger();
            health = currentHealth;
        }
    }
}