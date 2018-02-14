using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndOfRoundScreen : MonoBehaviour
{
	private bool hasTriggered = false;

	private void Start()
	{
		hasTriggered = false;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space) && !hasTriggered)
		{
			ArenaManager.GetInstance.ReturnToPicking();
			hasTriggered = true;
		}
	}
}