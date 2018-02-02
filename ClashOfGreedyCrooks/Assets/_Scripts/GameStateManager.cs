using UnityEngine;
using UnityEngine.SceneManagement;

//TODO: Move global enums to own script?
public enum State { MainMenu, PlayerConnect, Picking, Arena };
public enum PauseState { Paused, NotPaused };

public class GameStateManager : MonoBehaviour
{

	private static GameStateManager instance;
	public static GameStateManager GetInstance()
	{
		return instance;
	}

	private State gameState;
	private PauseState pauseState;

	public delegate void StateChanged(State newGameState);
	public event StateChanged OnStateChanged;

	//public System.Action<State> OnStateChanged;

	private void Awake()
	{
		instance = this;

		DontDestroyOnLoad(gameObject);

		if (instance == null)
		{
			instance = this;
		}
		else if (FindObjectOfType<GameStateManager>().gameObject != this.gameObject)
		{
			Destroy(FindObjectOfType<GameStateManager>().gameObject);
		}

	}

	private void Start()
	{
		SceneManager.sceneLoaded += OnSceneChanged;
	}

	/// <summary>
	/// Get the current State
	/// </summary>
	/// <returns></returns>
	public State GetState()
	{
		State tempState = gameState;
		return tempState;
	}

	/// <summary>
	/// Changes the scene
	/// </summary>
	/// <param name="newState"></param>
	public void SetState(State newState)
	{
		if (newState == State.MainMenu)
			OnMainMenuState();
		else if (newState == State.PlayerConnect)
			OnPlayerConnectState();
		else if (newState == State.Picking)
			OnPickingState();
		else if (newState == State.Arena)
			OnArenaState();
	}

	private void OnMainMenuState()
	{
		SceneManager.LoadScene("MainMenu");
	}

	private void OnPlayerConnectState()
	{
		SceneManager.LoadScene("PlayerConnect");
	}

	private void OnPickingState()
	{
		SceneManager.LoadScene("Picking");
	}

	private void OnArenaState()
	{
		SceneManager.LoadScene("Arena01");
	}

	/// <summary>
	/// Pause or unpause the game
	/// </summary>
	/// <param name="newState"></param>
	public void SetPausedState(PauseState newState)
	{
		if (newState == PauseState.Paused)
			OnPausedState();
		else if (newState == PauseState.NotPaused)
			OnNotPausedState();

		pauseState = newState;
	}

	private void OnSceneChanged(Scene newScene, LoadSceneMode loadingMode)
	{
		gameState = (State)newScene.buildIndex;

		OnStateChanged(gameState);
		print("(GSM)State Changed to: " + gameState);
	}

	private void OnPausedState()
	{

	}

	private void OnNotPausedState()
	{

	}
}
