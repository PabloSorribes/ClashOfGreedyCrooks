using UnityEngine;
using UnityEngine.UI;

public enum BlackScreen { Out, In }

public class ScreenFade : MonoBehaviour
{
	private GameObject blackScreenPrefab;
	private Image blackScreen;

	private BlackScreen dir;
	private bool fading;

	public bool Fading {	get	{ return fading; }
	}

	public System.Action FadeOutComplete;
	public System.Action FadeInComplete;

	private void Start()
	{
		DontDestroyOnLoad(gameObject);
		blackScreenPrefab = Resources.Load("BlackScreen") as GameObject;
		GameObject newCanvas = Instantiate(blackScreenPrefab, transform);
		blackScreen = newCanvas.GetComponentInChildren<Image>();
	}

	private void Update()
	{
		if (!fading)
			return;

		if (dir == BlackScreen.In)
		{
			if (blackScreen.color.a > 0)
			{
				Color col = blackScreen.color;
				col.a -= .01f;
				blackScreen.color = col;
			}
			else
			{
				fading = false;
				InputManager.GetInstance.freezeInput = false;
				FadeInComplete();
			}
		}
		else if (dir == BlackScreen.Out)
		{
			if (blackScreen.color.a < 1)
			{
				Color col = blackScreen.color;
				col.a += .01f;
				blackScreen.color = col;
			}
			else
			{
				fading = false;
				InputManager.GetInstance.freezeInput = false;
				FadeOutComplete();
			}
		}
	}

	public void SetDir(BlackScreen dir)
	{
		InputManager.GetInstance.freezeInput = true;
		this.dir = dir;
		fading = true;
	}
}
