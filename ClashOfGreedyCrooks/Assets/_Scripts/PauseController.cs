using UnityEngine;

public class PauseController : MonoBehaviour
{

	public UnityEngine.EventSystems.EventSystem eventSystem;
	public GameObject firstSelectedItem;

	private void Start()
	{
		eventSystem.SetSelectedGameObject(null);
		eventSystem.SetSelectedGameObject(firstSelectedItem);
	}

	public void BackToMainMenu()
	{
		if (GameStateManager.GetInstance.GetState() == GameState.Arena)
			ArenaManager.GetInstance.DestroyLastPlayers();

		GameStateManager.GetInstance.SetState(GameState.MainMenu);
	}

	public void Back()
	{
		GameStateManager.GetInstance.PauseToggle();
	}
}
