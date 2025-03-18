using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GoblinAnim : NetworkBehaviour
{
   Animator Goblinanim;
    void Start()
    {
        Goblinanim = GetComponent<Animator>();
    }


    [ClientRpc]
    public void RpcSetRun(float Run)
    {
        Goblinanim.SetFloat("Run", Run);
    }
    [ClientRpc]
     public void RpcsetAttack1Trigger()
    {
        Goblinanim.SetTrigger("Attack1");
    }
    [ClientRpc]
    public void RpcsetAttack2Trigger()
    {
        Goblinanim.SetTrigger("Attack2");
    }  
    [ClientRpc] 
    public void RpcsethitTrigger()
    {
        Goblinanim.SetTrigger("Hit");
        GetComponent<GoblinBehavior>().SetHitAnimationPlaying(true);
    }
    [ClientRpc]
    public void RpcResetHitAnimationFlag()
    {
        // Assuming SkeletonBehavior is attached to the same GameObject
        GetComponent<GoblinBehavior>().SetHitAnimationPlaying(false);
    }
    [ClientRpc]
    public void RpcsetDeathTrigger()
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
