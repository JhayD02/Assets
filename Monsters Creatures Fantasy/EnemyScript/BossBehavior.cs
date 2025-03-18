using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehavior : MonoBehaviour
{
    BossAnimation bossAnim;
    SpriteRenderer spriteRenderer;
    EnemyHealth enemyHealth;

    #region Hitbox
    public GameObject Attackpoint;
    public float attackradius;
    public string playerTag = "Player";
    private bool hasHitPlayer = false;
    private Vector3 initialAttackPointLocalPosition;
    private bool isHitAnimationPlaying = false;
    #endregion

    #region Detection
    Transform player;
    public float attackRange = 2f;
    public float stopDistance = 2f;
    public float attackDelay = 2f;
    bool isAttacking = false;
    bool playerInRange = false;
    int check = 0;
    #endregion

    void Start()
    {
        bossAnim = GetComponent<BossAnimation>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialAttackPointLocalPosition = Attackpoint.transform.localPosition;
        enemyHealth = GetComponent<EnemyHealth>();

    }


    void Update()
    {
        if (enemyHealth.isDead)
        {
            return;
        }

        if (playerInRange)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= attackRange && !isAttacking)
            {
                StartCoroutine(AlternateAttacks());
            }

            if (!bossAnim.isAttacking())
            {
                if (player.position.x > transform.position.x)
                {
                    spriteRenderer.flipX = false;
                }
                else
                {
                    spriteRenderer.flipX = true;
                }
            }

            // This is just so the hitbox follows the direction of the sprite
            if (spriteRenderer.flipX)
            {
                Attackpoint.transform.localPosition = new Vector3(-Mathf.Abs(initialAttackPointLocalPosition.x), initialAttackPointLocalPosition.y, initialAttackPointLocalPosition.z);
            }
            else
            {
                Attackpoint.transform.localPosition = new Vector3(Mathf.Abs(initialAttackPointLocalPosition.x), initialAttackPointLocalPosition.y, initialAttackPointLocalPosition.z);
            }
        }
        else
        {
            isAttacking = false;
        }
    }

    private bool useAttack1 = true;

    IEnumerator AlternateAttacks()
    {
        isAttacking = true;
        hasHitPlayer = false;

        if (useAttack1)
        {
            bossAnim.setAttack1Trigger();
        }
        else
        {
            bossAnim.setAttack2Trigger();
        }

        useAttack1 = !useAttack1; // Toggle the attack

        yield return new WaitForSeconds(attackDelay);
        isAttacking = false;
    }
    public void SetPlayerInRange(bool inRange, Transform playerTransform)
    {
        playerInRange = inRange;
        player = playerTransform;
    }

 public void attack()
{
    if (!bossAnim.IsInAttackAnimation() || isHitAnimationPlaying)
    {
        hasHitPlayer = false;
        return;
    }

    if (!hasHitPlayer && bossAnim.IsInAttackAnimation())
    {
        Collider2D[] players = Physics2D.OverlapCircleAll(Attackpoint.transform.position, attackradius);
        Debug.Log("Players detected: " + players.Length);

        foreach (Collider2D collider in players)
        {
            if (collider.CompareTag(playerTag))
            {
                Debug.Log("hit player" + check);
                hasHitPlayer = true;
                check++;
                Health health = collider.GetComponent<Health>();
                if (health != null)
                {
                    Debug.Log("Player health before damage: " + health.CurrentHealth);
                    health.TakeDamage(35); // Adjust the damage value as needed
                    Debug.Log("Player health after damage: " + health.CurrentHealth);
                }
                else
                {
                    Debug.LogError("Player does not have a PlayerHealth component: " + collider.name);
                }
                break;
            }
        }
    }
}

    private void OnDrawGizmos()
    {
        if (Attackpoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(Attackpoint.transform.position, attackradius);
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void SetHitAnimationPlaying(bool isPlaying)
    {
        isHitAnimationPlaying = isPlaying;
    }
}