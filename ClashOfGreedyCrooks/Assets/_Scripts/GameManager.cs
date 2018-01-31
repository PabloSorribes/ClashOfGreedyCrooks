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
			PlayerIndex testPlayerIndex = (PlayerIndex)i;
			GamePadState testState = GamePad.GetState(testPlayerIndex);
			if (testState.IsConnected)
			{
				Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
				players[i].playerIndex = (int)testPlayerIndex;
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

	public void AddPlayer(int playerIndex)
	{
		//TODO: Add back the code that was lost
	}

	public void RemovePlayer(int playerIndex)
	{

	}

	public int GetPlayersCount()
	{
		int count = 0;
		for (int i = 0; i < players.Length; i++)
		{
			//TODO: Add back the code that was lost

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
		public GameObject champion;
	}
}
