using UnityEngine;

public class ManagerInitialization : MonoBehaviour
{
	//[Header("For use in testscenes only! Should be false elsewhere.")]
	//public bool SetTrueForTesting = false;
	//public GameState gameStateOverride;

	//Calls the static singleton manager classes with an irrellevant check which will instantiate them. 
	private void Awake()
	{
		if (GameManager.GetInstance)
		{
		}

		if (GameStateManager.GetInstance)
		{
		}

		if (InputManager.GetInstance)
		{
			//InputManager.setTrueForTesting = this.SetTrueForTesting;
			//InputManager.manualGameStateOverride = this.gameStateOverride;
		}

		if (AudioManager.GetInstance) 
		{
		}

		Destroy(this.gameObject);
	}
}
