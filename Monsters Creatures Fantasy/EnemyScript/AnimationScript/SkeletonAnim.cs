using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SkeletonAnim : NetworkBehaviour
{
    Animator skeletonanim;
    void Start()
    {
        skeletonanim = GetComponent<Animator>();
    }

[ClientRpc]
    public void RpcSetWalk(float walk)
    {
        skeletonanim.SetFloat("Walk", walk);
    }
    [ClientRpc]
    public void RpcsetAttack1Trigger()
    {
        skeletonanim.SetTrigger("Attack1");
    }
    [ClientRpc]
    public void RpcsetAttack2Trigger()
    {
        skeletonanim.SetTrigger("Attack2");
    }   
    [ClientRpc]
    public void RpcsetBossattackTrigger()
    {
        skeletonanim.SetTrigger("Bossattack");
    }
    [ClientRpc]
    public void RpcsethitTrigger()
    {
        skeletonanim.SetTrigger("Hit");
        GetComponent<SkeletonBehavior>().SetHitAnimationPlaying(true);
        GetComponent<BossBehavior>().SetHitAnimationPlaying(true);
    }
    [ClientRpc]
    public void RpcResetHitAnimationFlag()
    {
        GetComponent<SkeletonBehavior>().SetHitAnimationPlaying(false);
        GetComponent<BossBehavior>().SetHitAnimationPlaying(false);

    }
    [ClientRpc]
    public void RpcsetDeathTrigger()
    {
        skeletonanim.SetTrigger("Death");
    }
    public bool IsInAttackAnimation()
    {
        return skeletonanim.GetCurrentAnimatorStateInfo(0).IsName("Attack1") || skeletonanim.GetCurrentAnimatorStateInfo(0).IsName("Attack2");
    }
    public bool IsInHitAnimation()
    {
        return skeletonanim.GetCurrentAnimatorStateInfo(0).IsName("Hit");
    }
}
