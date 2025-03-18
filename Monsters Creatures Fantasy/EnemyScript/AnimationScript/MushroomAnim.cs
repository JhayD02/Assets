using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomAnim : MonoBehaviour
{
    Animator mushroomanim;
    void Start()
    {
        mushroomanim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    //    if(Input.GetButtonDown("Fire1"))
    //     {
    //         mushroomanim.SetTrigger("Attack1");
    //     }
    //     if(Input.GetButtonDown("Fire2"))
    //     {
    //         mushroomanim.SetTrigger("Attack2");
    //     }
    }


    public void setAttack1Trigger()
    {
        mushroomanim.SetTrigger("Attack1");
    }
    public void setAttack2Trigger()
    {
        mushroomanim.SetTrigger("Attack2");
    }   
    public void sethitTrigger()
    {
        mushroomanim.SetTrigger("Hit");
        GetComponent<MushroomBehavior>().SetHitAnimationPlaying(true);
    }
    public void ResetHitAnimationFlag()
    {
        GetComponent<MushroomBehavior>().SetHitAnimationPlaying(false);
    }
    public void setDeathTrigger()
    {
        mushroomanim.SetTrigger("Death");
    }

    public bool isAttacking()
    {
         return mushroomanim.GetCurrentAnimatorStateInfo(0).IsTag("Attack");
    }
        public bool IsInAttackAnimation()
    {
        return mushroomanim.GetCurrentAnimatorStateInfo(0).IsName("Attack1") || mushroomanim.GetCurrentAnimatorStateInfo(0).IsName("Attack2");
    }
    public bool IsInHitAnimation()
    {
        return mushroomanim.GetCurrentAnimatorStateInfo(0).IsName("Hit");
    }
}
