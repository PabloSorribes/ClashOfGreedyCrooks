﻿using UnityEngine;
using UnityEngine.UI;
using FMODUnity;

public class PlayerConnectManager : MonoBehaviour
{
    private static PlayerConnectManager instance;
    public static PlayerConnectManager GetInstance
    {
        get
        {
            return instance;
        }
    }

    private GameObject canvas;
    private Transform[] playerSlots;
    private GameObject startGameText;
    private bool allReady;

    public bool SetTrueFor1PlayerTesting;

	StudioEventEmitter a_connectController;
	StudioEventEmitter a_disconnectController;
	StudioEventEmitter a_ready;
	StudioEventEmitter a_unReady;
	StudioEventEmitter a_playerConnectToPicking;


	private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
		InitializeAudio();
        InstantiateCanvas();
        startGameText = canvas.transform.Find("StartText").gameObject;
        startGameText.SetActive(false);
        FillPlayerSlotsArray();
        PlayerManager.FillPlayersArray();
    }

	private void InitializeAudio() {
		a_connectController = gameObject.AddComponent<FMODUnity.StudioEventEmitter>();
		a_connectController.Event = "event:/PlayerConnect/connectController";

		a_disconnectController = gameObject.AddComponent<FMODUnity.StudioEventEmitter>();
		a_disconnectController.Event = "event:/PlayerConnect/disconnectController";

		a_ready = gameObject.AddComponent<FMODUnity.StudioEventEmitter>();
		a_ready.Event = "event:/PlayerConnect/ready";

		a_unReady = gameObject.AddComponent<FMODUnity.StudioEventEmitter>();
		a_unReady.Event = "event:/PlayerConnect/unready";

		a_playerConnectToPicking = gameObject.AddComponent<FMODUnity.StudioEventEmitter>();
		a_playerConnectToPicking.Event = "event:/PlayerConnect/playerConnectToPicking";
	}

	private void InstantiateCanvas()
    {
        canvas = Instantiate(Resources.Load("PlayerConnect/PlayerConnectCanvas") as GameObject);
    }

    //Finds all slots in the canvas where players can connect.
    private void FillPlayerSlotsArray()
    {
        playerSlots = new Transform[4];
        Transform playerSlot = canvas.transform.Find("PlayerSlots");
        for (int i = 0; i < playerSlot.childCount; i++)
            playerSlots[i] = playerSlot.GetChild(i);
    }

    /// <summary>
    /// Called from InputManager.
    /// </summary>
    /// <param name="gamepadIndex"></param>
    public void AddPlayer(int gamepadIndex)
    {
        if (allReady)
            for (int i = 0; i < PlayerManager.players.Length; i++)
                if (PlayerManager.players[i].Gamepad == gamepadIndex && PlayerManager.players[i].Ready)
                {
                    GoToPickingPhase();
                    return;
                }

        //Check if gamepad is connected. If connected set ready. Else if connected and ready end function.
        for (int i = 0; i < PlayerManager.players.Length; i++)
        {
            if (PlayerManager.players[i].Gamepad == gamepadIndex)
            {
                if (!PlayerManager.players[i].Ready)
                {
                    Ready(i);
                    ReadyCheck();
                    return;
                }
                else if (PlayerManager.players[i].Ready)
                    return;
            }
        }

        //Assign gamepad to first available player slot.
        for (int i = 0; i < PlayerManager.players.Length; i++)
        {
            if (!PlayerManager.players[i].Connected)
            {
                OnAddPlayer(i, gamepadIndex);
                return;
            }
        }
    }

    public void OnAddPlayer(int playerIndex, int gamepadIndex)
    {
        playerSlots[playerIndex].GetChild(0).gameObject.SetActive(false);
        playerSlots[playerIndex].GetChild(1).gameObject.SetActive(true);

        PlayerManager.players[playerIndex].Connected = true;
        PlayerManager.players[playerIndex].Player = playerIndex;
        PlayerManager.players[playerIndex].Gamepad = gamepadIndex;
        PlayerManager.players[playerIndex].Avatar = playerSlots[playerIndex].GetChild(1).GetComponent<Image>().color;

        if (startGameText.activeInHierarchy)
            startGameText.SetActive(false);

		a_connectController.Play();
    }

    /// <summary>
    /// Called from InputManager. UnReadies / Disconnects player, depending on its state.
    /// </summary>
    /// <param name="gamepadIndex"></param>
    public void RemovePlayer(int gamepadIndex)
    {
        for (int i = 0; i < PlayerManager.players.Length; i++)
            if (PlayerManager.players[i].Gamepad == gamepadIndex)
                if (PlayerManager.players[i].Ready)
                {
                    UnReady(i);
                    return;
                }
                else if (!PlayerManager.players[i].Ready)
                {
                    OnRemovePlayer(gamepadIndex);
                    return;
                }
    }

    public void OnRemovePlayer(int gamepadIndex)
    {
        for (int i = 0; i < PlayerManager.players.Length; i++)
            if (PlayerManager.players[i].Gamepad == gamepadIndex)
            {
                playerSlots[i].GetChild(0).gameObject.SetActive(true);
                playerSlots[i].GetChild(1).gameObject.SetActive(false);

                PlayerManager.players[i].Connected = false;
                PlayerManager.players[i].Player = 99;
                PlayerManager.players[i].Gamepad = 99;
            }
        ReadyCheck();

		a_disconnectController.Play();
    }

    private void Ready(int pos)
    {
        PlayerManager.players[pos].Ready = true;
        playerSlots[pos].GetChild(2).gameObject.SetActive(true);

		a_ready.Play();
    }

    private void UnReady(int pos)
    {
        PlayerManager.players[pos].Ready = false;
        playerSlots[pos].GetChild(2).gameObject.SetActive(false);
        allReady = false;
        if (startGameText.activeInHierarchy)
            startGameText.SetActive(false);

		a_unReady.Play();
    }

    private void ReadyCheck()
    {
        int connections = 0;
        int connectedAndReady = 0;

        for (int i = 0; i < PlayerManager.players.Length; i++)

            if (PlayerManager.players[i].Connected)
            {
                connections++;
                if (PlayerManager.players[i].Ready)
                    connectedAndReady++;
            }

        if (connections == connectedAndReady && connections > 1)
        {
            allReady = true;
            startGameText.SetActive(true);
        }
        else if (connections == connectedAndReady && SetTrueFor1PlayerTesting)
        {
            allReady = true;
            startGameText.SetActive(true);
        }
    }

    public void GoToPickingPhase()
    {
		a_playerConnectToPicking.Play();
        PlayerManager.SaveConnectedPlayers();
        GameStateManager.GetInstance.SetState(GameState.Picking);
    }
}
