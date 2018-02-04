using UnityEngine;
using UnityEngine.UI;

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

	private GameManager gm;
    private GameObject canvas;
    private Transform[] playerSlots;
    private GameObject startGameText;
    private ConnectedPlayer[] connectedPlayers;
    private bool allReady;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        gm = GameManager.GetInstance.GetComponent<GameManager>();
        InstantiateCanvas();
        startGameText = canvas.transform.Find("StartText").gameObject;
        startGameText.SetActive(false);
        FillPlayerSlotsArray();
        FillConnectedPlayersArray();
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

    //Fill ConnectedPlayers with empty ConnectedPlayer and their "bool connected" set to false.
    private void FillConnectedPlayersArray()
    {
        connectedPlayers = new ConnectedPlayer[playerSlots.Length];
        for (int i = 0; i < connectedPlayers.Length; i++)
        {
            connectedPlayers[i].connected = false;
            connectedPlayers[i].ready = false;
            connectedPlayers[i].gamepadIndex = 99;
            connectedPlayers[i].avatar = Color.black;
        }
    }

    /// <summary>
    /// Called from InputManager.
    /// </summary>
    /// <param name="gamepadIndex"></param>
    public void AddPlayer(int gamepadIndex)
    {
        if (allReady)
        {
            GoToPickingPhase();
            return;
        }

        //Check if gamepad is connected. If connected set ready. Else if connected and ready end function.
        for (int i = 0; i < connectedPlayers.Length; i++)
        {
            if (connectedPlayers[i].gamepadIndex == gamepadIndex)
            {
                if (!connectedPlayers[i].ready)
                {
                    Ready(i);
                    ReadyCheck();
                    return;
                }
                else if (connectedPlayers[i].ready)
                    return;
            }
        }

        //Assign gamepad to first available player slot.
        for (int i = 0; i < connectedPlayers.Length; i++)
        {
            if (!connectedPlayers[i].connected)
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
        ConnectedPlayer newConnectedPlayer = new ConnectedPlayer
        {
            connected = true,
            gamepadIndex = gamepadIndex,
            avatar = playerSlots[playerIndex].GetChild(1).GetComponent<Image>().color
        };
        connectedPlayers[playerIndex] = newConnectedPlayer;

        if (startGameText.activeInHierarchy)
            startGameText.SetActive(false);
    }

    /// <summary>
    /// Called from InputManager.
    /// </summary>
    /// <param name="gamepadIndex"></param>
    public void RemovePlayer(int gamepadIndex)
    {
        for (int i = 0; i < connectedPlayers.Length; i++)
            if (connectedPlayers[i].gamepadIndex == gamepadIndex)
                if (connectedPlayers[i].ready)
                {
                    UnReady(i);
                    return;
                }
                else if (!connectedPlayers[i].ready)
                {
                    OnRemovePlayer(gamepadIndex);
                    return;
                }
    }

    public void OnRemovePlayer(int gamepadIndex)
    {
        for (int i = 0; i < connectedPlayers.Length; i++)
            if (connectedPlayers[i].gamepadIndex == gamepadIndex)
            {
                playerSlots[i].GetChild(0).gameObject.SetActive(true);
                playerSlots[i].GetChild(1).gameObject.SetActive(false);
                connectedPlayers[i] = new ConnectedPlayer
                {
                    connected = false,
                    ready = false,
                    gamepadIndex = 99,
                    avatar = Color.black
                };
            }
    }

    private void Ready(int pos)
    {
        connectedPlayers[pos].ready = true;
        playerSlots[pos].GetChild(2).gameObject.SetActive(true);
    }

    private void UnReady(int pos)
    {
        connectedPlayers[pos].ready = false;
        playerSlots[pos].GetChild(2).gameObject.SetActive(false);
        allReady = false;
        if (startGameText.activeInHierarchy)
            startGameText.SetActive(false);
    }

    private void ReadyCheck()
    {
        int connections = 0;
        int connectedAndReady = 0;

        for (int i = 0; i < connectedPlayers.Length; i++)

            if (connectedPlayers[i].connected)
            {
                connections++;
                if (connectedPlayers[i].ready)
                    connectedAndReady++;
            }

        if (connections == connectedAndReady && connections > 1)
        {
            allReady = true;
            startGameText.SetActive(true);
        }
    }

    public void GoToPickingPhase()
    {
        for (int i = 0; i < connectedPlayers.Length; i++)
        {
            if (connectedPlayers[i].connected)
                gm.AddPlayer(i, connectedPlayers[i].gamepadIndex, connectedPlayers[i].avatar);
        }

        GameStateManager.GetInstance.SetState(GameState.Picking);
    }

    struct ConnectedPlayer
    {
        public bool connected;
        public bool ready;
        public int gamepadIndex;
        public Color avatar;
    }
}
