using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Instantiate : NetworkBehaviour
{
    public GameObject enemyPrefab; 
    public Button startButton;    

    void Start()
    {
        if (startButton != null)
        {
            startButton.onClick.AddListener(SpawnEnemy);
        }
    }

    [Server] // Only the server should spawn enemies
    public void SpawnEnemy()
    {
        Debug.Log("SpawnEnemy method called."); // Debug log

        if (!isServer)
        {
            Debug.LogWarning("SpawnEnemy called on a client, not the server."); // Debug log
            return; // Ensure only the server executes this
        }

        if (enemyPrefab == null)
        {
            Debug.LogError("Enemy prefab is missing! Assign it in the Inspector.");
            return;
        }

        // Instantiate the enemy at its default prefab position
        GameObject enemy = Instantiate(enemyPrefab);
        Debug.Log("Enemy instantiated."); // Debug log

        // Ensure the prefab has a NetworkIdentity
        if (enemy.GetComponent<NetworkIdentity>() == null)
        {
            Debug.LogError("Enemy prefab is missing a NetworkIdentity! Add one in the prefab.");
            return;
        }

        // Spawn the enemy for all clients
        NetworkServer.Spawn(enemy);
        Debug.Log("Enemy spawned on the network."); // Debug log
    }
}