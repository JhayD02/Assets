using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomBehavior : MonoBehaviour
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
    bool isAttacking = false;
    bool playerInRange = false;
    int check = 0;
#endregion
    void Start()
    {
        mushroomAnim = GetComponent<MushroomAnim>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialAttackPointLocalPosition = Attackpoint.transform.localPosition;
        enemyHealth = GetComponent<EnemyHealth>();


    }

    // Update is called once per frame
    void Update()
    {
    if(enemyHealth.isDead)
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
                    spriteRenderer.flipX = false;
                }
                else
                {
                    spriteRenderer.flipX = true;
                }
            }
        //This is just so the hitbox follows the direction of the sprite
        if (spriteRenderer.flipX)
        {
            Attackpoint.transform.localPosition = new Vector3(-Mathf.Abs(initialAttackPointLocalPosition.x), initialAttackPointLocalPosition.y, initialAttackPointLocalPosition.z);
        }
        else
        {
            Attackpoint.transform.localPosition = new Vector3(Mathf.Abs(initialAttackPointLocalPosition.x), initialAttackPointLocalPosition.y, initialAttackPointLocalPosition.z);
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
    public void attack()
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
        }
    }
    
}
