using UnityEngine;
using Mirror;

public class NetworkedCameraController : NetworkBehaviour
{
    private CameraSript cameraScript;

    void Start()
    {
        if (isLocalPlayer)
        {
            cameraScript = FindObjectOfType<CameraSript>();
            cameraScript.enabled = true;
        }
        else
        {
            enabled = false;
        }
    }

    [ClientRpc]
    public void RpcUpdateCamera(Vector3 position, float zoom)
    {
        if (!isLocalPlayer)
        {
            cameraScript.transform.position = position;
            cameraScript.cam.orthographicSize = zoom;
        }
    }

    public void UpdateCamera(Vector3 position, float zoom)
    {
        if (isLocalPlayer)
        {
            RpcUpdateCamera(position, zoom);
        }
    }
}