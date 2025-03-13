using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkMang : MonoBehaviour
{
    // Start is called before the first frame updat[Header("UI Elements")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject hostPanel;
    [SerializeField] private GameObject clientPanel;
    [SerializeField] private GameObject serverActivePanel;
    [SerializeField] private GameObject clientActivePanel;

    [SerializeField] private GameObject StartPanel;


    private ushort hostPort = 3000;
    private int maxClients = 2;
    private string hostIp = "127.0.0.1";

    public void start()
    {
        mainMenuPanel.SetActive(false);
        hostPanel.SetActive(false);
        StartPanel.SetActive(true);
        clientPanel.SetActive(false);
        serverActivePanel.SetActive(false);
        clientActivePanel.SetActive(false);
    }

    public void OnstartPanel()
    {
        mainMenuPanel.SetActive(true);
    }

    public void OpenHostPanel()
    {
        mainMenuPanel.SetActive(false);
        hostPanel.SetActive(true);
    }
}
