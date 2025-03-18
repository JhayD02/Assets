using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBehavior : MonoBehaviour
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
    #endregion

    #region Detection
    Transform player;
    public float attackRange = 2f;
    public float stopDistance = 2f;
    public float attackDelay = 2f;
    bool isAttacking = false;
    bool playerInRange = false;

    #endregion
    #region  Movement
    bool checkDirectionX = true;
    float speed = .1f;
    public float distance = 10f;
    #endregion

    void Start()
    {
        skeletonAnim = GetComponent<SkeletonAnim>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialAttackPointLocalPosition = Attackpoint.transform.localPosition;
        enemyHealth = GetComponent<EnemyHealth>();

    }

    void Update()
    {
        if(enemyHealth.isDead)
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
            transform.Translate(Vector3.right * speed/4);
            skeletonAnim.SetWalk(1f);
            spriteRenderer.flipX = false;
        }
        else
        {
            transform.Translate(Vector3.left * speed/4);
             skeletonAnim.SetWalk(-1f);
             spriteRenderer.flipX = true;
        }
        if (transform.position.x > distance)
        {
            checkDirectionX = false;
        }
        else if (transform.position.x < -distance)
        {
            checkDirectionX = true;
        } 
    }

      void FollowPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > stopDistance)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            Vector3 newPosition = transform.position + direction * speed / 3;
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
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        hasHitPlayer = false;
        skeletonAnim.setAttack1Trigger();
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
        if (!hasHitPlayer) 
        {
            Collider2D[] players = Physics2D.OverlapCircleAll(Attackpoint.transform.position, attackradius);

            foreach (Collider2D collider in players)
            {
                if (collider.CompareTag(playerTag))
                {
                    Debug.Log("hit player");
                    hasHitPlayer = true; 
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
    }
}
