using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
	private List<GameObject> uiPanels = new List<GameObject>();
	private Transform activePanel;

    public GameObject mainCanvas;
    public GameObject settingsCanvas;
    public GameObject creditsCanvas;

	/// <summary>
	/// Get the UiPanels for the menu and activate the MainMenu-panel.
	/// </summary>
	private void Awake()
	{
		for (int i = 0; i < transform.childCount; i++) {
			uiPanels.Add(transform.GetChild(i).gameObject);
		}

		MainMenu();
	}


	/// <summary>
	/// Load the Player Connect-scene. Should be called by the menu buttons.
	/// </summary>
	public void NewGame()
	{
		AudioManager.GetInstance.PlayOneShot("event:/MainMenu/menuAccept");
		GameStateManager.GetInstance.SetState(GameState.PlayerConnect);
	}

	//Forward
	public void Settings() {
		ActivatePanel("Settings");
        GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(settingsCanvas, null);
		AudioManager.GetInstance.PlayOneShot("event:/MainMenu/menuAccept");
	}

	//Forward
	public void Credits() {
		ActivatePanel("Credits");
        GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(creditsCanvas, null);
		AudioManager.GetInstance.PlayOneShot("event:/MainMenu/menuAccept");
	}

	//Go back
	public void MainMenu() {
		ActivatePanel("MainMenu");
        GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(mainCanvas, null);
		AudioManager.GetInstance.PlayOneShot("event:/MainMenu/menuDecline");
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

    //private void SwitchCanvas ()
    //{
        //GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(settingsCanvas, null);
    //}

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
