using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JPAnimation : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private GameObject hitbox;

    
    AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            audioManager.PlayWalkSound();
        }
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
            audioManager.PlaySFX(audioManager.jumpSound);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("JPMelee");
            audioManager.PlaySFX(audioManager.punchSound);
        }

        // if (Input.GetButtonDown("Fire2"))
        // {
        //     networkAnimator.SetTrigger("Shoot");
        //     audioManager.PlaySFX(audioManager.shootSound);
        // }

        if (Input.GetKeyDown(KeyCode.H))
        {
            animator.SetTrigger("JPHit");
            audioManager.PlaySFX(audioManager.damageSound);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            animator.SetTrigger("JPDeath");
            audioManager.PlaySFX(audioManager.deathSound);
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