using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro; // Using TextMeshPro
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;
using UnityEngine.SceneManagement; // Add this line

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

    [SerializeField] private TMP_Text serverStatusText;
    [SerializeField] private TMP_Text clientStatusText;

    private ushort hostPort = 3000;
    private int maxClients = 2;
    private string hostIp = "127.0.0.1";

    private kcp2k.KcpTransport networkTransport;

    void Start()
    {
        networkTransport = GetComponent<kcp2k.KcpTransport>();

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
        // Host will load the game scene
        SceneManager.LoadScene("GameScene");
    }

    public void StartClientConnection()
    {
        if (ushort.TryParse(clientPortInput.text, out hostPort))
        {
            networkTransport.Port = hostPort;
            networkAddress = hostIp; // Use the default IP address
            Debug.Log($"Attempting to connect to {networkAddress}:{hostPort}");
            StartClient();
        }
        else
        {
            Debug.LogWarning("Invalid Host Port Input!");
        }
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        // Client will load the game scene upon successful connection
        SceneManager.LoadScene("GameScene");
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

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}