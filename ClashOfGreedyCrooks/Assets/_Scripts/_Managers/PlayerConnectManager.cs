using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;

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

	private string connectSound = "event:/PlayerConnect/connectController";
	private string disconnectSound = "event:/PlayerConnect/disconnectController";
	private string readySound = "event:/PlayerConnect/ready";
	private string unReadySound = "event:/PlayerConnect/unready";
	private string playerConnectToPickingSound = "event:/PlayerConnect/playerConnectToPicking";

	private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        LoadResources();
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
        avatarColors = Resources.LoadAll("UI/PlayerColors", typeof(Sprite)).Cast<Sprite>().ToArray();
        avatarSymbols = Resources.LoadAll("UI/Avatars", typeof(Sprite)).Cast<Sprite>().ToArray();
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

    private bool[] changeSymbolCooldown = new bool[4];
    public void ChangeSymbol(float input, int gamepadIndex)
    {
        if (changeSymbolCooldown[gamepadIndex])
            return;
        else
        {
            changeSymbolCooldown[gamepadIndex] = true;
            StartCoroutine(ResetSymbolCooldown(gamepadIndex));
        }

        for (int i = 0; i < PlayerManager.players.Length; i++)
        {
            PlayerInfo player = PlayerManager.players[i];
            if (player.Connected && player.Gamepad == gamepadIndex && !player.Ready)
                for (int j = 0; j < avatarSymbols.Length; j++)
                    if (player.AvatarSymbol == avatarSymbols[j].name)
                    {
                        int selection = j;
                        selection = ((selection + avatarSymbols.Length + (int)Mathf.Sign(input)) % avatarSymbols.Length);
                        playerSlots[player.Player].Find("Symbol").GetComponent<Image>().sprite = avatarSymbols[selection];
                        player.AvatarSymbol = avatarSymbols[selection].name;

						AudioManager.GetInstance.PlayOneShot("event:/PlayerConnect/avatarScroll");
                        return;
                    }
        }
    }

    private IEnumerator ResetSymbolCooldown(int index)
    {
        yield return new WaitForSeconds(.3f);
        changeSymbolCooldown[index] = false;
    }

    /// <summary>
    /// Called from InputManager.
    /// </summary>
    /// <param name="gamepadIndex"></param>
    public void AddPlayer(int gamepadIndex)
    {
        for (int i = 0; i < PlayerManager.players.Length; i++)
            if (PlayerManager.players[i].Gamepad == gamepadIndex)
                if (PlayerManager.players[i].Ready)
                    return;
                else if (!PlayerManager.players[i].Ready)
                {
                    InputManager.GetInstance.Rumble(gamepadIndex, .5f, 1f, .1f);
                    Ready(i);
                    ReadyCheck();
                    return;
                }

        //Assign gamepad to first available player slot.
        for (int i = 0; i < PlayerManager.players.Length; i++)
            if (!PlayerManager.players[i].Connected)
            {
                InputManager.GetInstance.Rumble(gamepadIndex, .5f, 1f, .1f);
                OnAddPlayer(i, gamepadIndex);
                ReadyCheck();
                return;
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
		PlayerManager.players[playerIndex].AvatarColor = avatarColors[playerIndex].name;

		AudioManager.GetInstance.PlayOneShot(connectSound);
    }

    /// <summary>
    /// Called from InputManager. UnReadies / Disconnects player, depending on its state.
    /// </summary>
    /// <param name="gamepadIndex"></param>
    public void RemovePlayer(int gamepadIndex)
    {
        if (noConnections)
		{
			AudioManager.GetInstance.PlayOneShot("event:/MainMenu/menuDecline");
            GameStateManager.GetInstance.SetState(GameState.MainMenu);
		}

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

		AudioManager.GetInstance.PlayOneShot(disconnectSound);
    }

    private void Ready(int pos)
    {
        PlayerInfo player = PlayerManager.players[pos];
        string symbol = playerSlots[pos].Find("Symbol").GetComponent<Image>().sprite.name;
        for (int i = 0; i < PlayerManager.players.Length; i++)
        {
            if (i != pos && PlayerManager.players[i].AvatarSymbol == symbol && PlayerManager.players[i].Ready)
            {
                Debug.Log("Symbol already taken!");
				AudioManager.GetInstance.PlayOneShot("event:/PlayerConnect/avatarDenied");
                return;
            }
        }
        player.Ready = true;
        player.AvatarSymbol = symbol;
        playerSlots[pos].Find("Ready").gameObject.SetActive(true);
        playerSlots[pos].Find("Locked").gameObject.SetActive(true);

		AudioManager.GetInstance.PlayOneShot(readySound);
	}

    private void UnReady(int pos)
    {
        PlayerManager.players[pos].Ready = false;
        playerSlots[pos].Find("Ready").gameObject.SetActive(false);
        playerSlots[pos].Find("Locked").gameObject.SetActive(false);

		AudioManager.GetInstance.PlayOneShot(unReadySound);
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
        if (allReady)
        {
			AudioManager.GetInstance.PlayOneShot(playerConnectToPickingSound);
            PlayerManager.SaveConnectedPlayers();
            GameStateManager.GetInstance.SetState(GameState.Picking);
        }
    }
}
