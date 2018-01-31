using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerConnectManager : MonoBehaviour {

    private GameObject canvas;
    private Transform[] playerSlots;
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

    //Finds all slots in the canvas where players can connect.
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
        if (connectedPlayers.Count == playerSlots.Length)
            return;

        playerSlots[connectedPlayers.Count].GetChild(0).gameObject.SetActive(false);
        playerSlots[connectedPlayers.Count].GetChild(1).gameObject.SetActive(true);

        ConnectedPlayer newConnectedPlayer = new ConnectedPlayer();
        newConnectedPlayer.joystick = null;
        newConnectedPlayer.avatar = playerSlots[connectedPlayers.Count].GetChild(1).GetComponent<Image>().color;
        connectedPlayers.Add(newConnectedPlayer);
    }

    //TODO: Remove correct player based on joystick
    public void RemovePlayer()
    {
        if (connectedPlayers.Count == 0)
            return;

        playerSlots[connectedPlayers.Count - 1].GetChild(0).gameObject.SetActive(true);
        playerSlots[connectedPlayers.Count - 1].GetChild(1).gameObject.SetActive(false);

        connectedPlayers.RemoveAt(connectedPlayers.Count - 1);
    }
    
    //TODO: Save data from ConnectedPlayers to GameManager
    public void GoToPickingPhase()
    {

    }

    //Input is for testing.
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
