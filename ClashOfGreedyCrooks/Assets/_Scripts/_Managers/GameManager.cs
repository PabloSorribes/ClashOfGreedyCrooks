using UnityEngine;

public class GameManager : GenericSingleton<GameManager>
{
    private void Start()
    {
		OnGameStateChanged(GameStateManager.GetInstance.GetState());
		GameStateManager.GetInstance.GameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newGameState)
    {
		if (newGameState == GameState.Picking)
        {
            
        }

        if (newGameState == GameState.Arena)
        {
            
        }
	}
}
