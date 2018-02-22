using UnityEngine;

public class PauseController : MonoBehaviour {

    public GameObject eventSystem;

    private void Start()
    {
        //Instantiate(eventSystem.gameObject);
    }

    private void OnEnable()
    {
        //GameStateManager.GetInstance.SetPausedState(OurPauseState.Paused);
    }

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
