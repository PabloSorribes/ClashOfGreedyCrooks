using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : GenericSingleton<AudioManager> {
	FMODUnity.BankRefAttribute bank;

	FMODUnity.StudioEventEmitter a_connectController;

	private void Start() {
		InitializeAudio();
	}

	private void InitializeAudio() {

		switch (GameStateManager.GetInstance.GetState()) {
			case GameState.MainMenu:
				break;
			case GameState.PlayerConnect:
				a_connectController = gameObject.AddComponent<FMODUnity.StudioEventEmitter>();
				a_connectController.Event = "event:/PlayerConnect/connectController";
				a_connectController.Preload = true;



				break;
			case GameState.Picking:
				break;
			case GameState.Arena:
				break;
			default:
				break;
		}

	}

	public void InitializeAudio(FMODUnity.StudioEventEmitter p_fmodEvent) {
		p_fmodEvent = gameObject.AddComponent<FMODUnity.StudioEventEmitter>();
		p_fmodEvent.Event = "event:/Arena/playerDeath";
		p_fmodEvent.Preload = true;
	}

	/// <summary>
	/// Either play or stop the inserted StudioEventEmitter-component.
	/// <para></para>
	/// TRUE = Play sound.
	/// <para></para> FALSE = Stop the sound.
	/// </summary>
	/// <param name="p_fmodComponent"></param>
	public void PlayStopSound(FMODUnity.StudioEventEmitter p_fmodComponent, bool p_playStop) {

		if (p_playStop) {
			p_fmodComponent.Play();
		}
		else {
			p_fmodComponent.Stop();
		}
	}
}
