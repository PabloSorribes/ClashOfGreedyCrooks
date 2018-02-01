using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager GetInstance()
    {
        return instance;
    }

    private Player[] players = new Player[4];

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneChanged;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        if (instance == null)
        {
            instance = this;
        }
        else if (FindObjectOfType<GameStateManager>().gameObject != this.gameObject)
        {
            Destroy(FindObjectOfType<GameStateManager>().gameObject);
        }

        FillConnectedPlayersArray();
    }

    //Fill ConnectedPlayers with empty ConnectedPlayer and their "bool connected" set to false.
    private void FillConnectedPlayersArray()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].connected = false;
            players[i].gamepadIndex = 99;
            players[i].avatar = Color.black;
        }
    }

    /// <summary>
    /// Call from PlayerConnectManager.
    /// </summary>
    /// <param name="playerIndex"></param>
    /// <param name="gamepadIndex"></param>
    /// <param name="avatar"></param>
	public void AddPlayer(int playerIndex, int gamepadIndex, Color avatar)
    {
        players[playerIndex].connected = true;
        players[playerIndex].gamepadIndex = gamepadIndex;
        players[playerIndex].avatar = avatar;
    }

    /// <summary>
    /// Call from PickingManager.
    /// </summary>
    /// <param name="playerIndex"></param>
    /// <param name="champion"></param>
    public void AddPlayer(int playerIndex, GameObject champion)
    {
        players[playerIndex].champion = champion;
    }

    public void RemovePlayer(int playerIndex)
    {

    }

    public int GetPlayersConnected()
    {
        int count = 0;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].connected)
                count++;
        }
        return count;
    }

    public void ResetGame()
    {

    }

    private void OnSceneChanged(Scene newScene, LoadSceneMode loadingMode)
    {
        if (newScene.name == "Picking")
        {
            for (int i = 0; i < players.Length; i++)
            {
                PickingManager.GetInstance().SetPlayerInfo(players[i].connected, players[i].avatar, i, players[i].gamepadIndex);
            }
        }

        if (newScene.name == "Arena01")
        {
            SpawnPlayers();
            SendInfoToInputManager();
        }
    }

    private void SpawnPlayers()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].connected)
            {
                GameObject newPlayer = Resources.Load("PlayerPrefab") as GameObject;
                GameObject spawnedPlayer = Instantiate(newPlayer);
                players[i].playerObject = spawnedPlayer;
                players[i].pc = spawnedPlayer.GetComponent<PlayerController>();
            }
        }
    }

    private void SendInfoToInputManager()
    {
        Player[] newPlayersArray = new Player[players.Length];

        //Copy Players to newPlayersArray
        for (int i = 0; i < players.Length; i++)
        {
            newPlayersArray[i] = players[i];
        }

        Player playerToSort;
        //Sort by gameIndex
        for (int i = 0; i < newPlayersArray.Length; i++)
        {
            for (int j = 0; j < newPlayersArray.Length; j++)
            {
                if (newPlayersArray[i].gamepadIndex > newPlayersArray[j].gamepadIndex)
                {
                    playerToSort = newPlayersArray[j];
                    newPlayersArray[j] = newPlayersArray[i];
                    newPlayersArray[i] = playerToSort;
                }
            }
        }

        PlayerController[] pcArray = new PlayerController[newPlayersArray.Length];

        for (int i = 0; i < pcArray.Length; i++)
        {
            pcArray[i] = newPlayersArray[i].pc;
        }

        InputManager.instance.SetPlayerReferences(pcArray);
    }

    private struct Player
    {
        public bool connected;
        public int gamepadIndex;
        public Color avatar;
        public GameObject champion;
        public GameObject playerObject;
        public PlayerController pc;
    }
}
