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

	internal bool countdownFinished;

	public float trackTime;
	private bool timeEnded = false;
	public Text timer;
	public System.Action TimeIsUp;

	private bool isPaused;

	//Freeze frame variables
	private float normalTimeScale = 1;
	private float slowMoTimeScale = 0.2f;
	private float lerpTime = 1f;

	//TODO: PauseState-actions should be done through InputManager
	//public GameObject player;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		timeEnded = false;
		timer.text = "Timer: " + Mathf.Floor(trackTime);
	}

	// Update is called once per frame
	void Update()
	{
		if (!timeEnded && countdownFinished)
		{
			trackTime -= Time.deltaTime;
			timer.text = "Timer: " + Mathf.Floor(trackTime);
		}

		//TODO: Add controller support
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			PauseGame();
		}

		//TODO: Remove this debug button
		//if (Input.GetKeyDown(KeyCode.Space))
		//{
		//	StartFreezeFrame(2f);
		//}

		//Fire event of TimeIsUp on a single frame
		if (trackTime <= 0 && !timeEnded)
		{
			timeEnded = true;
			trackTime = 0;
			timer.text = "Timer: " + Mathf.Floor(trackTime);

			if (TimeIsUp != null)
			{
				TimeIsUp();
			}
		}
	}

	private void PauseGame()
	{
		if (!isPaused)
		{
			Time.timeScale = 0;

			isPaused = true;
			GameStateManager.GetInstance.SetPausedState(OurPauseState.Paused);

			//TODO: This should be set by the player, by getting an Event Delegate 
			//from the GameStateManager which says if the game is paused or not.
			//player.GetComponent<Shooting>().enabled = false;
		}

		else
		{
			Time.timeScale = 1;

			isPaused = false;
			GameStateManager.GetInstance.SetPausedState(OurPauseState.NotPaused);

			//player.GetComponent<Shooting>().enabled = true;
		}
	}

	/// <summary>
	/// Slows down the game. <paramref name="timeToFreeze"/> is the amount of seconds that the slowMo should be.
	/// </summary>
	/// <param name="timeToFreeze"></param>
	public void StartFreezeFrame(float timeToFreeze)
	{
		StartCoroutine(FreezeFrameLength(timeToFreeze));
		Time.timeScale = Mathf.Lerp(normalTimeScale, slowMoTimeScale, lerpTime);
	}

	public IEnumerator FreezeFrameLength(float waitTime)
	{
		yield return new WaitForSecondsRealtime(waitTime);
		Time.timeScale = 1;
	}
}
