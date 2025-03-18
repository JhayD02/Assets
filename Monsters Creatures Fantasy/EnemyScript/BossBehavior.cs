using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BossBehavior : NetworkBehaviour
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
    [SyncVar] bool isAttacking = false;
    [SyncVar] bool playerInRange = false;
    [SyncVar(hook = nameof(OnDirectionChanged))] bool isFacingRight = true;
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
        if (!isServer) return; // Ensure only the server controls the boss

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

            if (!bossAnim.IsAttacking())
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
        else
        {
            isAttacking = false;
        }
    }

    private bool useAttack1 = true;

    [Server]
    IEnumerator AlternateAttacks()
    {
        isAttacking = true;
        hasHitPlayer = false;

        if (useAttack1)
        {
            RpcSetAttack1Trigger();
        }
        else
        {
            RpcSetAttack2Trigger();
        }

        useAttack1 = !useAttack1;

        yield return new WaitForSeconds(attackDelay);
        isAttacking = false;
    }

    [ClientRpc]
    void RpcSetAttack1Trigger()
    {
        bossAnim.RpcSetAttack1Trigger();
    }

    [ClientRpc]
    void RpcSetAttack2Trigger()
    {
        bossAnim.RpcSetAttack2Trigger();
    }

    [Server]
    public void SetPlayerInRange(bool inRange, Transform playerTransform)
    {
        playerInRange = inRange;
        player = playerTransform;
    }

    [Server]
    public void Attack()
    {
        if (!bossAnim.IsInAttackAnimation() || isHitAnimationPlaying)
        {
            hasHitPlayer = false;
            return;
        }

        if (!hasHitPlayer && bossAnim.IsInAttackAnimation())
        {
            Collider2D[] players = Physics2D.OverlapCircleAll(Attackpoint.transform.position, attackradius);
            foreach (Collider2D collider in players)
            {
                if (collider.CompareTag(playerTag))
                {
                    hasHitPlayer = true;
                    Debug.Log("Collider detected: " + collider.name);
                    Health health = collider.GetComponent<Health>();
                    if (health != null)
                    {
                        health.TakeDamage(35);
                        Debug.Log("Player hit: " + collider.name + ", Health after damage: " + health.CurrentHealth);
                    }
                    else
                    {
                        Debug.LogError("Player does not have a Health component: " + collider.name);
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
}