using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // For reloading the scene
using UnityEngine.UI; // For UI elements

public class WinConScript : MonoBehaviour
{
    public GameObject losePanel; // Assign in the inspector
    public GameObject winPanel; // Assign in the inspector
    public int playerHealth = 100; // Example player health

    // Start is called before the first frame update
    void Start()
    {
        losePanel.SetActive(false);
        winPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth <= 0)
        {
            LoseGame();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("WinConBox"))
        {
            WinGame();
        }
    }

    void LoseGame()
    {
        Time.timeScale = 0; // Pause the game
        losePanel.SetActive(true);
    }

    void WinGame()
    {
        Time.timeScale = 0; // Pause the game
        winPanel.SetActive(true);
    }
}