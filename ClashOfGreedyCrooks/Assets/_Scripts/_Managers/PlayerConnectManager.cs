using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
using System.Linq;

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

    private Sprite[] avatarColors;
    private Sprite[] avatarSymbols;
    private GameObject canvas;
    private Transform[] playerSlots;
    private GameObject startGameText, backText;
    private bool noConnections;
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
        LoadResources();
		InitializeAudio();
        InstantiateCanvas();
        startGameText = canvas.transform.Find("StartGame").gameObject;
        backText = canvas.transform.Find("Back").gameObject;
        startGameText.SetActive(false);
        FillPlayerSlotsArray();
        PlayerManager.FillPlayersArray();
        noConnections = true;
    }

    private void LoadResources()
    {
        avatarColors = Resources.LoadAll("UI/Avatars", typeof(Sprite)).Cast<Sprite>().ToArray();
        avatarSymbols = Resources.LoadAll("UI/PlayerColors", typeof(Sprite)).Cast<Sprite>().ToArray();
    }

	private void InitializeAudio() 
	{
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
                ReadyCheck();
                return;
            }
        }
    }

    public void OnAddPlayer(int playerIndex, int gamepadIndex)
    {
        playerSlots[playerIndex].Find("Connected").gameObject.SetActive(true);
        playerSlots[playerIndex].Find("Symbol").gameObject.SetActive(true);

        PlayerManager.players[playerIndex].Connected = true;
        PlayerManager.players[playerIndex].Player = playerIndex;
        PlayerManager.players[playerIndex].Gamepad = gamepadIndex;
        PlayerManager.players[playerIndex].AvatarSymbol = playerSlots[playerIndex].Find("Symbol").GetComponent<Image>().sprite.name;

		a_connectController.Play();
    }

    /// <summary>
    /// Called from InputManager. UnReadies / Disconnects player, depending on its state.
    /// </summary>
    /// <param name="gamepadIndex"></param>
    public void RemovePlayer(int gamepadIndex)
    {
        if (noConnections)
            GameStateManager.GetInstance.SetState(GameState.MainMenu);

        for (int i = 0; i < PlayerManager.players.Length; i++)
            if (PlayerManager.players[i].Gamepad == gamepadIndex)
                if (PlayerManager.players[i].Ready)
                {
                    UnReady(i);
                    ReadyCheck();
                    return;
                }
                else if (!PlayerManager.players[i].Ready && !noConnections)
                {
                    OnRemovePlayer(gamepadIndex);
                    ReadyCheck();
                    return;
                }
    }

    public void OnRemovePlayer(int gamepadIndex)
    {
        for (int i = 0; i < PlayerManager.players.Length; i++)
            if (PlayerManager.players[i].Gamepad == gamepadIndex)
            {
                playerSlots[i].Find("Connected").gameObject.SetActive(false);
                playerSlots[i].Find("Symbol").gameObject.SetActive(false);

                PlayerManager.players[i].Connected = false;
                PlayerManager.players[i].Player = 99;
                PlayerManager.players[i].Gamepad = 99;
            }

		a_disconnectController.Play();
    }

    private void Ready(int pos)
    {
        PlayerManager.players[pos].Ready = true;
        playerSlots[pos].Find("Ready").gameObject.SetActive(true);

        a_ready.Play();
    }

    private void UnReady(int pos)
    {
        PlayerManager.players[pos].Ready = false;
        playerSlots[pos].Find("Ready").gameObject.SetActive(false);

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
            noConnections = false;
            allReady = true;
            startGameText.SetActive(true);
            backText.SetActive(false);
        }
        else if (connections == connectedAndReady && SetTrueFor1PlayerTesting && connections > 0)
        {
            noConnections = false;
            allReady = true;
            backText.SetActive(false);
            startGameText.SetActive(true);
        }
        else if (connections == 0 && connectedAndReady == 0)
        {
            noConnections = true;
            allReady = false;
            backText.SetActive(true);
            startGameText.SetActive(false);
        }
        else if (connections != connectedAndReady)
        {
            noConnections = false;
            allReady = false;
            backText.SetActive(false);
            startGameText.SetActive(false);
        }
    }

    public void GoToPickingPhase()
    {
		a_playerConnectToPicking.Play();
        PlayerManager.SaveConnectedPlayers();
        GameStateManager.GetInstance.SetState(GameState.Picking);
    }
}
