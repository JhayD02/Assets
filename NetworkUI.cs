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

    [SerializeField] private TMP_InputField hostPortInput;
    [SerializeField] private TMP_InputField clientPortInput;
    [SerializeField] private TMP_InputField hostIpInput;
    [SerializeField] private TMP_InputField maxClientsInput;

    [SerializeField] private TMP_Text serverStatusText;
    [SerializeField] private TMP_Text clientStatusText;
    [SerializeField] private Transform clientListContainer;
    [SerializeField] private GameObject clientListItemPrefab; // Prefab for client UI

    private ushort hostPort;
    private int maxClients;
    private string hostIp;

    private kcp2k.KcpTransport networkTransport;
    private List<NetworkConnectionToClient> clients = new List<NetworkConnectionToClient>();

    void Start()
    {
        networkTransport = GetComponent<kcp2k.KcpTransport>();

        mainMenuPanel.SetActive(true);
        hostPanel.SetActive(false);
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
        if (ushort.TryParse(hostPortInput.text, out hostPort) && int.TryParse(maxClientsInput.text, out maxClients))
        {
            networkTransport.Port = hostPort;
            maxConnections = maxClients;
            StartHost();

            hostPanel.SetActive(false);
            serverActivePanel.SetActive(true);
            UpdateServerStatus();
        }
        else
        {
            Debug.LogWarning("Invalid Host Port or Max Clients Input!");
        }
    }

    public void StartClientConnection()
    {
        if (ushort.TryParse(clientPortInput.text, out hostPort))
        {
            networkTransport.Port = hostPort;
            networkAddress = hostIpInput.text;
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

    private void UpdateServerStatus()
    {
        serverStatusText.text = $"<b>Server Active</b>\nHost Port: {networkTransport.Port}\nHost IP: {GetLocalIPAddress()}";
        UpdateClientList();
    }

    private void UpdateClientStatus()
    {
        
        clientStatusText.text = $"<b>Client Connected</b>\nHost Port: {networkTransport.Port}\nHost IP: {networkAddress}";
    }

    private void UpdateClientList()
    {
        foreach (Transform child in clientListContainer)
        {
            Destroy(child.gameObject);
        }

        int index = 0;
        foreach (var conn in clients)
        {
            GameObject clientItem = Instantiate(clientListItemPrefab, clientListContainer);
            TMP_Text clientText = clientItem.transform.Find("ClientText").GetComponent<TMP_Text>();
            Button disconnectButton = clientItem.transform.Find("DisconnectButton").GetComponent<Button>();

            clientText.text = conn.address;
            disconnectButton.onClick.AddListener(() => KickClient(conn));

            // Set local UI position inside the canvas
            RectTransform itemRect = clientItem.GetComponent<RectTransform>();
            itemRect.anchoredPosition = new Vector2(216, 253 - (index * 50)); // Adjust Y for spacing
            index++;
        }
    }

    public void KickClient(NetworkConnectionToClient conn)
    {
        if (conn != null)
        {
            conn.Disconnect();
            clients.Remove(conn);
            UpdateClientList();
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
        clients.Add(conn);
        base.OnServerConnect(conn);
        UpdateClientList();
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
        clients.Remove(conn);
        base.OnServerDisconnect(conn);
        UpdateClientList();
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
}
