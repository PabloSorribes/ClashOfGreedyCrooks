using UnityEngine;

public class PauseController : MonoBehaviour {

    private void OnEnable()
    {
        GameStateManager.GetInstance.SetPausedState(OurPauseState.Paused);
    }

    public void BackToMainMenu()
    {
        GameStateManager.GetInstance.SetState(GameState.MainMenu);
    }

    public void Back()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        GameStateManager.GetInstance.SetPausedState(OurPauseState.NotPaused);
    }
}
