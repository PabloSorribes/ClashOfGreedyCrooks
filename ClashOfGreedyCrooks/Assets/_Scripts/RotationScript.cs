using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationScript : MonoBehaviour {

	FMODUnity.StudioEventEmitter a_buoyRotation;

	private void Start()
	{
		a_buoyRotation = AudioManager.GetInstance.InitializeAudioOnObject(this.gameObject, "event:/Arena/buoyLoop");
		a_buoyRotation.Play();
	}

	// Update is called once per frame
	void Update () {
		transform.eulerAngles = transform.eulerAngles += Vector3.up * 20;
	}

	private void OnDestroy()
	{
		a_buoyRotation.Stop();
	}
}
