using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class JhayAnimation : NetworkBehaviour
{
    private Animator animator;
    private NetworkAnimator networkAnimator;
    [SerializeField] private GameObject hitbox;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        networkAnimator = GetComponent<NetworkAnimator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) return;

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        animator.SetFloat("Walk", horizontalInput);

        animator.SetBool("isGrounded", PlayerMovement.isGrounded);

        if (Input.GetKey(KeyCode.LeftShift) && horizontalInput != 0)
        {
            animator.SetBool("Run", true);
        }
        else
        {
            animator.SetBool("Run", false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && PlayerMovement.isGrounded)
        {
            networkAnimator.SetTrigger("Jump");
        }

        if (Input.GetButtonDown("Fire1"))
        {
            networkAnimator.SetTrigger("Melee");
        }
        
        if (Input.GetButtonDown("Fire2"))
        {
            networkAnimator.SetTrigger("Shoot");
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            networkAnimator.SetTrigger("Hit");
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            networkAnimator.SetTrigger("Death");
        }
    }

    // Called by animation event
    public void ActivateHitbox()
    {
        hitbox.SetActive(true);
         if (enemy.CompareTag("Enemy"))
            {
                Debug.Log("Hit " + enemy.name);
                EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damage);
                }
                else
                {
                    Debug.LogError("Enemy does not have an EnemyHealth component: " + enemy.name);
                }
            }
    }

    // Called by animation event
    public void DeactivateHitbox() 
    {
        hitbox.SetActive(false);
    }
}