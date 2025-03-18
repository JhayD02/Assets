using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    public float disappearTime = 3f; // Time in seconds before disappearing and reappearing
    public float reappearTime = 3f; // Time in seconds before reappearing

    void Start()
    {
        InvokeRepeating("Disappear", disappearTime, disappearTime + reappearTime);
    }

    void Disappear()
    {
        gameObject.SetActive(false);
        Invoke("Reappear", reappearTime);
    }

    void Reappear()
    {
        gameObject.SetActive(true);
    }
}