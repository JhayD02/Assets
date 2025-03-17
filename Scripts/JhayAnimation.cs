using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JhayAnimation : MonoBehaviour
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
            animator.SetTrigger("Shoot");
        }

        if (Input.GetButtonDown("Fire2"))
        {
            animator.SetTrigger("Melee");
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            animator.SetTrigger("Hit");
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            animator.SetTrigger("Death");
        }

        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
        hitbox.SetActive(currentState.IsName("Melee"));
    }
}