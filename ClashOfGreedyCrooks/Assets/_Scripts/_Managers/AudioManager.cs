using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : GenericSingleton<AudioManager> {

	private FMODUnity.StudioEventEmitter musicMainMenu;
	public FMODUnity.StudioEventEmitter MusicMainMenu { get; set; }

	private FMODUnity.StudioEventEmitter musicPicking;
	private FMODUnity.StudioEventEmitter musicArena;

	FMOD.Studio.EventInstance a_buttonSound;

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

	public void PlayMusic(FMODUnity.StudioEventEmitter musicToPlay, FMODUnity.StudioEventEmitter[] musicToStop)
	{
		for (int i = 0; i < musicToStop.Length; i++)
		{
			musicToStop[i].Stop();
		}

		if (!musicToPlay.IsPlaying())
		{
			musicToPlay.Play();
		}
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




	/// <summary>
	/// Return an instance to use in OneShot-functions.
	/// </summary>
	/// <param name="eventName"></param>
	/// <returns></returns>
	private FMOD.Studio.EventInstance CreateFmodEventInstance(string eventName)
	{
		return FMODUnity.RuntimeManager.CreateInstance(eventName);
	}

	/// <summary>
	/// Use for 2D-events. <paramref name="eventPath"/> is a string to the fmod event, eg. "event:/Arena/countDown".
	/// </summary>
	/// <param name="eventPath"></param>
	public void PlayOneShot(string eventPath)
	{
		CreateFmodEventInstance(eventPath).start();
	}

	/// <summary>
	/// For setting a parameter before playing.
	/// </summary>
	/// <param name="eventPath"></param>
	/// <param name="parameterName"></param>
	/// <param name="parameterValue"></param>
	public void PlayOneShot(string eventPath, string parameterName, float parameterValue)
	{
		var eventInstance = CreateFmodEventInstance(eventPath);

		eventInstance.setParameterValue(parameterName, parameterValue);
		eventInstance.start();

		eventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));
	}

	/// <summary>
	/// For playing 3D-events at a set position. "<paramref name="position"/>" could be your transform.position.
	/// </summary>
	/// <param name="eventPath"></param>
	/// <param name="parameterName"></param>
	/// <param name="parameter"></param>
	public void PlayOneShot3D(string eventPath, Vector3 position)
	{
		FMODUnity.RuntimeManager.PlayOneShot(eventPath, position);

		//var eventInstance = CreateFmodEventInstance(eventName);

		//eventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(position));
		//eventInstance.start();
	}

	/// <summary>
	/// For setting a parameter before playing a 3D-event.
	/// </summary>
	/// <param name="eventPath"></param>
	/// <param name="parameterName"></param>
	/// <param name="parameterValue"></param>
	public void PlayOneShot3D(string eventPath, Vector3 position, string parameterName, float parameterValue)
	{
		var eventInstance = CreateFmodEventInstance(eventPath);

		eventInstance.setParameterValue(parameterName, parameterValue);
		eventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(position));
		eventInstance.start();

		FMODUnity.RuntimeManager.PlayOneShot("hej", transform.position);
	}

	public void InitializeAudioOnObject(GameObject gameObject, string eventPath) {

		var fmodEventEmitter = gameObject.AddComponent<FMODUnity.StudioEventEmitter>();
		fmodEventEmitter.Event = eventPath;

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
	public void ChangeEmitterParameter(FMODUnity.StudioEventEmitter eventEmitter, string parameterName, float parameterValue)
	{
		eventEmitter.SetParameter(parameterName, parameterValue);
	}
}
