using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSript : MonoBehaviour
{
    public Vector3 offset;
    public List<Transform> targets = new List<Transform>();
    public float smoothTime = .5f;
    public float minZoom = 40;
    public float maxZoom = 10;
    public float zoomLimiter = 50;

    public Camera cam;
    private Vector3 velocity;

    void Start()
    {
        cam = GetComponent<Camera>();
        cam.orthographic = true; // Set the camera to orthographic mode
        cam.orthographicSize = 5; // Adjust the size as needed
    }

    void LateUpdate()
    {
        if (targets.Count == 0)
        {
            return;
        }

        Move();
        Zoom();
    }

    void Zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newZoom, Time.deltaTime);
    }

    void Move()
    {
        Vector3 centerPoint = GetCenterPoint();
        Vector3 newPosition = centerPoint + offset;
        newPosition.z = -10; // Ensure the camera is positioned correctly in the 2D plane
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    float GetGreatestDistance()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        return bounds.size.x;
    }

    Vector3 GetCenterPoint()
    {
        if (targets.Count == 1)
        {
            return targets[0].position;
        }

        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        return bounds.center;
    }

    public void AddTarget(Transform target)
    {
        targets.Add(target);
    }

    public void SetTarget(Transform target)
    {
        targets.Clear();
        targets.Add(target);
    }

    // New method to add multiple targets
    public void AddTargets(List<Transform> newTargets)
    {
        foreach (var target in newTargets)
        {
            targets.Add(target);
        }
    }
}