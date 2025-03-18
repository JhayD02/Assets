using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerDetection : NetworkBehaviour
{
    private BossBehavior bossBehavior;
    private FlyingBehavior flyingBehavior;
    private GoblinBehavior goblinBehavior;
    private SkeletonBehavior skeletonBehavior;
    private MushroomBehavior mushroomBehavior;

    void Start()
    {
        bossBehavior = GetComponentInParent<BossBehavior>();
        flyingBehavior = GetComponentInParent<FlyingBehavior>();
        goblinBehavior = GetComponentInParent<GoblinBehavior>();
        skeletonBehavior = GetComponentInParent<SkeletonBehavior>();
        mushroomBehavior = GetComponentInParent<MushroomBehavior>();
    }

    [ServerCallback]
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (bossBehavior != null)
                bossBehavior.SetPlayerInRange(true, other.transform);
            if (flyingBehavior != null)
                flyingBehavior.SetPlayerInRange(true, other.transform);
            if (goblinBehavior != null)
                goblinBehavior.SetPlayerInRange(true, other.transform);
            if (skeletonBehavior != null)
                skeletonBehavior.SetPlayerInRange(true, other.transform);
            if (mushroomBehavior != null)
                mushroomBehavior.SetPlayerInRange(true, other.transform);
        }
    }

    [ServerCallback]
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (bossBehavior != null)
                bossBehavior.SetPlayerInRange(false, null);
            if (flyingBehavior != null)
                flyingBehavior.SetPlayerInRange(false, null);
            if (goblinBehavior != null)
                goblinBehavior.SetPlayerInRange(false, null);
            if (skeletonBehavior != null)
                skeletonBehavior.SetPlayerInRange(false, null);
            if (mushroomBehavior != null)
                mushroomBehavior.SetPlayerInRange(false, null);
        }
    }
}