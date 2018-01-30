using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerConnectManager : MonoBehaviour {

    private GameObject canvas;
    private Transform[] playerSlots;
    private int connections;
    private List<ConnectedPlayer> connectedPlayers = new List<ConnectedPlayer>();

    private void Start()
    {
        InstantiateCanvas();
        FillPlayerSlotsArray();
    }

    private void InstantiateCanvas()
    {
        canvas = Instantiate(Resources.Load("PlayerConnect/PlayerConnectCanvas") as GameObject);
    }

    private void FillPlayerSlotsArray()
    {
        playerSlots = new Transform[4];
        Transform playerSlot = canvas.transform.Find("PlayerSlots");
        for (int i = 0; i < playerSlot.childCount; i++)
            playerSlots[i] = playerSlot.GetChild(i);
    }

    //TODO: Add correct player based on joystick
    public void AddPlayer()
    {
        if (connections == playerSlots.Length)
            return;

        playerSlots[connections].GetChild(0).gameObject.SetActive(false);
        playerSlots[connections].GetChild(1).gameObject.SetActive(true);

        ConnectedPlayer newConnectedPlayer = new ConnectedPlayer();
        newConnectedPlayer.joystick = null;
        newConnectedPlayer.avatar = playerSlots[connections].GetChild(1).GetComponent<Image>().color;
        connectedPlayers.Add(newConnectedPlayer);

        connections++;
    }

    //TODO: Remove correct player based on joystick
    public void RemovePlayer()
    {
        if (connections == 0)
            return;

        playerSlots[connections-1].GetChild(0).gameObject.SetActive(true);
        playerSlots[connections-1].GetChild(1).gameObject.SetActive(false);

        connectedPlayers.RemoveAt(connections - 1);

        connections--;
    }
    
    //TODO: Save data from ConnectedPlayers to GameManager
    public void GoToPickingPhase()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AddPlayer();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            RemovePlayer();
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            GameStateManager.GetInstance().SetState(GameStateManager.State.Picking);
        }
    }

    struct ConnectedPlayer
    {
        public string joystick;
        public Color avatar;
    }
}
