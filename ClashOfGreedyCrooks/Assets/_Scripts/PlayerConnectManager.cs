using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerConnectManager : MonoBehaviour
{

    private static PlayerConnectManager instance;
    public static PlayerConnectManager GetInstance()
    {
        return instance;
    }

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
            connectedPlayers[i].gamepadIndex = 99;
        }
    }

    public void AddPlayer(int gamepadIndex)
    {
        foreach (ConnectedPlayer item in connectedPlayers)
        {
            if (item.connected && item.gamepadIndex == gamepadIndex)
            {
                if (Ready())
                    GoToPickingPhase();
                return;
            }
        }

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

        ConnectedPlayer newConnectedPlayer = new ConnectedPlayer();
        newConnectedPlayer.connected = true;
        newConnectedPlayer.playerIndex = playerIndex;
        newConnectedPlayer.gamepadIndex = gamepadIndex;
        newConnectedPlayer.avatar = playerSlots[playerIndex].GetChild(1).GetComponent<Image>().color;
        connectedPlayers[playerIndex] = newConnectedPlayer;
        Ready();
    }

    public void RemovePlayer(int gamepadIndex)
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
                connectedPlayers[i].gamepadIndex = 99;
            }
        }
    }

    public bool Ready()
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

        if (connections == connectedAndReady && connections > 1 && !allReady)
        {
            allReady = true;
            startGameText.SetActive(true);
            return false;
        }
        else if (connections != connectedAndReady && connections > 1)
        {
            allReady = false;
            startGameText.SetActive(false);
            return false;
        }
        else
            return true;
    }

    //TODO: Save data from ConnectedPlayers to GameManager
    public void GoToPickingPhase()
    {
        foreach (ConnectedPlayer cp in connectedPlayers)
        {

        }
        GameStateManager.GetInstance().SetState(State.Picking);
    }

    //Input is for testing.
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AddPlayer(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            AddPlayer(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            AddPlayer(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            RemovePlayer(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            GoToPickingPhase();
        }
    }

    struct ConnectedPlayer
    {
        public bool connected;
        public bool ready;
        public int playerIndex;
        public int gamepadIndex;
        public Color avatar;
    }
}
