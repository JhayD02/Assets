using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    private MushroomBehavior mushroomBehavior;  
    private GoblinBehavior goblinBehavior;
    private SkeletonBehavior skeletonBehavior;
    private FlyingBehavior flyingBehavior;
    private BossBehavior bossBehavior;

    void Start()
    {
        skeletonBehavior = GetComponentInParent<SkeletonBehavior>();
        mushroomBehavior = GetComponentInParent<MushroomBehavior>();
        goblinBehavior = GetComponentInParent<GoblinBehavior>();
        flyingBehavior = GetComponentInParent<FlyingBehavior>();
        bossBehavior = GetComponentInParent<BossBehavior>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered detection range");
            if (goblinBehavior != null)
                goblinBehavior.SetPlayerInRange(true, other.transform);
            if (skeletonBehavior != null)
                skeletonBehavior.SetPlayerInRange(true, other.transform);
            if (mushroomBehavior != null)
                mushroomBehavior.SetPlayerInRange(true, other.transform);
            if (flyingBehavior != null)
                flyingBehavior.SetPlayerInRange(true, other.transform);
            if (bossBehavior != null)
                bossBehavior.SetPlayerInRange(true, other.transform);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited detection range");
            if (goblinBehavior != null)
                goblinBehavior.SetPlayerInRange(false, null);
            if (skeletonBehavior != null)
                skeletonBehavior.SetPlayerInRange(false, null);
            if (mushroomBehavior != null)
                mushroomBehavior.SetPlayerInRange(false, null);
            if (flyingBehavior != null)
                flyingBehavior.SetPlayerInRange(false, null);
            if (bossBehavior != null)
                bossBehavior.SetPlayerInRange(false, null);
        }
    }
}