using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinBehavior : MonoBehaviour
{
    GoblinAnim goblinAnim;
    SpriteRenderer spriteRenderer;
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
        goblinAnim = GetComponent<GoblinAnim>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (playerInRange)
        {
            FollowPlayer();
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
            goblinAnim.SetRun(1f);
            spriteRenderer.flipX = false;
        }
        else
        {
            transform.Translate(Vector3.left * speed / 3);
            goblinAnim.SetRun(-1f);
            spriteRenderer.flipX = true;
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
                goblinAnim.SetRun(1f);
            }
            else
            {
                spriteRenderer.flipX = true;
                goblinAnim.SetRun(-1f);
            }
        }
        else
            {
               goblinAnim.SetRun(0f);
            }

            if (distanceToPlayer <= attackRange && !isAttacking)
            {
                StartCoroutine(Attack());
            }
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        goblinAnim.setAttack1Trigger();
        yield return new WaitForSeconds(attackDelay);
        isAttacking = false;
    }

    public void SetPlayerInRange(bool inRange, Transform playerTransform)
    {
        playerInRange = inRange;
        player = playerTransform;
    }
}
