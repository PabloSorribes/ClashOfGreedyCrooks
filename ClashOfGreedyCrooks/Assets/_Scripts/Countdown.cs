using System.Linq;
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
		float timeForPickYourCrookAnimation = 2f;

        if (time == 0)
        {
			CancelInvoke();

			//"Pick your Crook!"-text (countdown_pick.png).
            if (GameStateManager.GetInstance.GetState() == GameState.Picking)
			{
				AudioManager.GetInstance.PlayOneShot("event:/Picking/pickingPickYourCrook");
				countdownImageHolder.sprite = images[4];
				Invoke("EndCountdown", timeForPickYourCrookAnimation);
				countdownImageHolder.GetComponent<Animator>().SetTrigger("EndPicking");
			}

			//"Fight!"-text (countdown_fight.png).
			else if (GameStateManager.GetInstance.GetState() == GameState.Arena)
			{
				AudioManager.GetInstance.PlayOneShot("event:/Arena/countDown", "end", 1f);
                countdownImageHolder.sprite = images[3];
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
