using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAnim : MonoBehaviour
{
    Animator skeletonanim;
    void Start()
    {
        skeletonanim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
         if(Input.GetButtonDown("Fire1"))
        {
            skeletonanim.SetTrigger("Attack1");
        }
        if(Input.GetButtonDown("Fire2"))
        {
            skeletonanim.SetTrigger("Attack2");
        }
    }

    public void SetWalk(float walk)
    {
        skeletonanim.SetFloat("Walk", walk);
    }
    public void setAttack1Trigger()
    {
        skeletonanim.SetTrigger("Attack1");
    }
    public void setAttack2Trigger()
    {
        skeletonanim.SetTrigger("Attack2");
    }   
    public void sethitTrigger()
    {
        skeletonanim.SetTrigger("Hit");
    }
    public void setDeathTrigger()
    {
        skeletonanim.SetTrigger("Death");
    }
}
