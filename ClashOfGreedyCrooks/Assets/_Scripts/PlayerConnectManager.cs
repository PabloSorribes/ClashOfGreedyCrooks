using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerConnectManager : MonoBehaviour {

    private static PlayerConnectManager instance;
    public static PlayerConnectManager GetInstance()
    {
        return instance;
    }

    private GameObject canvas;
    private Transform[] playerSlots;
    private ConnectedPlayer[] connectedPlayers;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InstantiateCanvas();
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
        }
    }

    public void AddPlayer(int gamepadIndex)
    {
        foreach (ConnectedPlayer item in connectedPlayers)
            if (item.connected && item.gamepadIndex == gamepadIndex)
                return;

        for (int i = 0; i < connectedPlayers.Length; i++)
            if (!connectedPlayers[i].connected)
            {
                playerSlots[i].GetChild(0).gameObject.SetActive(false);
                playerSlots[i].GetChild(1).gameObject.SetActive(true);

                ConnectedPlayer newConnectedPlayer = new ConnectedPlayer();
                newConnectedPlayer.connected = true;
                newConnectedPlayer.playerIndex = i;
                newConnectedPlayer.gamepadIndex = gamepadIndex;
                newConnectedPlayer.avatar = playerSlots[i].GetChild(1).GetComponent<Image>().color;
                connectedPlayers[i] = newConnectedPlayer;
                return;
            }
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
            }
        }
    }
    
    //TODO: Save data from ConnectedPlayers to GameManager
    public void GoToPickingPhase()
    {
        GameStateManager.GetInstance().SetState(GameStateManager.State.Picking);
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
        public int playerIndex;
        public int gamepadIndex;
        public Color avatar;
    }
}
