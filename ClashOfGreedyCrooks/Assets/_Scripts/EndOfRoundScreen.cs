using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndOfRoundScreen : MonoBehaviour
{
	private bool hasTriggered = false;

	public PlayerInfo playerThatWon;

	private PlayerInfo[] connectedPlayers;
	private GameObject[] playerScoreHorizontal;

	private void Start()
	{
		connectedPlayers = PlayerManager.connectedPlayers;
		playerScoreHorizontal = new GameObject[connectedPlayers.Length];

		for (int i = 0; i < connectedPlayers.Length; i++)
		{
			//GameObject tempPlayerScore = Resources.Load("UI/playerScoreHorizontal") as GameObject;
			playerScoreHorizontal[i] = Instantiate(Resources.Load("UI/playerScoreHorizontal") as GameObject, transform.Find("PlayerScoreBoard").GetChild(0));

			playerScoreHorizontal[i].transform.Find("PlayerColor").transform.GetComponent<Image>().sprite = Resources.Load("UI/Avatars" + connectedPlayers[i].AvatarSymbol) as Sprite; 
			playerScoreHorizontal[i].transform.Find("PlayerColor").GetChild(0).GetComponent<Image>().sprite = Resources.Load("UI/PlayerColors" + connectedPlayers[i].AvatarColor) as Sprite;
			playerScoreHorizontal[i].transform.Find("Wins").GetComponent<Text>().text = connectedPlayers[i].NumberOfWins.ToString();
			playerScoreHorizontal[i].transform.Find("Kills").GetComponent<Text>().text = connectedPlayers[i].TotalKills.ToString();
			playerScoreHorizontal[i].transform.Find("Damage").GetComponent<Text>().text = connectedPlayers[i].TotalDamage.ToString();
			playerScoreHorizontal[i].transform.Find("Accuracy").GetComponent<Text>().text = connectedPlayers[i].Accuracy.ToString();
			//playerScoreHorizontal[i].transform.Find("Accuracy").GetComponent<Text>().text = (connectedPlayers[i].TotalShotsFired / connectedPlayers[i].TotalHits).ToString();

		}


		hasTriggered = false;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			NextRound();
		}
	}

	public void NextRound()
	{
		if (!hasTriggered)
		{
			ArenaManager.GetInstance.ReturnToPicking();
			hasTriggered = true;
		}
	}
}