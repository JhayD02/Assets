using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomBehavior : MonoBehaviour
{
    MushroomAnim mushroomAnim;
    SpriteRenderer spriteRenderer;

#region Detection
    Transform player;
     public float attackRange = 10f;
    public float stopDistance = 2f;
    public float attackDelay = 2.5f;
    bool isAttacking = false;
    bool playerInRange = false;
#endregion
    void Start()
    {
        mushroomAnim = GetComponent<MushroomAnim>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
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
                    spriteRenderer.flipX = false;
                }
                else
                {
                    spriteRenderer.flipX = true;
                }
            }

    }

    }

    IEnumerator Attack()
    {
        isAttacking = true;
        mushroomAnim.setAttack1Trigger();
        yield return new WaitForSeconds(attackDelay);
        isAttacking = false;
    }

    public void SetPlayerInRange(bool inRange, Transform playerTransform)
    {
        playerInRange = inRange;
        player = playerTransform;
    }
 
    
}
