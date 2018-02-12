using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FmodTestScript : MonoBehaviour
{

	// Use this for initialization
	void Start()
	{
		//TODO: Play with get/set-variables in audio manager
		//AudioManager.GetInstance.MusicMainMenu.
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha8))
		{
			//AudioManager.GetInstance.ChangeParameter();
		}

		if (Input.GetKeyDown(KeyCode.Alpha9))
		{
			//AudioManager.GetInstance.ChangeParameter(0);
		}
	}
}
