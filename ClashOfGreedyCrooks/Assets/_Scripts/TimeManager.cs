using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
	private static TimeManager instance;
	public static TimeManager GetInstance
	{
		get
		{
			return instance;
		}
	}

	public float trackTime = 120;

	private bool isPaused;

    private float normalTimeScale = 1;
    private float slowMoTimeScale = 0.2f;
    private float lerpTime = 1f;

	public GameObject player;

	public Text timer;
    public System.Action TimeIsUp;

	// Update is called once per frame
	void Update()
	{

		trackTime -= Time.deltaTime;
		timer.text = "Timer: " + Mathf.Floor(trackTime);

        //lerpTime = ;

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			PauseGame();
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			StartCoroutine(FreezeFrame(2f));
			Time.timeScale = Mathf.Lerp(normalTimeScale, slowMoTimeScale, lerpTime);
		}

        if (trackTime <= 0)
        {
            TimeIsUp();
        }
	}

	private void PauseGame()
	{

		if (!isPaused)
		{
			Time.timeScale = 0;

			isPaused = true;

            //TODO: This should be set by the player, by getting an Event Delegate 
            //from the GameStateManager which says if the game is paused or not.
            player.GetComponent<Shooting>().enabled = false;
		}

		else
		{
			Time.timeScale = 1;

			isPaused = false;

			player.GetComponent<Shooting>().enabled = true;
		}

	}

	public void StartFreezeFrame(float timeFreeze)
	{
		StartCoroutine(FreezeFrame(timeFreeze));
	}


	public IEnumerator FreezeFrame(float waitTime)
	{

		yield return new WaitForSecondsRealtime(waitTime);
		Time.timeScale = 1;

	}

}
