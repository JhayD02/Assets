using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class JhayAnimation : NetworkBehaviour
{
    private Animator animator;
    private NetworkAnimator networkAnimator;
    int damage = 25;
    public float attackRadius = 1f;
    public Transform attackPoint;
    
    AudioManager audioManager;
    private WinConScript winConScript; // Reference to the WinConScript component

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        networkAnimator = GetComponent<NetworkAnimator>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();

        // Find the WinConScript in the scene
        GameObject winConObject = GameObject.Find("wincond");
        if (winConObject != null)
        {
            winConScript = winConObject.GetComponent<WinConScript>();
        }

        if (winConScript == null)
        {
            Debug.LogError("WinConScript component not found in the 'wincond' GameObject.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) return;

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            audioManager.PlayWalkSound();
        }
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
            audioManager.PlaySFX(audioManager.jumpSound);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            networkAnimator.SetTrigger("Melee");
            audioManager.PlaySFX(audioManager.punchSound);
        }
        
        if (Input.GetButtonDown("Fire2"))
        {
            networkAnimator.SetTrigger("Shoot");
            audioManager.PlaySFX(audioManager.shootSound);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            networkAnimator.SetTrigger("Hit");
            audioManager.PlaySFX(audioManager.damageSound);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            networkAnimator.SetTrigger("Death");
            audioManager.PlaySFX(audioManager.deathSound);

            // Call the lose game command
            if (winConScript != null)
            {
                winConScript.CmdLoseGame();
            }
            else
            {
                Debug.LogError("WinConScript component is null.");
            }
        }
    }

    public void TriggerAttack()
    {
        CmdAttack();
    }

    [Command]
    void CmdAttack()
    {
        RpcAttack();
    }

    [ClientRpc]
    void RpcAttack()
    {
        if (attackPoint == null)
        {
            Debug.LogError("Attack point is not set.");
            return;
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius);

        foreach (Collider2D enemy in hitEnemies)
        {
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
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}