using UnityEngine;
using XInputDotNetPure;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	private static GameManager instance;
	public static GameManager GetInstance()
	{
		return instance;
	}

	private Player[] players = new Player[4];

	private void Awake()
	{
		instance = this;
	}

    private void Start()
	{
		DontDestroyOnLoad(gameObject);

		if (instance == null)
		{
			instance = this;
		}
		else if (FindObjectOfType<GameStateManager>().gameObject != this.gameObject)
		{
			Destroy(FindObjectOfType<GameStateManager>().gameObject);
		}

        SceneManager.sceneLoaded += SendInfoToInputManager;
	}

    /// <summary>
    /// Call from PlayerConnectManager.
    /// </summary>
    /// <param name="playerIndex"></param>
    /// <param name="gamepadIndex"></param>
    /// <param name="avatar"></param>
	public void AddPlayer(int playerIndex, int gamepadIndex, Color avatar)
	{
		players[playerIndex].connected = true;
		players[playerIndex].playerIndex = playerIndex;
		players[playerIndex].gamepadIndex = gamepadIndex;
        players[playerIndex].avatar = avatar;
    }

    /// <summary>
    /// Call from PickingManager.
    /// </summary>
    /// <param name="playerIndex"></param>
    /// <param name="champion"></param>
    public void AddPlayer(int playerIndex, GameObject champion)
    {
        players[playerIndex].playerIndex = playerIndex;
        players[playerIndex].champion = champion;
    }

    public void RemovePlayer(int playerIndex)
	{

	}

	public int GetPlayersCount()
	{
		int count = 0;
		for (int i = 0; i < players.Length; i++)
		{
			if (players[i].connected)
				count++;
		}
		return count;
	}

    public Color GetPlayerAvatar(int playerIndex)
    {
        if (players[playerIndex].connected)
            return players[playerIndex].avatar;
        else
            return Color.black;
    }

	public void ResetGame()
	{

	}

    public void SendInfoToInputManager(Scene newScene, LoadSceneMode loadingMode)
    {
        if (newScene.name == "Picking")
        {

        }
    }

	private struct Player
	{
		public bool connected;
		public int playerIndex;
		public int gamepadIndex;
        public Color avatar;
		public GameObject champion;
		public PlayerController pc;
	}
}
