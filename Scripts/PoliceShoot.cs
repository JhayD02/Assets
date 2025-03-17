using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceShoot : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform bulletLocation;
    void OnEnable()
    {
        Instantiate(bulletPrefab, bulletLocation.position, bulletLocation.rotation);
    }
}
