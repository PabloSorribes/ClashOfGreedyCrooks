using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour {

    private Text countdownText;

    public int time;

	private FMODUnity.StudioEventEmitter a_countDown;

	private void Start()
    {
		InitializeAudio();
        countdownText = GetComponentInChildren<Text>();
        InvokeRepeating("CountdownTimer", 0f, 1f);
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
            countdownText.text = "Fight!";
            Invoke("EndCountdown", 2f);
            InputManager.GetInstance.freezeInput = false;
            return;
        }
		print(time);
		a_countDown.Play();
        countdownText.text = "" + time;
        time--;
    }

    private void EndCountdown()
    {
		TimeManager.GetInstance.countdownFinished = true;

		Destroy(gameObject);
    }
}
