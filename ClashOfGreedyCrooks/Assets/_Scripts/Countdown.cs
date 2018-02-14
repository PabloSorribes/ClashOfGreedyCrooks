using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour {

    private Sprite[] images;
    private Image countdownImageHolder;

    private int time;

	private FMODUnity.StudioEventEmitter a_countDown;

	private void Start()
    {
        LoadImages();
		InitializeAudio();
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

	private void InitializeAudio() 
	{
		a_countDown = gameObject.AddComponent<FMODUnity.StudioEventEmitter>();
		a_countDown.Event = "event:/Arena/countDown";
		a_countDown.SetParameter("end", 1f);
	}

	private void CountdownTimer()
    {
        if (time == 0)
        {
			a_countDown.SetParameter("end", 1f);
			a_countDown.Play();

            CancelInvoke();
            if (GameStateManager.GetInstance.GetState() == GameState.Picking)
                countdownImageHolder.sprite = images[4];
            else if (GameStateManager.GetInstance.GetState() == GameState.Arena)
                countdownImageHolder.sprite = images[3];
            countdownImageHolder.SetNativeSize();
            Invoke("EndCountdown", 2f);
            InputManager.GetInstance.freezeInput = false;
            return;
        }
		a_countDown.Play();
        countdownImageHolder.sprite = images[time - 1];
        countdownImageHolder.SetNativeSize();
        time--;
    }

    private void EndCountdown()
    {
        if(GameStateManager.GetInstance.GetState() == GameState.Arena)
		    TimeManager.GetInstance.countdownFinished = true;

		Destroy(gameObject);
    }

}
