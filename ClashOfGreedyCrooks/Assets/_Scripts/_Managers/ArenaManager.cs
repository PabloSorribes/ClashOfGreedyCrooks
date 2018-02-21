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
		AudioManager.GetInstance.HandleWin();

		TimeManager.GetInstance.StartFreezeFrame(1f);
		CameraShake.GetInstance.DoShake();

		UpdatePlayerScoreStats(playerThatDied);

		playersAlive--;

		if (playersAlive <= 1)
			TriggerEndOfRound();
	}

	public void UpdatePlayerScoreStats(PlayerInfo player)
	{
		for (int i = 0; i < connectedPlayers.Length; i++)
		{
			if (connectedPlayers[i].Player == player.Player)
			{
				connectedPlayers[i].TotalDamage = player.TotalDamage;
				connectedPlayers[i].TotalHits = player.TotalHits;
				connectedPlayers[i].TotalKills = player.TotalKills;
				connectedPlayers[i].TotalShotsFired = player.TotalShotsFired;
				connectedPlayers[i].NumberOfWins = player.NumberOfWins;
			}
		}

	}

	private void TriggerEndOfRound()
	{
		bool gameHasBeenWon = false;
		PlayerInfo lastPlayerAlive = FindObjectOfType<PlayerInfo>();
		lastPlayerAlive.NumberOfWins++;

		UpdatePlayerScoreStats(lastPlayerAlive);

		for (int i = 0; i < connectedPlayers.Length; i++)
		{
			if (connectedPlayers[i].NumberOfWins >= 3)
			{
				gameHasBeenWon = true;
				//TODO: Someone has Won the whole game
			}
		}

		if (!gameHasBeenWon)
			EndOfRoundScreenCanvas = Instantiate(Resources.Load("UI/EndOfRoundScreenCanvas") as GameObject);
		else
			Debug.Log("PLAYER HAS WON");
		//EndOfRoundScreenCanvas = Instantiate(Resources.Load("UI/WinScreenCanvas") as GameObject);

		EndOfRoundScreenCanvas.GetComponent<EndOfRoundScreen>().playerThatWon = lastPlayerAlive;

		roundHasEnded = true;

		//TODO: Rewrite to handle this better @fippan
		DeathCircle.GetInstance.roundIsOver = true;
		DeathCircle.GetInstance.deathZoneDamage = 0;

		DestroyLastPlayer();
	}

	public void NextRound()
	{
		if (!hasTriggered)
		{
			ReturnToPicking();
			hasTriggered = true;
		}
	}

	public void ReturnToPicking()
	{
		//DestroyLastPlayer();

		PlayerManager.NextPickingPhase();
		GameStateManager.GetInstance.SetState(GameState.Picking);
	}

	public void ReturnToMainMenu()
	{
		//DestroyLastPlayer();

		PlayerManager.Reset();
		GameStateManager.GetInstance.SetState(GameState.MainMenu);
	}

	private void DestroyLastPlayer()
	{
		GameObject lastPlayerAlive = GameObject.FindGameObjectWithTag("Player");
		if (lastPlayerAlive != null)
		{
			Camera.main.GetComponent<NewCameraController>().RemoveTarget(lastPlayerAlive.name);

			Destroy(lastPlayerAlive);
		}
	}
}
