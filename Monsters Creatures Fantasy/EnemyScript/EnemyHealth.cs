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
    public bool isDead = false;

    void Start()
    {
        currentHealth = health;
        GoblinAnim = GetComponent<GoblinAnim>();
        SkeletonAnim = GetComponent<SkeletonAnim>();
        FlyingAnim = GetComponent<FlyingAnim>();
        MushroomAnim = GetComponent<MushroomAnim>();
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
            health = currentHealth; 
        }
    }

    public void TakeDamage(float damage)
    {
        // To not play the hit animation if it's the last hit
        if (isDead) return;

        currentHealth -= damage;
        // TO PLAY DEATH ANIMATION IF IT'S THE LAST HIT
        if (currentHealth <= 0)
        {
            isDead = true;
            if (GoblinAnim != null) GoblinAnim.setDeathTrigger();
            if (SkeletonAnim != null) SkeletonAnim.setDeathTrigger();
            if (FlyingAnim != null) FlyingAnim.setDeathTrigger();
            if (MushroomAnim != null) MushroomAnim.setDeathTrigger();
            Destroy(gameObject, 1f);
            Debug.Log("Enemy is dead");
        }
    }
}