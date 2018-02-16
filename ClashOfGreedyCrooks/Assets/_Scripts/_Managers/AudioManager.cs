using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : GenericSingleton<AudioManager>
{

	private FMODUnity.StudioEventEmitter musicMainMenu;
	private FMODUnity.StudioEventEmitter musicPicking;
	private FMODUnity.StudioEventEmitter musicArena;

	#region BUSSES
	public enum AudioBusses { mainBus, musicBus, sfxBus }

	private FMOD.Studio.Bus masterBus;
	private FMOD.Studio.Bus musicBus;
	private FMOD.Studio.Bus sfxBus;
	bool helloMute;

	float busVolumeToSet = 1;

	#endregion BUSSES

	private void Awake()
	{
		InitializeBuses();
		InitializeMusic();
	}

	private void InitializeMusic()
	{
		musicMainMenu = gameObject.AddComponent<FMODUnity.StudioEventEmitter>();
		musicMainMenu.Event = "event:/Music/musicMainMenu";

		musicPicking = gameObject.AddComponent<FMODUnity.StudioEventEmitter>();
		musicPicking.Event = "event:/Music/musicPicking";

		musicArena = gameObject.AddComponent<FMODUnity.StudioEventEmitter>();
		musicArena.Event = "event:/Music/musicArena";
	}

	private void InitializeBuses()
	{
		masterBus = FMODUnity.RuntimeManager.GetBus("bus:/MAIN");
		musicBus = FMODUnity.RuntimeManager.GetBus("bus:/MAIN/MU");
		sfxBus = FMODUnity.RuntimeManager.GetBus("bus:/MAIN/SFX");
	}

	private void Start()
	{
		OnGameStateChanged(GameStateManager.GetInstance.GetState());
		GameStateManager.GetInstance.GameStateChanged += OnGameStateChanged;

		//MuteAudio(AudioBusses.musicBus, true);
	}

	//private void Update()
	//{
	//	print("Music: " + musicMainMenu.IsPlaying());
	//	print("Mute: " + masterBus.getMute(out helloMute));
	//}

	private void OnGameStateChanged(GameState newGameState)
	{
		switch (newGameState)
		{
			case GameState.MainMenu:
				PlayMusic(musicMainMenu, musicPicking, musicArena);
				break;
			case GameState.PlayerConnect:
				PlayMusic(musicMainMenu, musicPicking, musicArena);
				break;
			case GameState.Picking:
				PlayMusic(musicPicking, musicMainMenu, musicArena);
				break;
			case GameState.Arena:
				PlayMusic(musicArena, musicMainMenu, musicPicking);
				break;
			default:
				break;
		}
	}

	public void PlayMusic(FMODUnity.StudioEventEmitter musicToPlay, FMODUnity.StudioEventEmitter musicToStop1, FMODUnity.StudioEventEmitter musicToStop2)
	{
		musicToStop1.Stop();
		musicToStop2.Stop();

		//for (int i = 0; i < musicToStop.Length; i++)
		//{
		//	musicToStop[i].Stop();
		//}

		if (!musicToPlay.IsPlaying())
		{
			musicToPlay.Play();
		}
	}

	/// <summary>
	/// <paramref name="mute"/> → TRUE for turning off audio. FALSE for turning it back on.
	/// </summary>
	/// <param name="mute"></param>
	public void MuteAudio(AudioBusses busToMute, bool mute)
	{
		switch (busToMute)
		{
			case AudioBusses.mainBus:
				masterBus.setMute(mute);
				break;
			case AudioBusses.musicBus:
				musicBus.setMute(mute);
				break;
			case AudioBusses.sfxBus:
				sfxBus.setMute(mute);
				break;
			default:
				break;
		}
	}

	/// <summary>
	/// "<paramref name="volumeToChangeWith"/>" should preferably be something small, such as 0.15f (volume increase) or -0.15f (volume decrease)
	/// <para></para> 0 == No volume, -80 dB. 
	/// <para></para> 1 == Max volume, 0 dB (or the level set in the Fmod-project?) 
	/// </summary>
	/// <param name="volumeToChangeWith"></param>
	public void SetAudioVolume(AudioBusses busToChangeVolumeOn, float volumeToChangeWith)
	{
		busVolumeToSet += volumeToChangeWith;

		if (busVolumeToSet > 1)
			busVolumeToSet = 1;

		if (busVolumeToSet < 0)
			busVolumeToSet = 0;

		switch (busToChangeVolumeOn)
		{
			case AudioBusses.mainBus:
				masterBus.setVolume(busVolumeToSet);
				break;
			case AudioBusses.musicBus:
				musicBus.setVolume(busVolumeToSet);
				break;
			case AudioBusses.sfxBus:
				sfxBus.setVolume(busVolumeToSet);
				break;
			default:
				break;
		}
		//print(busVolumeToSet);
	}

	/// <summary>
	/// Returns an instance to use in OneShot-functions.
	/// </summary>
	/// <param name="eventName"></param>
	/// <returns></returns>
	public FMOD.Studio.EventInstance CreateFmodEventInstance(string eventName)
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

		//For 3D-sound
		//eventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));
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
	/// For playing 3D-events at a set position and setting a parameter before playing a 3D-event.
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

	public FMODUnity.StudioEventEmitter InitializeAudioOnObject(GameObject gameObject, string eventPath)
	{

		var fmodEventEmitter = gameObject.AddComponent<FMODUnity.StudioEventEmitter>();
		fmodEventEmitter.Event = eventPath;
		return fmodEventEmitter;
		//TODO: Add the created event to an array or something.
	}

	/// <summary>
	/// Either play or stop the inserted StudioEventEmitter-component.
	/// <para></para>
	/// TRUE = Play sound.
	/// <para></para> FALSE = Stop the sound.
	/// </summary>
	/// <param name="p_fmodComponent"></param>
	public void PlayStopSound(FMODUnity.StudioEventEmitter p_fmodComponent, bool p_playStop)
	{

		if (p_playStop)
		{
			p_fmodComponent.Play();
		}
		else
		{
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
