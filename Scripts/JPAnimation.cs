using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JPAnimation : MonoBehaviour
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
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        animator.SetFloat("JPWalk", horizontalInput);

        animator.SetBool("isGrounded", PlayerMovement.isGrounded);

        if (Input.GetKey(KeyCode.LeftShift) && horizontalInput != 0)
        {
            animator.SetBool("JPRun", true);
        }
        else
        {
            animator.SetBool("JPRun", false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && PlayerMovement.isGrounded)
        {
            animator.SetTrigger("JPJump");
        }

        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("JPMelee");
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            animator.SetTrigger("JPHit");
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            animator.SetTrigger("JPDeath");
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