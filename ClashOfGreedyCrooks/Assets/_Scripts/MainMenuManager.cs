using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour {

	private List<GameObject> uiPanels = new List<GameObject>();
	private Transform activePanel;

	/// <summary>
	/// Get the UiPanels for the menu and activate the MainMenu-panel.
	/// </summary>
	private void Awake() {

		for (int i = 0; i < transform.childCount; i++) {
			uiPanels.Add(transform.GetChild(i).gameObject);
		}

		MainMenu();
	}

	/// <summary>
	/// Load the Player Connect-scene.
	/// </summary>
	public void NewGame() {
		GameStateManager.GetInstance.SetState(GameState.PlayerConnect);
	}


	public void Settings() {
		ActivatePanel("Settings");
	}

	public void Credits() {
		ActivatePanel("Credits");
	}

	public void MainMenu() {
		ActivatePanel("MainMenu");
	}


	/// <summary>
	/// Shows the panel (menu content) that you want.
	/// </summary>
	/// <param name="panel"></param>
	private void ActivatePanel(string panel) {
		for (int i = 0; i < uiPanels.Count; i++) {
			if (uiPanels[i].name == panel) {
				uiPanels[i].SetActive(true);
				activePanel = uiPanels[i].transform;
			}
			else
				uiPanels[i].SetActive(false);
		}
	}

	/// <summary>
	/// Quit the game regardless if playing in Editor, WebPlayer or in .exe-file.
	/// </summary>
	public void ExitGame() {
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
	Application.OpenURL(webplayerQuitURL);
#else
	Application.Quit();
#endif
	}
}
