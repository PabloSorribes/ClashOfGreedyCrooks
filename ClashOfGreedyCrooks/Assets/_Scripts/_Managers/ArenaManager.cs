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

    private void Awake()
    {
        instance = this;
    }

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
        DeathCircle.GetInstance.ChangeSize(true);
	}

	public void HandlePlayerDeath(GameObject playerThatDied)
	{
		TimeManager.GetInstance.StartFreezeFrame(1f);

        CameraShake.GetInstance.DoShake();
    }
}
