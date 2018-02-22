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
	//private int totalKills;
	//private int totalHits;
	//private float totalDamage;
	//private int totalShotsFired;
	//private float accuracy;

	[HideInInspector]
	public string championName;
	[HideInInspector]
	public int numberOfWins = 0;
	[HideInInspector]
	public int totalKills = 0;
	[HideInInspector]
	public int totalHits = 0;
	[HideInInspector]
	public float totalDamage = 0f;
	[HideInInspector]
	public int totalShotsFired = 0;
	//public float accuracy
	//{
	//	get
	//	{
	//		if (TotalShotsFired != 0f)
	//			return TotalHits / TotalShotsFired * 100f;
	//		else
	//			return 0;
	//	}
	//}

}
