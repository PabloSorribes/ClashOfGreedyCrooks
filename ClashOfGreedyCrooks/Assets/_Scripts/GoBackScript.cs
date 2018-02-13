using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoBackScript : MonoBehaviour {

    public Button backButton;

	// Use this for initialization
	void Start () {
        Button btn = backButton.GetComponent<Button>();
        btn.onClick.AddListener(GoBack);
	}
	
	void GoBack()
    {
        GameStateManager.GetInstance.SetState(GameState.MainMenu);
    }
}
