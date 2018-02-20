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

	private GameObject spawnPositionParent;

	private int playersAlive;
	private GameObject[] spawnedPlayers;
	private PlayerInfo[] connectedPlayers;


	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		TimeManager.GetInstance.TimeIsUp += HandleEndTime;

		spawnPositionParent = GameObject.Find("SpawnPoints");
		spawnedPlayers = PlayerManager.spawnedPlayers;
		connectedPlayers = PlayerManager.connectedPlayers;

		//Set spawnpoints for each player
		for (int i = 0; i < spawnedPlayers.Length; i++)
			spawnedPlayers[i].transform.position = spawnPositionParent.transform.GetChild(i).transform.position;

		playersAlive = spawnedPlayers.Length;
	}

	public void HandleEndTime()
	{
		ShrinkDeathCircle();
	}

	private void ShrinkDeathCircle()
	{
		DeathCircle.GetInstance.ChangeSize(true);
	}

	public void HandlePlayerDeath(PlayerInfo playerThatDied)
	{
		TimeManager.GetInstance.StartFreezeFrame(1f);
		CameraShake.GetInstance.DoShake();

		UpdatePlayerScoreStats(playerThatDied);

		playersAlive--;

		if (playersAlive <= 1)
			TriggerEndOfRound();
	}

	public void UpdatePlayerScoreStats(PlayerInfo player)
	{
		player.TotalDamage += player.CurrentRoundDamage;
		player.TotalHits += player.CurrentRoundHits;
		player.TotalShotsFired += player.CurrentRoundShotsFired;

		//Modify to fit with jims handling of PlayerInfo
		//Copy PlayerInfo from player to connectedPlayers Array in PlayerManager
		connectedPlayers[playersAlive] = player;
	}

	private void TriggerEndOfRound()
	{
		PlayerInfo lastPlayerAlive = FindObjectOfType<PlayerInfo>();
		lastPlayerAlive.NumberOfWins++;

		UpdatePlayerScoreStats(lastPlayerAlive);

		for (int i = 0; i < connectedPlayers.Length; i++)
		{
			if (connectedPlayers[i].NumberOfWins >= 3)
			{
				//TODO: Someone has Won
			}
		}

		Instantiate(Resources.Load("UI/EndOfRoundScreenCanvas") as GameObject);

		//TODO: Rewrite to handle this better @fippan
		DeathCircle.GetInstance.roundIsOver = true;
		DeathCircle.GetInstance.deathZoneDamage = 0;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.I))
		{
			ReturnToPicking();
		}
	}

	public void ReturnToPicking()
	{
		GameObject lastPlayerAlive = GameObject.FindGameObjectWithTag("Player");
		if (lastPlayerAlive != null)
		{
			Camera.main.GetComponent<NewCameraController>().RemoveTarget(lastPlayerAlive.name);

			Destroy(lastPlayerAlive);
		}

		PlayerManager.NextPickingPhase();

		GameStateManager.GetInstance.SetState(GameState.Picking);
	}

	public void ReturnToMainMenu()
	{
		//TODO: Code to reset arrays and go back to main menu after a game is finished with one player having 3 wins.
	}
}
