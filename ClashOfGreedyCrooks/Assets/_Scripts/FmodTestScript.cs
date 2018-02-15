using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FmodTestScript : MonoBehaviour
{
	// Use this for initialization
	void Start()
	{
		//AudioManager.GetInstance.MuteAudio(AudioManager.AudioBusses.mainBus, true);
		//AudioManager.GetInstance.SetAudioVolume(AudioManager.AudioBusses.mainBus, -0.15f);
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha8))
		{
			//AudioManager.GetInstance.SetAudioVolume(AudioManager.AudioBusses.musicBus, -0.15f);
		}

		if (Input.GetKeyDown(KeyCode.Alpha9))
		{
			//AudioManager.GetInstance.SetAudioVolume(AudioManager.AudioBusses.musicBus, 0.15f);
		}
	}
}
