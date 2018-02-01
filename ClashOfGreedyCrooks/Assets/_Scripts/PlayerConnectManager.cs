using UnityEngine;
using UnityEngine.UI;

public class PlayerConnectManager : MonoBehaviour
{
    private static PlayerConnectManager instance;
    public static PlayerConnectManager GetInstance()
    {
        return instance;
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
        gm = GameManager.GetInstance().GetComponent<GameManager>();
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

        for (int i = 0; i < connectedPlayers.Length; i++)
        {
            if (connectedPlayers[i].connected && connectedPlayers[i].gamepadIndex == gamepadIndex && !connectedPlayers[i].ready)
            {
                connectedPlayers[i].ready = true;
                playerSlots[i].GetChild(2).gameObject.SetActive(true);
                Ready();
                return;
            }
        }

        for (int i = 0; i < connectedPlayers.Length; i++)
        {
            if (!connectedPlayers[i].connected && connectedPlayers[i].gamepadIndex != gamepadIndex)
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

        ConnectedPlayer newConnectedPlayer = new ConnectedPlayer();
        newConnectedPlayer.connected = true;
        newConnectedPlayer.gamepadIndex = gamepadIndex;
        newConnectedPlayer.avatar = playerSlots[playerIndex].GetChild(1).GetComponent<Image>().color;
        connectedPlayers[playerIndex] = newConnectedPlayer;
        Debug.Log(connectedPlayers[playerIndex].avatar);
        Ready();
    }

    /// <summary>
    /// Called from InputManager.
    /// </summary>
    /// <param name="gamepadIndex"></param>
    public void RemovePlayer(int gamepadIndex)
    {
        for (int i = 0; i < connectedPlayers.Length; i++)
        {
            if (connectedPlayers[i].gamepadIndex == gamepadIndex && connectedPlayers[i].ready)
            {
                connectedPlayers[i].ready = false;
                playerSlots[i].GetChild(2).gameObject.SetActive(false);
                Ready();
                return;
            }
            else if (connectedPlayers[i].gamepadIndex == gamepadIndex && !connectedPlayers[i].ready)
            {
                OnRemovePlayer(gamepadIndex);
                return;
            }
        }
    }

    public void OnRemovePlayer(int gamepadIndex)
    {
        if (connectedPlayers.Length == 0)
            return;

        for (int i = 0; i < connectedPlayers.Length; i++)
        {
            if (connectedPlayers[i].gamepadIndex == gamepadIndex)
            {
                playerSlots[i].GetChild(0).gameObject.SetActive(true);
                playerSlots[i].GetChild(1).gameObject.SetActive(false);
                connectedPlayers[i] = new ConnectedPlayer();
                connectedPlayers[i].connected = false;
                connectedPlayers[i].ready = false;
                connectedPlayers[i].gamepadIndex = 99;
                connectedPlayers[i].avatar = Color.black;
            }
        }
    }

    public void Ready()
    {
        int connections = 0;
        int connectedAndReady = 0;
        foreach (ConnectedPlayer item in connectedPlayers)
        {
            if (item.connected)
            {
                connections++;
                if (item.ready)
                    connectedAndReady++;
            }
        }

        if (connections == connectedAndReady && connections > 1)
        {
            allReady = true;
            startGameText.SetActive(true);
        }
        else if (connections != connectedAndReady)
        {
            allReady = false;
            startGameText.SetActive(false);
        }
    }
    
    public void GoToPickingPhase()
    {
        for (int i = 0; i < connectedPlayers.Length; i++)
        {
            if (connectedPlayers[i].connected)
                gm.AddPlayer(i, connectedPlayers[i].gamepadIndex, connectedPlayers[i].avatar);
        }

        GameStateManager.GetInstance().SetState(State.Picking);
    }

    struct ConnectedPlayer
    {
        public bool connected;
        public bool ready;
        public int gamepadIndex;
        public Color avatar;
    }
}
