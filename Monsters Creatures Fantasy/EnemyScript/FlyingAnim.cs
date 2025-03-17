using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingAnim : MonoBehaviour
{
    Animator Flyinganim;
    void Start()
    {
        Flyinganim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    //    if(Input.GetButtonDown("Fire1"))
    //     {
    //         Flyinganim.SetTrigger("Attack1");
    //     }
    //    if(Input.GetButtonDown("Fire2"))
    //     {
    //        Flyinganim.SetTrigger("Attack2");
    //     }
    }

      public void setAttack1Trigger()
    {
        Flyinganim.SetTrigger("Attack1");
    }
    public void setAttack2Trigger()
    {
        Flyinganim.SetTrigger("Attack2");
    }   
    public void sethitTrigger()
    {
        Flyinganim.SetTrigger("Hit");
    }
    public void setDeathTrigger()
    {
        Flyinganim.SetTrigger("Death");
    }


    public bool isAttack1()
    {
        return Flyinganim.GetCurrentAnimatorStateInfo(0).IsName("Attack1");
    }
}
