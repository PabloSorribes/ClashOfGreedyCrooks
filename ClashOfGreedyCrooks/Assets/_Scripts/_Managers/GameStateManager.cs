using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { MainMenu, PlayerConnect, Picking, Arena, LoadingScreen };
public enum OurPauseState { Paused, NotPaused };

public class GameStateManager : GenericSingleton<GameStateManager>
{
	private GameState gameState;
	private OurPauseState pauseState;

	public System.Action<GameState> GameStateChanged;

	private ScreenFade screenFade;
	private string sceneToLoad;

	private int temp_DeathCircleDamage;

	private GameObject pauseMenu;


	private void Awake()
	{
		gameState = (GameState)SceneManager.GetActiveScene().buildIndex;
		pauseState = OurPauseState.NotPaused;

		SceneManager.sceneLoaded += OnSceneChanged;

		//Hide mouse cursor
		Cursor.visible = false;
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
			pauseMenu = GameObject.Find("PauseMenuHolder").transform.GetChild(0).gameObject;
			StartCountdown(0);
		}
		if (gameState == GameState.Arena)
		{
			pauseMenu = GameObject.Find("PauseMenuHolder").transform.GetChild(0).gameObject;
			StartCountdown(3);
		}
	}

	private void StartCountdown(int time)
	{
		GameObject countdownPrefab = Resources.Load("Countdown") as GameObject;
		GameObject newCountdown = Instantiate(countdownPrefab);
		newCountdown.GetComponent<Countdown>().InitializeCountdown(time);
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
		else if (newState == GameState.LoadingScreen)
			OnLoadingScreenState();

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

	private void OnLoadingScreenState()
	{
		sceneToLoad = "LoadingScreen";
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
	}

	public OurPauseState GetPauseState()
	{
		OurPauseState tempPauseState = pauseState;
		return tempPauseState;
	}

	public void PauseToggle()
	{
		if (pauseState == OurPauseState.NotPaused)
			SetPaused();
		else if (pauseState == OurPauseState.Paused)
			SetUnpaused();
	}

	private void SetPaused()
	{
		InputManager.GetInstance.freezeInput = true;

		Time.timeScale = 0;

		if (gameState == GameState.Arena)
		{
			//TODO: Rewrite to handle this better. Hack to avoid DeathCircle from killing players when game is paused.
			DeathCircle.GetInstance.roundIsOver = true;
			temp_DeathCircleDamage = DeathCircle.GetInstance.deathZoneDamage;
			DeathCircle.GetInstance.deathZoneDamage = 0;
		}

		pauseMenu.SetActive(true);
		pauseMenu.GetComponent<PauseController>().InitializePauseMenu();

		pauseState = OurPauseState.Paused;

		AudioManager.GetInstance.OnPauseBegin();
	}

	private void SetUnpaused()
	{
		if (gameState == GameState.Arena)
		{
			//TODO: Rewrite to handle this better
			DeathCircle.GetInstance.roundIsOver = false;
			DeathCircle.GetInstance.deathZoneDamage = temp_DeathCircleDamage;
		}

		pauseMenu.SetActive(false);
		pauseState = OurPauseState.NotPaused;

		AudioManager.GetInstance.OnPauseEnd();

		Time.timeScale = 1f;
		Invoke("Unfreeze", 0.2f);
	}

	private void Unfreeze()
	{
		if (!screenFade.Fading)
			InputManager.GetInstance.freezeInput = false;
	}
}
