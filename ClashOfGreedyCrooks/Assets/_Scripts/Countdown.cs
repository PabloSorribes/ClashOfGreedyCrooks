using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour {

    private Text countdownText;

    public int time;

	FMOD.Studio.EventInstance a_countDownInstance;

	private void Start()
    {
		InitializeAudio();
        countdownText = GetComponentInChildren<Text>();
        InvokeRepeating("CountdownTimer", 0f, 1f);
    }

	private void InitializeAudio() 
	{
		a_countDownInstance = FMODUnity.RuntimeManager.CreateInstance("event:/Arena/countDown");
	}

	private void CountdownTimer()
    {
		//AudioManager.GetInstance.

        if (time == 0)
        {
			a_countDownInstance.setParameterValue("end", 1f);
			a_countDownInstance.start();

			CancelInvoke();
            countdownText.text = "Fight!";
            Invoke("EndCountdown", 2f);
            InputManager.GetInstance.freezeInput = false;
            return;
        }
		print(time);
		a_countDownInstance.start();
		countdownText.text = "" + time;
        time--;
    }

    private void EndCountdown()
    {
		TimeManager.GetInstance.countdownFinished = true;

		Destroy(gameObject);
    }
}
