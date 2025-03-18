using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimation : MonoBehaviour
{
    Animator bossAnim;
    void Start()
    {
       bossAnim = GetComponent<Animator>(); 
    }

     public void SetWalk(float walk)
    {
        bossAnim.SetFloat("Walk", walk);
    }
    public void setAttack1Trigger()
    {
        bossAnim.SetTrigger("Attack1");
    }
    public void setAttack2Trigger()
    {
        bossAnim.SetTrigger("Attack2");
    }   
    public void setBossattackTrigger()
    {
        bossAnim.SetTrigger("Bossattack");
    }
        public void sethitTrigger()
    {
        bossAnim.SetTrigger("Hit");
        GetComponent<BossBehavior>().SetHitAnimationPlaying(true);
    }
    public void ResetHitAnimationFlag()
    {
        GetComponent<BossBehavior>().SetHitAnimationPlaying(false);

    }
        public void setDeathTrigger()
    {
        bossAnim.SetTrigger("Death");
    }
        public bool isAttacking()
    {
         return bossAnim.GetCurrentAnimatorStateInfo(0).IsTag("Attack");
    }
    public bool IsInAttackAnimation()
    {
        return bossAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack1") || bossAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack2");
    }
    public bool IsInHitAnimation()
    {
        return bossAnim.GetCurrentAnimatorStateInfo(0).IsName("Hit");
    }

}
