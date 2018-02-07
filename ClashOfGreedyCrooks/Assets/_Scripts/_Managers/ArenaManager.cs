using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : MonoBehaviour
{
	private static ArenaManager instance;
	public static ArenaManager GetInstance
	{
		get
		{
			return instance;
		}
	}

	private float time;

	private void Start()
	{
		TimeManager.GetInstance.TimeIsUp += HandleEndTime;
	}



	public void HandleEndTime()
	{
		ShrinkDeathCircle();
	}

	private void ShrinkDeathCircle()
	{
		gameObject.GetComponent<DeathCircle>().ChangeSize(true);
	}

	public void HandlePlayerDeath(GameObject playerThatDied)
	{
		TimeManager.GetInstance.StartFreezeFrame(1f);
	}


}
