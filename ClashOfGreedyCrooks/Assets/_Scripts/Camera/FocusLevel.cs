using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusLevel : MonoBehaviour
{
    public float halfXBounds = 20f;
    public float halfYBounds = 15f;
    public float halfZBounds = 15f;

    public Bounds focusBounds;

    private void Update()
    {
        Vector3 position = gameObject.transform.position;
        Bounds bounds = new Bounds();
        bounds.Encapsulate(new Vector3(position.x - halfXBounds, position.y - halfYBounds, position.z - halfZBounds));
        bounds.Encapsulate(new Vector3(position.x + halfXBounds, position.y + halfYBounds, position.z + halfZBounds));
        focusBounds = bounds;
    }
}