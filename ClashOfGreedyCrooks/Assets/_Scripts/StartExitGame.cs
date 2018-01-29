using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartExitGame : MonoBehaviour {

    public Button startButton;
    public Button exitButton;

    // Use this for initialization
    void Start () {
        startButton.onClick.AddListener(StartButtonClicked);
        exitButton.onClick.AddListener(ExitButtonClicked);
	}
	
	// Update is called once per frame
	void StartButtonClicked()
    {
        SceneManager.LoadScene("PlayerConnect");
    }

    void ExitButtonClicked ()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
	Application.OpenURL(webplayerQuitURL);
#else
	Application.Quit();
#endif
    }
}
