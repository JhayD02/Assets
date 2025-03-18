using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinAnim : MonoBehaviour
{
   Animator Goblinanim;
    void Start()
    {
        Goblinanim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    //    if(Input.GetButtonDown("Fire1"))
    //     {
    //         Goblinanim.SetTrigger("Attack1");
    //     }
    //     if(Input.GetButtonDown("Fire2"))
    //     {
    //         Goblinanim.SetTrigger("Attack2");
    //     }
    }

    public void SetRun(float Run)
    {
        Goblinanim.SetFloat("Run", Run);
    }
     public void setAttack1Trigger()
    {
        Goblinanim.SetTrigger("Attack1");
    }
    public void setAttack2Trigger()
    {
        Goblinanim.SetTrigger("Attack2");
    }   
    public void sethitTrigger()
    {
        Goblinanim.SetTrigger("Hit");
        GetComponent<GoblinBehavior>().SetHitAnimationPlaying(true);
    }
    public void ResetHitAnimationFlag()
    {
        // Assuming SkeletonBehavior is attached to the same GameObject
        GetComponent<GoblinBehavior>().SetHitAnimationPlaying(false);
    }
    public void setDeathTrigger()
    {
        Goblinanim.SetTrigger("Death");
    }

    
    public bool IsInAttackAnimation()
    {
        return Goblinanim.GetCurrentAnimatorStateInfo(0).IsName("Attack1") || Goblinanim.GetCurrentAnimatorStateInfo(0).IsName("Attack2");
    }
    public bool IsInHitAnimation()
    {
        return Goblinanim.GetCurrentAnimatorStateInfo(0).IsName("Hit");
    }
}
