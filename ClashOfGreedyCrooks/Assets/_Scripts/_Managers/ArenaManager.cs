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

	private int playersAlive;
	private PlayerInfo[] players;


	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		TimeManager.GetInstance.TimeIsUp += HandleEndTime;

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

		GameManager.GetInstance.RoundsPlayed++;

		for (int i = 0; i < players.Length; i++)
		{
			players[i].TotalDamage += players[i].CurrentRoundDamage;
			players[i].TotalHits += players[i].CurrentRoundHits;
			players[i].TotalShotsFired += players[i].CurrentRoundShotsFired;
		}

		//TODO: Instantiate EndOfGameScreen to display score etc.

		Debug.Log("RETURNING TO PICKING PHASE IN 5 SECONDS");
		Invoke("ReturnToPicking", 5f);
	}

	private void ReturnToPicking()
	{
		PlayerManager.PreparePlayersForNewPickingPhase();
		GameStateManager.GetInstance.SetState(GameState.Picking);
	}
}
