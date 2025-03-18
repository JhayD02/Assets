using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SkeletonAnim : NetworkBehaviour
{
    Animator skeletonanim;
    [SyncVar(hook = nameof(OnWalkChanged))]
    private float walkSpeed = 0f;

    void Start()
    {
        skeletonanim = GetComponent<Animator>();
    }



    public void SetWalk(float walk)
    {
        if (!isServer) return; 

        walkSpeed = walk; 
    }

    void OnWalkChanged(float oldVal, float newVal)
    {
        skeletonanim.SetFloat("Walk", newVal);
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
    public void RpcsethitTrigger()
    {
        skeletonanim.SetTrigger("Hit");
        GetComponent<SkeletonBehavior>().SetHitAnimationPlaying(true);
    }

    [ClientRpc]
    public void RpcResetHitAnimationFlag()
    {
        GetComponent<SkeletonBehavior>().SetHitAnimationPlaying(false);
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