using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetScene : MonoBehaviour
{

    public GameObject objectPrefab; // Assign the prefab in the Inspector

    public void ResetObject()
    {
        Vector3 spawnPosition = transform.position;
        Quaternion spawnRotation = transform.rotation;

        Destroy(gameObject); // Destroy the current object
        Instantiate(objectPrefab, spawnPosition, spawnRotation); // Spawn a new one
    }
}
