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

	private float trackTime = 120;

	private bool isPaused;

	public GameObject player;

	public Text timer;

	// Update is called once per frame
	void Update()
	{

		trackTime -= Time.deltaTime;
		timer.text = "Timer: " + Mathf.Floor(trackTime);

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			PauseGame();
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			StartCoroutine(FreezeFrame(0.1f));
			Time.timeScale = 0;
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

	public void StartFreezeFrame()
	{
		StartCoroutine(FreezeFrame(1f));
	}


	public IEnumerator FreezeFrame(float waitTime)
	{

		yield return new WaitForSecondsRealtime(waitTime);
		Time.timeScale = 1;

	}

}
