using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    private bool connected;
    private bool ready;
    private int player;
    private int gamepad;
    private Color32 avatar;
    private bool hasChampion;

    public bool Connected { get; set; }
    public bool Ready { get; set; }
    public int Player { get; set; }
    public int Gamepad { get; set; }
    public Color32 Avatar { get; set; }
    public bool HasChampion { get; set; }

    public PlayerInfo(int player, int gamepad)
    {
        Player = player;
        Gamepad = gamepad;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
