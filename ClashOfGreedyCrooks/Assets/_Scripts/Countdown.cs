using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour {

    private Text countdownText;

    public int time;

    private void Start()
    {
        countdownText = GetComponentInChildren<Text>();
        InvokeRepeating("CountdownTimer", 0f, 1f);
    }

    private void CountdownTimer()
    {
        if (time == 0)
        {
            CancelInvoke();
            countdownText.text = "Fight!";
            Invoke("EndCountdown", 2f);
            InputManager.GetInstance.freezeInput = false;
            return;
        }

        countdownText.text = "" + time;
        time--;
    }

    private void EndCountdown()
    {
		TimeManager.GetInstance.countdownFinished = true;

		Destroy(gameObject);
    }
}
