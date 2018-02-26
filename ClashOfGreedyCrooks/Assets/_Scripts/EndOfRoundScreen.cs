using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndOfRoundScreen : MonoBehaviour
{
	private string winnerColor;
	private string winnerAvatar;
	private string winnerName;
	private int winnerPlayerIndex;

    private bool endOfGame;

	private PlayerInfo[] connectedPlayers;
	private GameObject[] playerScoreHorizontal;

	public void SetRoundWinner(string winnerColor, string winnerAvatar, string winnerName, int winnerPlayerIndex)
	{
		this.winnerColor = winnerColor;
		this.winnerAvatar = winnerAvatar;
		this.winnerName = winnerName;
		this.winnerPlayerIndex = winnerPlayerIndex;
        
		FillScoreScreen();
	}

    public void SetEndOfGame()
    {
        endOfGame = true;
        FillScoreScreen();
    }

	private void FillScoreScreen()
	{
		connectedPlayers = PlayerManager.connectedPlayers;
		playerScoreHorizontal = new GameObject[connectedPlayers.Length];

        if (!endOfGame)
        {
		    transform.Find("RoundWinner").GetChild(0).Find("PlayerColor").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/PlayerColors/" + winnerColor);
		    transform.Find("RoundWinner").GetChild(0).Find("PlayerAvatar").gameObject.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Avatars/" + winnerAvatar);
		    transform.Find("RoundWinner").GetChild(0).Find("PlayerName").gameObject.GetComponent<Text>().text = winnerName;
        }


		for (int i = 0; i < connectedPlayers.Length; i++)
		{
			playerScoreHorizontal[i] = Instantiate(Resources.Load("UI/playerScoreHorizontal") as GameObject, transform.Find("PlayerScoreBoard").GetChild(0));

			float shotsFired = connectedPlayers[i].totalShotsFired;
			float hits = connectedPlayers[i].totalHits;
			float accuracy = 0f;

			if (shotsFired != 0f)
				accuracy = hits / shotsFired * 100f;

			playerScoreHorizontal[i].transform.Find("Player").Find("PlayerColor").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/PlayerColors/" + connectedPlayers[i].AvatarColor);
			playerScoreHorizontal[i].transform.Find("Player").Find("PlayerAvatar").transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Avatars/" + connectedPlayers[i].AvatarSymbol);
			playerScoreHorizontal[i].transform.Find("Wins").GetComponent<Text>().text = connectedPlayers[i].numberOfWins.ToString();
			playerScoreHorizontal[i].transform.Find("Kills").GetComponent<Text>().text = connectedPlayers[i].totalKills.ToString();
			playerScoreHorizontal[i].transform.Find("Damage").GetComponent<Text>().text = Mathf.Floor(connectedPlayers[i].totalDamage).ToString();
			//playerScoreHorizontal[i].transform.Find("Accuracy").GetComponent<Text>().text = connectedPlayers[i].Accuracy + "%";
			//playerScoreHorizontal[i].transform.Find("Accuracy").GetComponent<Text>().text = ((connectedPlayers[i].TotalHits / connectedPlayers[i].TotalShotsFired) * 100f) + "%";
			playerScoreHorizontal[i].transform.Find("Accuracy").GetComponent<Text>().text = Mathf.Floor(accuracy) + " %";

			if (connectedPlayers[i].Player == winnerPlayerIndex)
			{
				Color32 winColor = new Color32(255, 133, 54, 255);
				playerScoreHorizontal[i].transform.Find("Wins").GetComponent<Text>().color = winColor;
				playerScoreHorizontal[i].transform.Find("Kills").GetComponent<Text>().color = winColor;
				playerScoreHorizontal[i].transform.Find("Damage").GetComponent<Text>().color = winColor;
				playerScoreHorizontal[i].transform.Find("Accuracy").GetComponent<Text>().color = winColor;

				//TODO: Animate RoundWinner here
			}
		}
	}
}