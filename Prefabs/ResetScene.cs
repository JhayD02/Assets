using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetScene : MonoBehaviour
{
    private List<Vector3> initialPositions = new List<Vector3>();
    private List<Quaternion> initialRotations = new List<Quaternion>();
    private List<GameObject> enemies = new List<GameObject>();

    void Start()
    {
        // Find all enemies and store their initial positions and rotations
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            initialPositions.Add(enemy.transform.position);
            initialRotations.Add(enemy.transform.rotation);
            enemies.Add(enemy);
        }
    }

    public void RespawnEnemies(GameObject enemyPrefab)
    {
        // Destroy all existing enemies
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }

        // Clear the enemies list
        enemies.Clear();

        // Respawn enemies at their original positions
        for (int i = 0; i < initialPositions.Count; i++)
        {
            GameObject newEnemy = Instantiate(enemyPrefab, initialPositions[i], initialRotations[i]);
            enemies.Add(newEnemy);
        }
    }
}