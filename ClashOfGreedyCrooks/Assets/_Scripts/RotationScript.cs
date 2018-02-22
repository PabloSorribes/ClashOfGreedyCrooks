using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationScript : MonoBehaviour {


	// Update is called once per frame
	void Update () {
		transform.eulerAngles = transform.eulerAngles += Vector3.up * 10;
	}
}
