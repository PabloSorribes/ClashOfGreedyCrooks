using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour {

    private Text countdownText;

    private int time;
    private string endText;

	private FMODUnity.StudioEventEmitter a_countDown;

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
		a_countDown = gameObject.AddComponent<FMODUnity.StudioEventEmitter>();
		a_countDown.Event = "event:/Arena/countDown";
		a_countDown.SetParameter("end", 1f);

		//Debug.Log(a_countDown.Params[0]);

	}

	private void CountdownTimer()
    {

        if (time == 0)
        {
			a_countDown.SetParameter("end", 1f);
			//Debug.Log(a_countDown.Params[0]);
			a_countDown.Play();

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
