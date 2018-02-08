using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    private float time;
    public TimeManager tMan;
    public DeathCircle dCirc;

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
        tMan.TimeIsUp += HandleEndTime;
		//TimeManager.TimeIsUp += HandleEndTime;
	}

	public void HandleEndTime()
	{
		ShrinkDeathCircle();
	}

	private void ShrinkDeathCircle()
	{
        dCirc.ChangeSize(true);
		//gameObject.GetComponent<DeathCircle>().ChangeSize(true);
	}

	public void HandlePlayerDeath(GameObject playerThatDied)
	{
		TimeManager.GetInstance.StartFreezeFrame(1f);

    }


}
