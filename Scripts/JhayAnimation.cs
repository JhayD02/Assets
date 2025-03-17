using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class JhayAnimation : NetworkBehaviour
{
    private Animator animator;
    [SerializeField] private GameObject hitbox;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
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
            animator.SetTrigger("Jump");
        }

        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("Melee");
        }
        
        if (Input.GetButtonDown("Fire2"))
        {
            animator.SetTrigger("Shoot");
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            animator.SetTrigger("Hit");
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            animator.SetTrigger("Death");
        }
    }

    // Called by animation event
    public void ActivateHitbox()
    {
        hitbox.SetActive(true);
    }

    // Called by animation event
    public void DeactivateHitbox() 
    {
        hitbox.SetActive(false);
    }
}