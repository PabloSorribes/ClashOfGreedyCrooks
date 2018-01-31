using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour {

    private float trackTime = 120;

    private bool isPaused;

    public GameObject player;

    public Text timer;


	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {

        trackTime -= Time.deltaTime;
        timer.text = "Timer: " + Mathf.Floor(trackTime);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }   
    }

    private void PauseGame()
    {

        if (!isPaused)
        {
            Time.timeScale = 0;

            isPaused = true;

            player.GetComponent<Shooting>().enabled = false;
        }

        else
        {
            Time.timeScale = 1;

            isPaused = false;

            player.GetComponent<Shooting>().enabled = true;
        }

    }
}
