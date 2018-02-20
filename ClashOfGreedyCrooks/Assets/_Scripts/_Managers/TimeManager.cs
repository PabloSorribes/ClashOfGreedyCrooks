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

	private bool countdownFinished;

    private float startTime;
	public float trackTime;
	private bool timeEnded = false;
	public Text timer;
	public System.Action TimeIsUp;
    private Image circleTimer;

    public GameObject pauseMenu;
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
        circleTimer = GameObject.Find("CircleTimerCanvas").GetComponentInChildren<Image>();
        startTime = trackTime;
		timeEnded = false;
		timer.text = "" /*+ Mathf.Floor(trackTime)*/;
	}

	// Update is called once per frame
	void Update()
	{
		if (!timeEnded && countdownFinished)
		{
			trackTime -= Time.deltaTime;
			timer.text = "" + Mathf.Floor(trackTime);
            circleTimer.fillAmount = Mathf.Abs((trackTime / startTime) - 1f);
		}

		//TODO: Add controller support
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			PauseGame();
            print("Dis does work!");
		}

		//Fire event of TimeIsUp on a single frame
		if (trackTime <= 0 && !timeEnded)
		{
			timeEnded = true;
			trackTime = 0;
			timer.text = "" + Mathf.Floor(trackTime);

			if (TimeIsUp != null)
			{
				TimeIsUp();
			}
		}
	}

	public void EnableTimer()
	{
		timer.gameObject.SetActive(true);
		countdownFinished = true;
	}

	private void PauseGame()
	{
		if (!isPaused)
		{
			isPaused = true;

            pauseMenu.SetActive(true);
            print("Nope");
			//TODO: This should be set by the player, by getting an Event Delegate 
			//from the GameStateManager which says if the game is paused or not.
			//player.GetComponent<Shooting>().enabled = false;
		}

		else
		{
			isPaused = false;

            pauseMenu.SetActive(true);

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
