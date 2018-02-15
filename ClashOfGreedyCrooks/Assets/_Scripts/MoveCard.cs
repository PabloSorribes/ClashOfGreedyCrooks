using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCard : MonoBehaviour {

    public Vector3 target;

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target, .1f);
    }
}
