using UnityEngine;
using XInputDotNetPure;

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

		AddConnectedGamepads();
	}

	private void AddConnectedGamepads()
	{
		for (int i = 0; i < 3; ++i)
		{
			PlayerIndex testGamepadIndex = (PlayerIndex)i;
			GamePadState testState = GamePad.GetState(testGamepadIndex);
			if (testState.IsConnected)
			{
				Debug.Log(string.Format("GamePad found {0}", testGamepadIndex));
				//players[i].gamepadIndex = (int)testGamepadIndex;
			}
		}
	}

	void Update()
	{
		for (int i = 0; i < 3; ++i)
		{
			InputManager.instance.CheckForInputs(players[i].playerIndex);
		}
	}

    public void AddPlayer(int playerIndex, int gamepadIndex)
    {
        players[playerIndex].connected = true;
        players[playerIndex].playerIndex = playerIndex;
        players[playerIndex].gamepadIndex = gamepadIndex;
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

    public void ResetGame()
	{

	}


	private struct Player
	{
		public bool connected;
		public int playerIndex;
        public int gamepadIndex;
        public GameObject champion;
        public PlayerController pc;
    }
}
