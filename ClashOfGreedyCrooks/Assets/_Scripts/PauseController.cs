using UnityEngine;

public class PauseController : MonoBehaviour
{

	public void BackToMainMenu()
	{
		//PlayerManager.Reset();

		GameStateManager.GetInstance.SetState(GameState.MainMenu);
	}

	public void Back()
	{
		GameStateManager.GetInstance.PauseGame();

		//gameObject.SetActive(false);
	}

	private void OnDisable()
	{
		//GameStateManager.GetInstance.SetPausedState(OurPauseState.NotPaused);
	}
}
