using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BossAnimation : NetworkBehaviour
{
    private Animator bossAnim;

    void Start()
    {
        bossAnim = GetComponent<Animator>();
    }

    [ClientRpc]
    public void RpcSetWalk(float walk)
    {
        bossAnim.SetFloat("Walk", walk);
    }

    [ClientRpc]
    public void RpcSetAttack1Trigger()
    {
        bossAnim.SetTrigger("Attack1");
    }

    [ClientRpc]
    public void RpcSetAttack2Trigger()
    {
        bossAnim.SetTrigger("Attack2");
    }

    [ClientRpc]
    public void RpcSetHitTrigger()
    {
        bossAnim.SetTrigger("Hit");
        GetComponent<BossBehavior>().SetHitAnimationPlaying(true);
    }

    [ClientRpc]
    public void RpcResetHitAnimationFlag()
    {
        GetComponent<BossBehavior>().SetHitAnimationPlaying(false);
    }

    [ClientRpc]
    public void RpcSetDeathTrigger()
    {
        bossAnim.SetTrigger("Death");
    }

    public bool IsAttacking()
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