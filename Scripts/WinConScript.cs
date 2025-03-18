using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // For reloading the scene
using UnityEngine.UI; // For UI elements
using Mirror; // For networking

public class WinConScript : NetworkBehaviour
{
    public GameObject losePanel; 
    public GameObject winPanel; 
    private Health playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        if (losePanel == null)
        {
            Debug.LogError("Lose panel not assigned in the inspector.");
        }
        else
        {
            losePanel.SetActive(false);
        }

        if (winPanel == null)
        {
            Debug.LogError("Win panel not assigned in the inspector.");
        }
        else
        {
            winPanel.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth == null)
        {
            FindLocalPlayer();
        }

        if (playerHealth != null && playerHealth.CurrentHealth <= 0)
        {
            if (isLocalPlayer)
            {
                Debug.Log("Calling CmdLoseGame");
                CmdLoseGame();
            }
        }

        // Check for key presses to show panels
        if (Input.GetKeyDown(KeyCode.O))
        {
            ShowLosePanel();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            ShowWinPanel();
        }
    }

    void FindLocalPlayer()
    {
        GameObject localPlayer = GameObject.FindWithTag("Player");
        if (localPlayer != null)
        {
            playerHealth = localPlayer.GetComponent<Health>();
            if (playerHealth == null)
            {
                Debug.LogError("Health component not found on the local player.");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (isLocalPlayer && other.gameObject.CompareTag("WinConBox"))
        {
            CmdWinGame();
        }
    }

    [Command]
    public void CmdLoseGame()
    {
        Debug.Log("CmdLoseGame called");
        RpcLoseGame();
    }

    [Command]
    public void CmdWinGame()
    {
        RpcWinGame();
    }

    [ClientRpc]
    void RpcLoseGame()
    {
        Debug.Log("RpcLoseGame called");
        Time.timeScale = 0;
        if (losePanel != null)
        {
            losePanel.SetActive(true);
            Debug.Log("losePanel set active");
        }
        else
        {
            Debug.LogError("losePanel is null");
        }
    }

    [ClientRpc]
    void RpcWinGame()
    {
        Time.timeScale = 0; // Pause the game
        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }
    }

    void ShowLosePanel()
    {
        Time.timeScale = 0; // Pause the game
        if (losePanel != null)
        {
            losePanel.SetActive(true);
            Debug.Log("losePanel set active");
        }
        else
        {
            Debug.LogError("losePanel is null");
        }
    }

    void ShowWinPanel()
    {
        Time.timeScale = 0; // Pause the game
        if (winPanel != null)
        {
            winPanel.SetActive(true);
            Debug.Log("winPanel set active");
        }
        else
        {
            Debug.LogError("winPanel is null");
        }
    }
}