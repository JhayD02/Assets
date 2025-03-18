using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class FireballScript : NetworkBehaviour
{
    public float speed = 7.5f;
    public float life = 5f;

    [SyncVar] // Syncs across all clients
    private Vector3 targetPosition;

    public void SetTargetPosition(Vector3 position)
    {
        if (isServer)
        {
            targetPosition = position;
        }
        else
        {
            CmdSetTargetPosition(position);
        }
    }

    [Command]
    private void CmdSetTargetPosition(Vector3 position)
    {
        targetPosition = position;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (transform.position == targetPosition)
        {
            NetworkServer.Destroy(gameObject);
        }
    }

    public override void OnStartServer()
    {
        StartCoroutine(DestroyAfterTime(life));
    }

    private IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        NetworkServer.Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            NetworkServer.Destroy(gameObject);
        }
    }
}