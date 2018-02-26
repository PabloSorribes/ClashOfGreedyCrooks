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

	private GameObject cam;
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
		cam = Camera.main.gameObject;

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

				player.AvatarColor = connectedPlayers[i].AvatarColor;
				player.AvatarSymbol = connectedPlayers[i].AvatarSymbol;
			}

		}
	}

	private void TriggerEndOfRound()
	{
		gameHasBeenWon = false;

		GameObject lastPlayerAlive = GameObject.FindGameObjectWithTag("Player");

		Debug.Log("winner: " + lastPlayerAlive.name);

		PlayerInfo lastPlayerAliveInfo = lastPlayerAlive.GetComponent<PlayerInfo>();
		lastPlayerAliveInfo.numberOfWins++;

		UpdatePlayerScoreStats(lastPlayerAliveInfo);

		for (int i = 0; i < connectedPlayers.Length; i++)
			if (connectedPlayers[i].numberOfWins >= 1)
				gameHasBeenWon = true;

		GameObject.Find("CameraHolder").transform.GetChild(0).GetComponent<NewCameraController>().OnVictory(lastPlayerAlive.transform);
		StartCoroutine(ShowEndScreen(lastPlayerAlive, lastPlayerAliveInfo));

		//TODO: Rewrite to handle this better @fippan
		DeathCircle.GetInstance.roundIsOver = true;
		DeathCircle.GetInstance.deathZoneDamage = 0;

		AudioManager.GetInstance.OnWin();
	}

	IEnumerator ShowEndScreen(GameObject lastPlayerAlive, PlayerInfo lastPlayerAliveInfo)
	{
		yield return new WaitForSeconds(3f);

		if (!gameHasBeenWon)
		{
			EndOfRoundScreenCanvas = Instantiate(Resources.Load("UI/EndOfRoundScreenCanvas") as GameObject);
			EndOfRoundScreenCanvas.GetComponent<EndOfRoundScreen>().SetRoundWinner(lastPlayerAliveInfo.AvatarColor, lastPlayerAliveInfo.AvatarSymbol,
				lastPlayerAlive.GetComponentInChildren<Champion>().name, lastPlayerAliveInfo.Player);
		}
		else
		{
			EndOfRoundScreenCanvas = Instantiate(Resources.Load("UI/EndOfGame") as GameObject);
			Invoke("EndOfGameScore", 3f);
		}

		roundHasEnded = true;
	}

	private void EndOfGameScore()
	{
		EndOfRoundScreenCanvas.transform.Find("Background").gameObject.SetActive(true);
		EndOfRoundScreenCanvas.transform.Find("PlayerScoreBoard").gameObject.SetActive(true);
		EndOfRoundScreenCanvas.GetComponent<EndOfRoundScreen>().SetEndOfGame();
	}

	public void NextRound()
	{
		DestroyLastPlayers();
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
		DestroyLastPlayers();

		PlayerManager.NextPickingPhase();
		GameStateManager.GetInstance.SetState(GameState.Picking);
	}

	public void ReturnToMainMenu()
	{
		DestroyLastPlayers();

		GameStateManager.GetInstance.SetState(GameState.MainMenu);
	}

	public void DestroyLastPlayers()
	{
		GameObject[] lastPlayersAlive = GameObject.FindGameObjectsWithTag("Player");

		if (lastPlayersAlive.Length != 0)
		{
			foreach (GameObject player in lastPlayersAlive)
			{
				if (GameStateManager.GetInstance.GetState() == GameState.Arena)
					cam.GetComponent<NewCameraController>().RemoveTarget(player.name);

				Destroy(player);
			}

		}
	}
}
