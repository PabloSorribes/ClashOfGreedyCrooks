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

	//private Camera cam;
	private GameObject endOfRoundScreenCanvas;
	private int playersAlive;
	private PlayerInfo[] players;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		TimeManager.GetInstance.TimeIsUp += HandleEndTime;
		//cam = Camera.main;
		players = new PlayerInfo[PlayerManager.GetPlayersConnected()];
		playersAlive = players.Length;
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
		//cam.GetComponent<CameraShake>().DoShake();

		playersAlive--;
		players[playersAlive] = playerThatDied.GetComponent<PlayerInfo>();
		players[playersAlive].IsAlive = false;
		if (playersAlive <= 1)
		{
			TriggerEndOfRound();
		}
	}

	private void TriggerEndOfRound()
	{
		foreach (var player in PlayerManager.spawnedPlayers)
		{
			if (player.GetComponent<PlayerInfo>().IsAlive)
			{
				players[0] = player.GetComponent<PlayerInfo>();
				players[0].NumberOfWins++;
			}
		}

		if (GameManager.GetInstance != null)
		{
			GameManager.GetInstance.RoundsPlayed++;
		}

		for (int i = 0; i < players.Length; i++)
		{
			//TODO: Can't access PlayerInfo?

			//players[i].TotalDamage += players[i].CurrentRoundDamage;
			//players[i].TotalHits += players[i].CurrentRoundHits;
			//players[i].TotalShotsFired += players[i].CurrentRoundShotsFired;
		}

		endOfRoundScreenCanvas = Instantiate(Resources.Load("UI/EndOfRoundScreenCanvas") as GameObject);
	}

	public void ReturnToPicking()
	{
		PlayerManager.PreparePlayersForNewPickingPhase();
		GameStateManager.GetInstance.SetState(GameState.Picking);
	}
}
