using UnityEngine;

public class PauseController : MonoBehaviour
{

	public UnityEngine.EventSystems.EventSystem eventSystem;
	public GameObject firstSelectedItem;

	public void InitializePauseMenu()
	{
		eventSystem.SetSelectedGameObject(null);
		eventSystem.SetSelectedGameObject(firstSelectedItem);
	}

	public void BackToMainMenu()
	{
		if (GameStateManager.GetInstance.GetState() == GameState.Arena)
			ArenaManager.GetInstance.DestroyLastPlayers();

		GameStateManager.GetInstance.PauseToggle();
		GameStateManager.GetInstance.SetState(GameState.MainMenu);
	}

	/// <summary>
	/// Show controls menu.
	/// </summary>
	public void Controls()
	{
		//TODO: Add functionality to show the Control-scheme menu.
	}

	public void Back()
	{
		GameStateManager.GetInstance.PauseToggle();
	}
}
