using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
	private bool connected;
	private bool ready;
	private bool choosingPenalty;
	private int player;
	private int gamepad;
	private bool hasChampion;
	private string avatarColor;
	private string avatarSymbol;

	public bool Connected { get; set; }
	public bool Ready { get; set; }
	public bool ChoosingPenalty { get; set; }
	public int Player { get; set; }
	public int Gamepad { get; set; }
	public bool HasChampion { get; set; }
	public string AvatarColor { get; set; }
	public string AvatarSymbol { get; set; }

	//Score data
	private int numberOfWins;
	private int totalKills;
	private int totalHits;
	private float totalDamage;
	private int totalShotsFired;
	private float accuracy;

	public int NumberOfWins { get; set; }
	public int TotalKills { get; set; }
	public int TotalHits { get; set; }
	public float TotalDamage { get; set; }
	public int TotalShotsFired { get; set; }
	public float Accuracy
	{
		get
		{
			if (totalShotsFired == 0f || totalHits == 0f)
				return 0f;
			else
				return accuracy = totalShotsFired / totalHits;
		}
	}

}
