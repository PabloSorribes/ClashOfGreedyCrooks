using UnityEngine;

/// <summary>
/// Calls the static singleton manager classes with an irrellevant check which will instantiate them.
/// </summary>
public class ManagerInitialization : MonoBehaviour
{
	private void Awake()
	{

		if (GameStateManager.GetInstance)
		{
		}

		if (InputManager.GetInstance)
		{
		}

		if (AudioManager.GetInstance)
		{
		}

		Destroy(gameObject);
	}
}
