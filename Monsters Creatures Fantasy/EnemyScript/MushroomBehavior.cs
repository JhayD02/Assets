using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MushroomBehavior : NetworkBehaviour
{
    MushroomAnim mushroomAnim;
    SpriteRenderer spriteRenderer;  
    EnemyHealth enemyHealth;

    #region Hitbox
    public GameObject Attackpoint;
    public float attackradius;
    public string playerTag = "Player";
    private bool hasHitPlayer = false;
    private bool isHitAnimationPlaying = false;
    private Vector3 initialAttackPointLocalPosition;
    #endregion

    #region Detection
    Transform player;
    public float attackRange = 10f;
    public float stopDistance = 2f;
    public float attackDelay = 2.5f;
    [SyncVar] private bool isAttacking = false;
    [SyncVar] private bool playerInRange = false;
    [SyncVar(hook = nameof(OnDirectionChanged))] private bool isFacingRight = true;
    int check = 0;
    #endregion

    void Start()
    {
        mushroomAnim = GetComponent<MushroomAnim>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialAttackPointLocalPosition = Attackpoint.transform.localPosition;
        enemyHealth = GetComponent<EnemyHealth>();
    }

    void Update()
    {
        if (!isServer) return;

        if (enemyHealth.isDead)
        {
            return;
        }

        if (playerInRange)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= attackRange && !isAttacking)
            {
                StartCoroutine(Attack());
            }

            if (!mushroomAnim.isAttacking())
            {
                if (player.position.x > transform.position.x)
                {
                    isFacingRight = true;
                }
                else
                {
                    isFacingRight = false;
                }
            }

            // Sync hitbox direction
            if (isFacingRight)
            {
                Attackpoint.transform.localPosition = new Vector3(Mathf.Abs(initialAttackPointLocalPosition.x), initialAttackPointLocalPosition.y, initialAttackPointLocalPosition.z);
            }
            else
            {
                Attackpoint.transform.localPosition = new Vector3(-Mathf.Abs(initialAttackPointLocalPosition.x), initialAttackPointLocalPosition.y, initialAttackPointLocalPosition.z);
            }
        }
    }

    [Server]
    IEnumerator Attack()
    {
        isAttacking = true;
        mushroomAnim.RpcsetAttack1Trigger();
        yield return new WaitForSeconds(attackDelay);
        isAttacking = false;
    }

    [Server]
    public void SetPlayerInRange(bool inRange, Transform playerTransform)
    {
        playerInRange = inRange;
        player = playerTransform;
    }

    [Server]
    public void AttackPlayer()
    {
        if (!mushroomAnim.IsInAttackAnimation() || isHitAnimationPlaying)
        {
            hasHitPlayer = false; 
            return;
        }
        
        if (!hasHitPlayer && mushroomAnim.IsInAttackAnimation()) 
        {
            Collider2D[] players = Physics2D.OverlapCircleAll(Attackpoint.transform.position, attackradius);

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
                        health.TakeDamage(25); // Adjust the damage value as needed
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

    public void SetHitAnimationPlaying(bool isPlaying)
    {
        isHitAnimationPlaying = isPlaying;
    }

    void OnDirectionChanged(bool oldValue, bool newValue)
    {
        spriteRenderer.flipX = !newValue;
    }

    private void OnDrawGizmos()
    {
        if (Attackpoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(Attackpoint.transform.position, attackradius);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            // Ignore bullet damage
            Debug.Log("Mushroom enemy is immune to bullet damage.");
        }
    }
}