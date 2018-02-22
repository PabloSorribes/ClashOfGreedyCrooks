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

	[HideInInspector]
	private bool gameHasBeenWon = false;
	public bool roundHasEnded;
	private bool hasTriggered = false;

	private int playersAlive;
	private GameObject[] spawnedPlayers;
	private PlayerInfo[] connectedPlayers;

	private GameObject EndOfRoundScreenCanvas;


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
		{
			spawnedPlayers[i].transform.position = spawnPositionParent.transform.GetChild(i).transform.position;
			spawnedPlayers[i].transform.rotation = spawnPositionParent.transform.GetChild(i).transform.rotation;
		}

		playersAlive = spawnedPlayers.Length;
		roundHasEnded = false;
		hasTriggered = false;
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
		AudioManager.GetInstance.HandlePlayerDeath();

		TimeManager.GetInstance.StartFreezeFrame(1f);
		CameraShake.GetInstance.DoShake();

		UpdatePlayerScoreStats(playerThatDied);

		Destroy(playerThatDied.gameObject);
		playersAlive--;

		if (playersAlive <= 1)
			Invoke("TriggerEndOfRound", 0.2f);
	}

	public void UpdatePlayerScoreStats(PlayerInfo player)
	{
		for (int i = 0; i < connectedPlayers.Length; i++)
		{
			if (connectedPlayers[i].Player == player.Player)
			{
				connectedPlayers[i].totalDamage += player.totalDamage;
				connectedPlayers[i].totalHits += player.totalHits;
				connectedPlayers[i].totalKills += player.totalKills;
				connectedPlayers[i].totalShotsFired += player.totalShotsFired;
				connectedPlayers[i].numberOfWins += player.numberOfWins;

				//Debug.Log("Player " + i + ": TotalDamage: " + connectedPlayers[i].TotalDamage);
				//Debug.Log("Player " + i + ": TotalHits: " + connectedPlayers[i].TotalHits);
				//Debug.Log("Player " + i + ": TotalKills: " + connectedPlayers[i].TotalKills);
				//Debug.Log("Player " + i + ": TotalShotsFired: " + connectedPlayers[i].TotalShotsFired);
				//Debug.Log("Player " + i + ": NumberOfWins: " + connectedPlayers[i].NumberOfWins);
			}
		}
	}

	private void TriggerEndOfRound()
	{
		gameHasBeenWon = false;

		var lastPlayers = FindObjectsOfType<PlayerController>();
		Debug.Log("End Of Round Players Left: " + lastPlayers.Length);

		PlayerInfo lastPlayerAlive = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInfo>();
		lastPlayerAlive.numberOfWins++;

		UpdatePlayerScoreStats(lastPlayerAlive);

		for (int i = 0; i < connectedPlayers.Length; i++)
		{
			if (connectedPlayers[i].numberOfWins >= 3)
			{
				gameHasBeenWon = true;
				//TODO: Someone has Won the whole game
			}
		}

		if (!gameHasBeenWon)
			EndOfRoundScreenCanvas = Instantiate(Resources.Load("UI/EndOfRoundScreenCanvas") as GameObject);
		else
		{
			Debug.Log("PLAYER HAS WON");
			EndOfRoundScreenCanvas = Instantiate(Resources.Load("UI/WinScreenCanvas") as GameObject);
		}

		EndOfRoundScreenCanvas.GetComponent<EndOfRoundScreen>().playerThatWon = lastPlayerAlive;

		roundHasEnded = true;

		//TODO: Rewrite to handle this better @fippan
		DeathCircle.GetInstance.roundIsOver = true;
		DeathCircle.GetInstance.deathZoneDamage = 0;

		AudioManager.GetInstance.OnWin();

		DestroyLastPlayer();
	}

	public void NextRound()
	{
		if (!hasTriggered)
		{
			if (!gameHasBeenWon)
			{
				ReturnToPicking();
				hasTriggered = true;
			}
			else
			{
				ReturnToMainMenu();
				hasTriggered = true;
			}
		}
	}

	public void ReturnToPicking()
	{
		DestroyLastPlayer();

		PlayerManager.NextPickingPhase();
		GameStateManager.GetInstance.SetState(GameState.Picking);
	}

	public void ReturnToMainMenu()
	{
		DestroyLastPlayer();

		PlayerManager.Reset();
		GameStateManager.GetInstance.SetState(GameState.MainMenu);
	}

	private void DestroyLastPlayer()
	{
		var lastPlayersAlive = FindObjectsOfType<PlayerController>();
		Debug.Log("number of players left: " + lastPlayersAlive.Length);

		GameObject lastPlayerAlive = GameObject.FindGameObjectWithTag("Player");
		if (lastPlayerAlive != null)
		{
			//print("lastPlayerAlive: " + lastPlayerAlive.name);
			Camera.main.GetComponent<NewCameraController>().RemoveTarget(lastPlayerAlive.name);
			Debug.Log("Player Destroyed: " + lastPlayerAlive.name);
			Destroy(lastPlayerAlive);
		}
	}
}
