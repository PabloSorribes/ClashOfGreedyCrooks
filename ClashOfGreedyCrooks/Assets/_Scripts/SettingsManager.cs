using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour {

	public Slider volumeSlider;
	public Button saveSettingsButton;

	private void OnEnable() {
		//Get the last Volume-value
		volumeSlider.value = PlayerPrefs.GetFloat("Volume");

		//Fires an event when the slider is changed.
		volumeSlider.onValueChanged.AddListener(delegate { OnVolumeChanged(); });


		saveSettingsButton.onClick.AddListener(delegate { SaveSettings(); });
	}

	void OnVolumeChanged() {
		AudioListener.volume = volumeSlider.value;
		PlayerPrefs.SetFloat("Volume", volumeSlider.value);
	}

	/// <summary>
	/// For a Save-button to use. 
	/// </summary>
	public void SaveSettings() {
		PlayerPrefs.Save();
	}
}
