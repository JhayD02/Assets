using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBehavior : MonoBehaviour
{
    FlyingAnim flyingAnim;
    SpriteRenderer spriteRenderer;

    #region Movement
    bool checkDirectionX = true;
    float speed = .1f;
    public float distance = 10f;
    #endregion
    
    #region Detection
    Transform player;
    public float attackRange = 2f;
    public float stopDistance = 2f;
    public float attackDelay = 2f;
    bool isAttacking = false;
    bool playerInRange = false;
    #endregion

    [SerializeField]
    Transform FlameLoc;
    [SerializeField]
    GameObject FlamePrefab;

    void Start()
    {
        flyingAnim = GetComponent<FlyingAnim>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange)
        {
            if (!isAttacking)
            {
                StartCoroutine(Attack());
            }

            // Update sprite direction based on player's position
            if (player.position.x > transform.position.x)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
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
            spriteRenderer.flipX = false;
        }
        else
        {
            transform.Translate(Vector3.left * speed / 3);
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

    IEnumerator Attack()
    {
        isAttacking = true;
        flyingAnim.setAttack2Trigger();
        
         Debug.Log("Attack triggered");

        yield return new WaitUntil(() => flyingAnim.isAttack1());
        GameObject fireball = Instantiate(FlamePrefab, FlameLoc.position, Quaternion.identity);
        fireball.GetComponent<FireballScript>().SetTargetPosition(player.position);


        yield return new WaitForSeconds(attackDelay);
        isAttacking = false;
    }

    public void SetPlayerInRange(bool inRange, Transform playerTransform)
    {
        playerInRange = inRange;
        player = playerTransform;
        
    }


}