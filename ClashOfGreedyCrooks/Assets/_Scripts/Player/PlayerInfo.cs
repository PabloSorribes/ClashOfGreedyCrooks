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
	//private int numberOfWins;
	//private int currentRoundHits;
	//private int totalHits;
	//private float currentRoundDamage;
	//private float totalDamage;
	//private int currentRoundShotsFired;
	//private int totalShotsFired;

	//public int NumberOfWins { get; set; }
	//public int CurrentRoundHits { get; set; }
	//public int TotalHits { get; set; }
	//public float CurrentRoundDamage { get; set; }
	//public float TotalDamage { get; set; }
	//public int CurrentRoundShotsFired { get; set; }
	//public int TotalShotsFired { get; set; }

}
