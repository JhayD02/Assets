using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro; // Using TextMeshPro
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;

public class NetworkUI : NetworkManager
{
    [Header("UI Elements")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject hostPanel;
    [SerializeField] private GameObject clientPanel;
    [SerializeField] private GameObject serverActivePanel;
    [SerializeField] private GameObject clientActivePanel;

    [SerializeField] private GameObject StartPanel;

    [SerializeField] private TMP_InputField clientPortInput;
    [SerializeField] private TMP_InputField networkAddressInput;

    [SerializeField] private TMP_Text serverStatusText;
    [SerializeField] private TMP_Text clientStatusText;

    private ushort hostPort = 3000;
    private int maxClients = 2;
    private string hostIp = "127.0.0.1";

    private TelepathyTransport networkTransport;

    [Header("Player Prefabs")]
    [SerializeField] private GameObject playerPrefab1;
    [SerializeField] private GameObject playerPrefab2;

    void Start()
    {
        networkTransport = GetComponent<TelepathyTransport>();

        mainMenuPanel.SetActive(false);
        hostPanel.SetActive(false);
        StartPanel.SetActive(true);
        clientPanel.SetActive(false);
        serverActivePanel.SetActive(false);
        clientActivePanel.SetActive(false);
    }

    public void OpenHostPanel()
    {
        mainMenuPanel.SetActive(false);
        hostPanel.SetActive(true);
    }

    public void OpenClientPanel()
    {
        mainMenuPanel.SetActive(false);
        clientPanel.SetActive(true);
    }

    public void BackToMainMenu()
    {
        hostPanel.SetActive(false);
        clientPanel.SetActive(false);
        serverActivePanel.SetActive(false);
        clientActivePanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void StartHosting()
    {
        networkTransport.Port = hostPort;
        maxConnections = maxClients;
        StartHost();
        StartPanel.SetActive(false);

        hostPanel.SetActive(false);
        serverActivePanel.SetActive(true);

        UpdateServerStatus(GetLocalIPAddress());
    }

    public void StartClientConnection()
    {
        if (ushort.TryParse(clientPortInput.text, out hostPort))
        {
            networkTransport.Port = hostPort;
            networkAddress = networkAddressInput.text;
            StartClient();

            clientPanel.SetActive(false);
            clientActivePanel.SetActive(true);
            UpdateClientStatus();
        }
        else
        {
            Debug.LogWarning("Invalid Host Port Input!");
        }
    }

    public void StopHosting()
    {
        StopHost();
        serverActivePanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void StopClientConnection()
    {
        StopClient();
        clientActivePanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    private void UpdateServerStatus(string ipAddress)
    {
        serverStatusText.text = $"<b>Server Active</b>\nHost Port: {networkTransport.Port}\nHost IP: {hostIp}";
    }

    private void UpdateClientStatus()
    {
        clientStatusText.text = $"<b>Client Connected</b>\nHost Port: {networkTransport.Port}\nHost IP: {networkAddress}";
    }

    public void KickClient(NetworkConnectionToClient conn)
    {
        if (conn != null)
        {
            conn.Disconnect();
            Debug.Log($"Kicked client: {conn.address}");
        }
    }

    public override void OnStartHost()
    {
        base.OnStartHost();
        Debug.Log("Host started!");
    }

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);
        Debug.Log("Client Connected!");
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        Debug.Log("Disconnected from server. Returning to main menu.");
        BackToMainMenu();
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);
        Debug.Log("Client Disconnected!");
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        // Choose which player prefab to use based on some condition
        GameObject playerPrefab = (numPlayers % 2 == 0) ? playerPrefab1 : playerPrefab2;

        // Instantiate the chosen player prefab
        Transform startPos = NetworkManager.startPositions[numPlayers % 2];
        GameObject player = Instantiate(playerPrefab, startPos.position, startPos.rotation);

        // Add the player to the connection
        NetworkServer.AddPlayerForConnection(conn, player);

        // Debug log to check if the player is instantiated
        Debug.Log($"Player instantiated: {player.name}");

        // Find the CameraSript instance and add the player to its targets list
        CameraSript cameraScript = FindObjectOfType<CameraSript>();
        if (cameraScript != null)
        {
            cameraScript.AddTarget(player.transform);
            Debug.Log($"Player added to camera targets: {player.name}");
        }
        else
        {
            Debug.LogWarning("CameraSript not found!");
        }
    }

    public string GetLocalIPAddress()
    {
        IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new System.Exception("No IPv4 address found!");
    }
}