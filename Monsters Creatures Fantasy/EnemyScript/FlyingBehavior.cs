using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class FlyingBehavior : NetworkBehaviour
{
    FlyingAnim flyingAnim;
    SpriteRenderer spriteRenderer;
    EnemyHealth enemyHealth;

    #region Movement
    bool checkDirectionX = true;
    float speed = .1f;
    public float distance = 10f;
    private bool isHitAnimationPlaying = false;
    #endregion
    
    #region Detection
    Transform player;
    public float attackRange = 2f;
    public float stopDistance = 2f;
    public float attackDelay = 2f;
    [SyncVar] private bool isAttacking = false;
    [SyncVar] private bool playerInRange = false;
    int check = 0;
    #endregion
    int checker = 0;
    [SerializeField]
    Transform FlameLoc;
    [SerializeField]
    GameObject FlamePrefab;

    [SyncVar(hook = nameof(OnDirectionChanged))]
    private bool isFacingRight = true;

    void Start()
    {
        flyingAnim = GetComponent<FlyingAnim>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyHealth = GetComponent<EnemyHealth>();

        if (enemyHealth == null)
        {
            Debug.LogError("EnemyHealth component not found on flying enemy: " + gameObject.name);
        }
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
            if (!isAttacking)
            {
                StartCoroutine(Attack());
            }

            if (player.position.x > transform.position.x)
            {
                SetDirection(true);
            }
            else
            {
                SetDirection(false);
            }
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        if (checkDirectionX)
        {
            transform.Translate(Vector3.right * speed / 3);
            SetDirection(true);
        }
        else
        {
            transform.Translate(Vector3.left * speed / 3);
            SetDirection(false);
        }
        if (transform.position.x > distance)
        {
            checkDirectionX = false;
        }
        if (transform.position.x < -distance)
        {
            checkDirectionX = true;
        }
    }

    [Server]
    IEnumerator Attack()
    {
        isAttacking = true;
        flyingAnim.RpcsetAttack2Trigger();
    
        yield return new WaitUntil(() => flyingAnim.isAttack1());
        if (player != null)
        {
            GameObject fireball = Instantiate(FlamePrefab, FlameLoc.position, Quaternion.identity);
            NetworkServer.Spawn(fireball); 
            fireball.GetComponent<FireballScript>().SetTargetPosition(player.position);
        }

        yield return new WaitForSeconds(attackDelay);
        isAttacking = false;
    }

    [Server]
    public void SetPlayerInRange(bool inRange, Transform playerTransform)
    {
        playerInRange = inRange;
        player = playerTransform;
    }

    public void SetHitAnimationPlaying(bool isPlaying)
    {
        isHitAnimationPlaying = isPlaying;
    }

    [Server]
    private void SetDirection(bool facingRight)
    {
        if (isFacingRight != facingRight)
        {
            isFacingRight = facingRight;
            RpcSetDirection(facingRight);
        }
    }

    [ClientRpc]
    private void RpcSetDirection(bool facingRight)
    {
        isFacingRight = facingRight;
        spriteRenderer.flipX = !facingRight;
    }

    private void OnDirectionChanged(bool oldDirection, bool newDirection)
    {
        spriteRenderer.flipX = !newDirection;
    }

    [ClientRpc]
    private void RpcUpdatePosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    [ServerCallback]
    void FixedUpdate()
    {
        if (playerInRange)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.fixedDeltaTime;
            RpcUpdatePosition(transform.position);
        }
        else
        {
            Patrol();
            RpcUpdatePosition(transform.position);
        }
    }
}