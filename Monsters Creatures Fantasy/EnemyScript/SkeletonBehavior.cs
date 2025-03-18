using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SkeletonBehavior : NetworkBehaviour
{
    SkeletonAnim skeletonAnim;
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
    [SyncVar] private bool isAttacking = false;
    [SyncVar] private bool playerInRange = false;
    #endregion

    #region Movement
    bool checkDirectionX = true;
    float speed = .1f;
    public float distance = 2f; // Reduced distance to 2 units
    int check = 0;
    [SyncVar(hook = nameof(OnPositionChanged))] Vector3 syncPosition;
    [SyncVar(hook = nameof(OnDirectionChanged))] bool syncDirection;
    private Vector3 initialPosition; // Store the initial position
    #endregion

    void Start()
    {
        skeletonAnim = GetComponent<SkeletonAnim>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialAttackPointLocalPosition = Attackpoint.transform.localPosition;
        enemyHealth = GetComponent<EnemyHealth>();
        initialPosition = transform.position; // Store the initial position

        if (skeletonAnim == null)
        {
            Debug.LogError("SkeletonAnim component not found on " + gameObject.name);
        }

        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found on " + gameObject.name);
        }

        if (enemyHealth == null)
        {
            Debug.LogError("EnemyHealth component not found on " + gameObject.name);
        }

        if (Attackpoint == null)
        {
            Debug.LogError("Attackpoint is not set on " + gameObject.name);
        }
    }

    void Update()
    {
        if (!isServer)
        {
            transform.position = syncPosition;
            spriteRenderer.flipX = syncDirection;
            return;
        }

        if (enemyHealth.isDead)
        {
            return;
        }

        if (playerInRange)
        {
            FollowPlayer();
        }
        else
        {
            Patrol();
        }

        if (spriteRenderer.flipX)
        {
            Attackpoint.transform.localPosition = new Vector3(-Mathf.Abs(initialAttackPointLocalPosition.x), initialAttackPointLocalPosition.y, initialAttackPointLocalPosition.z);
        }
        else
        {
            Attackpoint.transform.localPosition = new Vector3(Mathf.Abs(initialAttackPointLocalPosition.x), initialAttackPointLocalPosition.y, initialAttackPointLocalPosition.z);
        }
    }

    void Patrol()
    {
        if (checkDirectionX)
        {
            transform.Translate(Vector3.right * speed / 3);
            skeletonAnim.SetWalk(1f);
            spriteRenderer.flipX = false;
        }
        else
        {
            transform.Translate(Vector3.left * speed / 3);
            skeletonAnim.SetWalk(-1f);
            spriteRenderer.flipX = true;
        }

        if (transform.position.x > initialPosition.x + distance)
        {
            checkDirectionX = false;
        }
        if (transform.position.x < initialPosition.x - distance)
        {
            checkDirectionX = true;
        }

        syncPosition = transform.position;
        syncDirection = spriteRenderer.flipX;
    }

    void FollowPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > stopDistance)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            Vector3 newPosition = transform.position + direction * speed / 2;
            newPosition.y = transform.position.y;
            transform.position = newPosition;

            if (direction.x > 0)
            {
                spriteRenderer.flipX = false;
                skeletonAnim.SetWalk(1f);
            }
            else
            {
                spriteRenderer.flipX = true;
                skeletonAnim.SetWalk(-1f);
            }
        }
        else
        {
            skeletonAnim.SetWalk(0f);
        }

        if (distanceToPlayer <= attackRange && !isAttacking)
        {
            StartCoroutine(Attack());
        }

        syncPosition = transform.position;
        syncDirection = spriteRenderer.flipX;
    }

    [Server]
    IEnumerator Attack()
    {
        isAttacking = true;
        hasHitPlayer = false;
        skeletonAnim.RpcsetAttack1Trigger();
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
        if (!skeletonAnim.IsInAttackAnimation() || isHitAnimationPlaying)
        {
            hasHitPlayer = false;
            return;
        }

        if (!hasHitPlayer && skeletonAnim.IsInAttackAnimation())
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

    private void OnDrawGizmos()
    {
        if (Attackpoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(Attackpoint.transform.position, attackradius);
            Debug.Log("Attackpoint.transform.position: " + Attackpoint.transform.position);
        }
    }

    void OnPositionChanged(Vector3 oldPosition, Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    void OnDirectionChanged(bool oldDirection, bool newDirection)
    {
        spriteRenderer.flipX = newDirection;
    }
}