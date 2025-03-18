using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class FlyingAnim : NetworkBehaviour
{
    Animator Flyinganim;
    void Start()
    {
        Flyinganim = GetComponent<Animator>();
    }


    [ClientRpc]
      public void RpcsetAttack1Trigger()
    {
        Flyinganim.SetTrigger("Attack1");
    }
    [ClientRpc]
    public void RpcsetAttack2Trigger()
    {
        Flyinganim.SetTrigger("Attack2");
    }   
    [ClientRpc]
    public void RpcsethitTrigger()
    {
        Flyinganim.SetTrigger("Hit");
        GetComponent<FlyingBehavior>().SetHitAnimationPlaying(true);
    }
    [ClientRpc]
    public void RpcResetHitAnimationFlag()
    {
        // Assuming SkeletonBehavior is attached to the same GameObject
        GetComponent<FlyingBehavior>().SetHitAnimationPlaying(false);
    }
    [ClientRpc]
    public void RpcsetDeathTrigger()
    {
        Flyinganim.SetTrigger("Death");
    }


    public bool isAttack1()
    {
        return Flyinganim.GetCurrentAnimatorStateInfo(0).IsName("Attack1");
    }
        public bool IsInAttackAnimation()
    {
        return Flyinganim.GetCurrentAnimatorStateInfo(0).IsName("Attack1") || Flyinganim.GetCurrentAnimatorStateInfo(0).IsName("Attack2");
    }
    public bool IsInHitAnimation()
    {
        return Flyinganim.GetCurrentAnimatorStateInfo(0).IsName("Hit");
    }
}
