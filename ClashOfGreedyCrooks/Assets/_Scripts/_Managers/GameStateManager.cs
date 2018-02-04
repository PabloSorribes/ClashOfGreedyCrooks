using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//TODO: Move global enums to own script?
public enum GameState { MainMenu, PlayerConnect, Picking, Arena };
public enum PauseState { Paused, NotPaused };

public class GameStateManager : GenericSingleton<GameStateManager>
{
	private GameState gameState;
	private PauseState pauseState;

	public System.Action<GameState> GameStateChanged;


	private void Awake()
	{
		//base.Awake();
		gameState = (GameState)SceneManager.GetActiveScene().buildIndex;
		SceneManager.sceneLoaded += OnSceneChanged;
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
		if (newState == GameState.MainMenu)
			OnMainMenuState();
		else if (newState == GameState.PlayerConnect)
			OnPlayerConnectState();
		else if (newState == GameState.Picking)
			OnPickingState();
		else if (newState == GameState.Arena)
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
