using UnityEngine;

public class PlayerInfo : MonoBehaviour
{

    private bool connected;
    private bool ready;
    private bool choosingPenalty;
    private int player;
    private int gamepad;
    private Color32 avatar;
    private bool hasChampion;
	private bool isAlive = true;

	public bool Connected { get; set; }
    public bool Ready { get; set; }
    public bool ChoosingPenalty { get; set; }
    public int Player { get; set; }
    public int Gamepad { get; set; }
    public Color32 Avatar { get; set; }
    public bool HasChampion { get; set; }
    public bool IsAlive { get; set; }

	//Score data
	private int numberOfWins = 0;
	private int currentRoundHits = 0;
	private int totalHits = 0;
	private float currentRoundDamage = 0;
	private float totalDamage = 0;
	private int currentRoundShotsFired = 0;
	private int totalShotsFired = 0;

	public int NumberOfWins { get; set; }
	public int CurrentRoundHits { get; set; }
	public int TotalHits { get; set; }
	public float CurrentRoundDamage { get; set; }
	public float TotalDamage { get; set; }
	public int CurrentRoundShotsFired { get; set; }
	public int TotalShotsFired { get; set; }


	public void SetDontDestroyOnLoad()
    {
        DontDestroyOnLoad(gameObject);
    }
}
