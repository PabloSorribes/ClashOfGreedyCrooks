using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour {

	public enum State { MainMenu, PlayerConnect, Picking, Arena };
    private State state;

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

        state = newState;
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
}
