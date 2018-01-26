using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour {

    private static GameStateManager instance;
    public static GameStateManager GetInstance()
    {
        return instance;
    }

    public enum State { MainMenu, PlayerConnect, Picking, Arena };
    private State state;

    public enum PauseState { Paused, NotPaused };
    private PauseState pauseState;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
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

        state = newState;
    }

    private void OnMainMenuState()
    {
        GameManager.GetInstance().ResetGame();
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

    private void OnPausedState()
    {

    }

    private void OnNotPausedState()
    {

    }
}
