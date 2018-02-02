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

        for (int i = 0; i < players.Length; i++)
        {
            players[i].gamepadIndex = 99;
        }
    }

    //Fill ConnectedPlayers with empty ConnectedPlayer and their "bool connected" set to false.
    //private void FillConnectedPlayersArray()
    //{
    //    for (int i = 0; i < players.Length; i++)
    //    {
    //        players[i].connected = false;
    //        players[i].gamepadIndex = 99;
    //        players[i].avatar = Color.black;
    //    }
    //}

    /// <summary>
    /// Call from PlayerConnectManager.
    /// </summary>
    /// <param name="playerIndex"></param>
    /// <param name="gamepadIndex"></param>
    /// <param name="avatar"></param>
	public void AddPlayer(int playerIndex, int gamepadIndex, Color avatar)
    {
        Player newPlayer = new Player
        {
            connected = true,
            gamepadIndex = gamepadIndex,
            avatar = avatar
        };
        players[playerIndex] = newPlayer;
    }

    /// <summary>
    /// Call from PickingManager.
    /// </summary>
    /// <param name="playerIndex"></param>
    /// <param name="champion"></param>
    public void AddChampion(int playerIndex, GameObject champion)
    {
        players[playerIndex].champion = champion;
    }

    public int GetPlayersConnected()
    {
        int count = 0;
        for (int i = 0; i < players.Length; i++)
            if (players[i].connected)
                count++;
        return count;
    }

    private void OnSceneChanged(Scene newScene, LoadSceneMode loadingMode)
    {
        if (newScene.name == "Picking")
        {
            for (int i = 0; i < players.Length; i++)
                PickingManager.GetInstance().SetPlayerInfo(players[i].connected, players[i].avatar, i, players[i].gamepadIndex);
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

                Debug.Log("SpawnPlayers: " + players[i].pc);
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

        //Sort by gameIndex
        Player playerToSort;
        for (int i = 0; i < newPlayersArray.Length - 1; i++)
        {
            for (int j = i + 1; j < newPlayersArray.Length; j++)
            {
                if (newPlayersArray[i].gamepadIndex > newPlayersArray[j].gamepadIndex)
                {
                    playerToSort = newPlayersArray[j];
                    newPlayersArray[j] = newPlayersArray[i];
                    newPlayersArray[i] = playerToSort;
                }
            }
            Debug.Log("newPlayersArray gamepadIndex" + newPlayersArray[i].gamepadIndex);
            Debug.Log("newPlayersArray playerController: " + newPlayersArray[i].pc);
        }

        PlayerController[] pcArray = new PlayerController[GetPlayersConnected()];
        for (int i = 0; i < pcArray.Length; i++)
        {
            pcArray[i] = newPlayersArray[i].pc;
            Debug.Log("pcArray player controllers: " + pcArray[i]);
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
