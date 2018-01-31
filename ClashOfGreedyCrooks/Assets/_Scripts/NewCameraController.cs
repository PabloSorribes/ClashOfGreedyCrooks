using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCameraController : MonoBehaviour
{
    public List<Transform> targets;

    public Vector3 offset;

    private Vector3 velocity;

    private void LateUpdate()
    {
        if (targets.Count == 0)
            return;

        Vector3 centerPoint = GetCenterPoint();

        Vector3 newPosition = centerPoint + offset;

		//Commented out this line since it returned an error -fippan
        //transform.position = Vector3.SmoothDamp(transform.position, newPosition)
    }

    Vector3 GetCenterPoint()
    {
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

}
