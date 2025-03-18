using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraCC : MonoBehaviour
{
    public Camera mainMenuCamera;
    public Camera playerCamera;

    // Start is called before the first frame update
    void Start()
    {
        // Ensure the main menu camera is active at the start
        SwitchToMainMenuCamera();
    }

    // Update is called once per frame
    void Update()
    {
        // Example of switching cameras based on a key press
        if (Input.GetKeyDown(KeyCode.M))
        {
            SwitchToMainMenuCamera();
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            SwitchToPlayerCamera();
        }
    }

    public void SwitchToMainMenuCamera()
    {
        mainMenuCamera.gameObject.SetActive(true);
        playerCamera.gameObject.SetActive(false);
    }

    public void SwitchToPlayerCamera()
    {
        mainMenuCamera.gameObject.SetActive(false);
        playerCamera.gameObject.SetActive(true);
    }
}