using UnityEngine;
using UnityEngine.SceneManagement;

//TODO: Move global enums to own script?
public enum GameState { MainMenu, PlayerConnect, Picking, Arena };
public enum OurPauseState { Paused, NotPaused };

public class GameStateManager : GenericSingleton<GameStateManager>
{
	private GameState gameState;
	private OurPauseState pauseState;

	public System.Action<GameState> GameStateChanged;

    private ScreenFade screenFade;
    private string sceneToLoad;

	private void Awake()
	{
		//base.Awake();
		gameState = (GameState)SceneManager.GetActiveScene().buildIndex;
		SceneManager.sceneLoaded += OnSceneChanged;
	}

    private void Start()
    {
        GameObject screenFadePrefab = Resources.Load("ScreenFade") as GameObject;
        GameObject newScreenFade = Instantiate(screenFadePrefab);
        screenFade = newScreenFade.GetComponent<ScreenFade>();
        screenFade.FadeOutComplete += ScreenFadeOutComplete;
        screenFade.FadeInComplete += ScreenFadeInComplete;
        screenFade.SetDir(BlackScreen.In);
    }

    private void ScreenFadeOutComplete()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    private void ScreenFadeInComplete()
    {
        if (gameState == GameState.Picking)
        {
            StartCountdown(3, "Start picking!");
        }
        if (gameState == GameState.Arena)
        {
            StartCountdown(3, "Fight!");
        }
    }

    private void StartCountdown(int time, string endText)
    {
        GameObject countdownPrefab = Resources.Load("Countdown") as GameObject;
        GameObject newCountdown = Instantiate(countdownPrefab);
        newCountdown.GetComponent<Countdown>().InitializeCountdown(time, endText);
        InputManager.GetInstance.freezeInput = true;
    }

    /// <summary>
    /// Get the current State
    /// </summary>
    /// <returns></returns>
    public GameState GetState()
	{
		GameState tempState = gameState;
		return tempState;
	}

	/// <summary>
	/// Changes the scene
	/// </summary>
	/// <param name="newState"></param>
	public void SetState(GameState newState)
	{
        screenFade.SetDir(BlackScreen.Out);

        if (newState == GameState.MainMenu)
			OnMainMenuState();
		else if (newState == GameState.PlayerConnect)
			OnPlayerConnectState();
		else if (newState == GameState.Picking)
			OnPickingState();
		else if (newState == GameState.Arena)
			OnArenaState();

		Time.timeScale = 1f;
	}

	private void OnMainMenuState()
	{
        sceneToLoad = "MainMenu";
	}

	private void OnPlayerConnectState()
	{        
        sceneToLoad = "PlayerConnect";
	}

	private void OnPickingState()
	{
        sceneToLoad = "Picking";
	}

	private void OnArenaState()
	{
        sceneToLoad = "Arena01";
	}

	/// <summary>
	/// Pause or unpause the game
	/// </summary>
	/// <param name="newState"></param>
	public void SetPausedState(OurPauseState newState)
	{
		if (newState == OurPauseState.Paused)
			OnPausedState();
		else if (newState == OurPauseState.NotPaused)
			OnNotPausedState();

		pauseState = newState;
	}

	/// <summary>
	/// Subscribed method for SceneLoaded Event. Runs when new scene is loaded.
	/// </summary>
	/// <param name="newScene"></param>
	/// <param name="loadingMode"></param>
	private void OnSceneChanged(Scene newScene, LoadSceneMode loadingMode)
	{
		gameState = (GameState)newScene.buildIndex;

		if (GameStateChanged != null)
		{
			GameStateChanged(gameState);
            screenFade.SetDir(BlackScreen.In);
		}
		//Debug.Log("(GSM) State Changed: " + gameState);
	}

	private void OnPausedState()
	{

	}

	private void OnNotPausedState()
	{

	}
}
