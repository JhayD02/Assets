using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // For reloading the scene
using UnityEngine.UI; // For UI elements
using Mirror; // For networking

public class WinConScript : NetworkBehaviour
{
    public GameObject losePanel; // Assign in the inspector
    public GameObject winPanel; // Assign in the inspector
    private Health playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        losePanel.SetActive(false);
        winPanel.SetActive(false);
        playerHealth = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer && playerHealth.CurrentHealth <= 0)
        {
            CmdLoseGame();
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
        Time.timeScale = 0; // Pause the game
        losePanel.SetActive(true);
    }

    [ClientRpc]
    void RpcWinGame()
    {
        Time.timeScale = 0; // Pause the game
        winPanel.SetActive(true);
    }
}