using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour {

    private Text countdownText;

    private int time;
    private string endText;

	FMOD.Studio.EventInstance a_countDownInstance;

	private void Start()
    {
		InitializeAudio();
        countdownText = GetComponentInChildren<Text>();
        InvokeRepeating("CountdownTimer", 0f, 1f);
    }

    public void InitializeCountdown(int time, string endText)
    {
        this.time = time;
        this.endText = endText;
    }

	private void InitializeAudio() 
	{
		a_countDownInstance = FMODUnity.RuntimeManager.CreateInstance("event:/Arena/countDown");
	}

	private void CountdownTimer()
    {
		//AudioManager.GetInstance.PlayOneShot3D()


		if (time == 0)
        {
			a_countDownInstance.setParameterValue("end", 1f);
			a_countDownInstance.start();

            CancelInvoke();
            countdownText.text = endText;
            Invoke("EndCountdown", 2f);
            InputManager.GetInstance.freezeInput = false;
            return;
        }
		print(time);
		a_countDown.Play();
        countdownText.text = time.ToString();
        time--;
    }

    private void EndCountdown()
    {
        if(GameStateManager.GetInstance.GetState() == GameState.Arena)
		    TimeManager.GetInstance.countdownFinished = true;

		Destroy(gameObject);
    }

}
