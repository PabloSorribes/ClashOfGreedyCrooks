using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndOfRoundScreen : MonoBehaviour
{
	public PlayerInfo playerThatWon;

	private PlayerInfo[] connectedPlayers;
	private GameObject[] playerScoreHorizontal;

	private void Start()
	{
		connectedPlayers = PlayerManager.connectedPlayers;
		playerScoreHorizontal = new GameObject[connectedPlayers.Length];

		for (int i = 0; i < connectedPlayers.Length; i++)
		{
			playerScoreHorizontal[i] = Instantiate(Resources.Load("UI/playerScoreHorizontal") as GameObject, transform.Find("PlayerScoreBoard").GetChild(0));

			playerScoreHorizontal[i].transform.Find("Player").Find("PlayerColor").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/PlayerColors/" + connectedPlayers[i].AvatarColor);
			playerScoreHorizontal[i].transform.Find("Player").Find("PlayerAvatar").transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Avatars/" + connectedPlayers[i].AvatarSymbol);
			playerScoreHorizontal[i].transform.Find("Wins").GetComponent<Text>().text = connectedPlayers[i].NumberOfWins.ToString();
			playerScoreHorizontal[i].transform.Find("Kills").GetComponent<Text>().text = connectedPlayers[i].TotalKills.ToString();
			playerScoreHorizontal[i].transform.Find("Damage").GetComponent<Text>().text = connectedPlayers[i].TotalDamage.ToString();
			playerScoreHorizontal[i].transform.Find("Accuracy").GetComponent<Text>().text = connectedPlayers[i].Accuracy.ToString();
		}
	}
}