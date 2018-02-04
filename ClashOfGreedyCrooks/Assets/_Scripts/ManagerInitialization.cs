using UnityEngine;

public class ManagerInitialization : MonoBehaviour
{

	//Calls the static singleton manager classes which instantiates them. 
	private void Awake()
	{
		if (GameManager.GetInstance)
		if (GameStateManager.GetInstance)
		if (InputManager.GetInstance)

		Destroy(this.gameObject);
	}
}
