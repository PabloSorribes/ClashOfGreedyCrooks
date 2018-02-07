using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class NewCameraController : MonoBehaviour
{
    public List<Transform> targets;

    public Vector3 offset;
    public float smoothTime = .5f;

    public float minZoom = 40f;
    public float maxZoom = 15f;
    public float zoomLimiter = 50f;

    private Vector3 velocity;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        foreach (GameObject player in PlayerManager.spawnedPlayers)
        {
            targets.Add(player.transform);
        }
    }

    private void LateUpdate()
    {
        if (targets.Count == 0)
            return;

        Move();
        Zoom();
 
    }

    void Move()
    {
        Vector3 centerPoint = GetCenterPoint();

        Vector3 newPosition = centerPoint + offset;

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    Vector3 GetCenterPoint()
    {
        //Same position as the only player left.
        if(targets.Count == 1)
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

    void Zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);
        
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }

    float GetGreatestDistance()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        if (bounds.size.x > bounds.size.z)
        {
            return bounds.size.x;
        }
        else
            return bounds.size.z;

        

    }


}
