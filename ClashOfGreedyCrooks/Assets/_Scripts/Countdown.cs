﻿using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour {

    private Sprite[] images;
    private Image countdownImageHolder;

    private int time;

	private void Start()
    {
        LoadImages();
        countdownImageHolder = GetComponentInChildren<Image>();
        InvokeRepeating("CountdownTimer", 0f, 1f);
    }

    private void LoadImages()
    {
        images = Resources.LoadAll("UI/Countdown", typeof(Sprite)).Cast<Sprite>().ToArray();
    }

    public void InitializeCountdown(int time)
    {
        this.time = time;
    }

	private void CountdownTimer()
    {
        if (time == 0)
        {
			CancelInvoke();
            if (GameStateManager.GetInstance.GetState() == GameState.Picking)
			{
                countdownImageHolder.sprite = images[4];
				Invoke("EndCountdown", 1.5f);
				countdownImageHolder.GetComponent<Animator>().SetTrigger("EndPicking");
			}
			else if (GameStateManager.GetInstance.GetState() == GameState.Arena)
			{
                countdownImageHolder.sprite = images[3];
				AudioManager.GetInstance.PlayOneShot("event:/Arena/countDown", "end", 1f);
				Invoke("EndCountdown", 1f);
				countdownImageHolder.GetComponent<Animator>().SetTrigger("EndArena");
			}

			countdownImageHolder.SetNativeSize();
            
            return;
        }

		AudioManager.GetInstance.PlayOneShot("event:/Arena/countDown");

        countdownImageHolder.sprite = images[time - 1];
        countdownImageHolder.SetNativeSize();
        countdownImageHolder.GetComponent<Animator>().SetTrigger("Show");
        time--;
    }

    private void EndCountdown()
    {
		if (GameStateManager.GetInstance.GetState() == GameState.Arena)
			TimeManager.GetInstance.EnableTimer();

		InputManager.GetInstance.freezeInput = false;

		Destroy(gameObject);
    }

}
