using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : GenericSingleton<AudioManager> {

	private FMODUnity.StudioEventEmitter musicMainMenu;
	public FMODUnity.StudioEventEmitter MusicMainMenu { get; set; }


	private FMODUnity.StudioEventEmitter musicPicking;
	private FMODUnity.StudioEventEmitter musicArena;

	private void Awake() {
		InitializeAudio();
	}

	private void Start() {
		OnGameStateChanged(GameStateManager.GetInstance.GetState());
		GameStateManager.GetInstance.GameStateChanged += OnGameStateChanged;
	}

	private void OnGameStateChanged(GameState newGameState) {
		switch (newGameState) {
			case GameState.MainMenu:
				PlayMusicMainMenu();
				break;
			case GameState.PlayerConnect:
				PlayMusicMainMenu();
				break;
			case GameState.Picking:
				PlayMusicPicking();
				break;
			case GameState.Arena:
				PlayMusicArena();
				break;
			default:
				break;
		}
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.O)) {
			PlayMusicMainMenu();
			//musicMainMenu.Play();
		}
	}

	private void InitializeAudio() {
		musicMainMenu = gameObject.AddComponent<FMODUnity.StudioEventEmitter>();
		musicMainMenu.Event = "event:/Music/musicMainMenu";

		//TODO: look this post up http://www.fmod.org/questions/question/get-parameter-value/ on how to get current value of a parameter from a StudioEventEmitter-object.
		//musicMainMenu.EventInstance.getParameter()

		musicPicking = gameObject.AddComponent<FMODUnity.StudioEventEmitter>();
		musicPicking.Event = "event:/Music/musicPicking";

		musicArena = gameObject.AddComponent<FMODUnity.StudioEventEmitter>();
		musicArena.Event = "event:/Music/musicArena";
	}

	public void PlayMusicMainMenu() {
		musicArena.Stop();
		musicPicking.Stop();

		if (!musicMainMenu.IsPlaying()) {
			musicMainMenu.Play();
		}
	}

	public void PlayMusicPicking() {

		musicArena.Stop();
		musicMainMenu.Stop();

		if (!musicPicking.IsPlaying()) {
			musicPicking.Play();
		}
	}

	public void PlayMusicArena() {

		musicPicking.Stop();
		musicMainMenu.Stop();

		if (!musicArena.IsPlaying()) {
			musicArena.Play();
		}
	}

	public void InitializeAudio(FMODUnity.StudioEventEmitter p_fmodEvent, string p_eventPath) {
		p_fmodEvent = gameObject.AddComponent<FMODUnity.StudioEventEmitter>();
		p_fmodEvent.Event = p_eventPath;

		//TODO: Add the created event to an array or something.
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

	/// <summary>
	/// Changes parameter values for the FMODUnity.StudioEventEmitter-object that is passed into the function.
	/// <para></para>
	/// This will only work for events that are currently playing, ie. you cannot set this before playing a OneShot. Use FMOD.Studio.CreateInstance for that instead.
	/// </summary>
	/// <param name="eventEmitter"></param>
	/// <param name="parameterName"></param>
	/// <param name="parameterValue"></param>
	public void ChangeParameter(FMODUnity.StudioEventEmitter eventEmitter, string parameterName, float parameterValue)
	{
		eventEmitter.SetParameter(parameterName, parameterValue);
	}
}
