using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MushroomAnim : NetworkBehaviour
{
    Animator mushroomanim;
    void Start()
    {
        mushroomanim = GetComponent<Animator>();
    }

    [ClientRpc]
    public void RpcsetAttack1Trigger()
    {
        mushroomanim.SetTrigger("Attack1");
    }
    [ClientRpc]
    public void RpcsetAttack2Trigger()
    {
        mushroomanim.SetTrigger("Attack2");
    }   
    [ClientRpc]
    public void RpcsethitTrigger()
    {
        mushroomanim.SetTrigger("Hit");
        GetComponent<MushroomBehavior>().SetHitAnimationPlaying(true);
    }
    [ClientRpc]
    public void RpcResetHitAnimationFlag()
    {
        GetComponent<MushroomBehavior>().SetHitAnimationPlaying(false);
    }
    [ClientRpc]
    public void RpcsetDeathTrigger()
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
