using UnityEngine;
using System.Linq;

public class PickingManager : MonoBehaviour
{
    private static PickingManager instance;
    public static PickingManager GetInstance
	{
		get
		{
			return instance;
		}
	}

    private GameObject arena, spawnedArena;
    private Transform[] spawnPositions = new Transform[4];
    private Transform[] playerPositions = new Transform[4];
    private Transform[] avatars = new Transform[4];
    private GameObject playerPrefab;
    private GameObject[] championPrefabs;
    private GameObject[] weaponPrefabs;
    private GameObject[] spawnedChampions;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        LoadResources();
        GameObject newArena = Instantiate(arena);
        spawnedArena = newArena;
        GetChilds(spawnPositions, "Waypoints/Pool");
        GetChilds(playerPositions, "Waypoints/Picked");
        GetChilds(avatars, "Avatars");
        SetAvatarsPlayers();
        Shuffle(championPrefabs);
        Shuffle(weaponPrefabs);
        SpawnChampions();
        SpawnWeapons();
        //TODO: Players should be able to nerf champions.
    }

    /// <summary>
    /// Loads all resources needed.
    /// </summary>
    private void LoadResources()
    {
        arena = Resources.Load("Arenas/PickingTestArena") as GameObject;
        playerPrefab = Resources.Load("PlayerPrefab") as GameObject;
        championPrefabs = Resources.LoadAll("Champions", typeof(Object)).Cast<GameObject>().ToArray();
        weaponPrefabs = Resources.LoadAll("Weapons", typeof(Object)).Cast<GameObject>().ToArray();
    }

    /// <summary>
    /// Finds the childs in waypoint holder in the instantiated arena.
    /// </summary>
    private void GetChilds(Transform[] array, string path)
    {
        Transform waypointsParent;
        waypointsParent = spawnedArena.transform.Find(path);

        for (int i = 0; i < waypointsParent.childCount; i++)
            array[i] = waypointsParent.GetChild(i);
    }
    
    private void SetAvatarsPlayers()
    {
        for (int i = 0; i < PlayerManager.players.Length; i++)
            if (PlayerManager.players[i].Connected)
                avatars[i].GetComponent<SpriteRenderer>().color = PlayerManager.players[i].Avatar;
            else
                avatars[i].gameObject.SetActive(false);
    }

    /// <summary>
    /// Switching place of items in an array.
    /// </summary>
    /// <param name="array"></param>
    private void Shuffle(GameObject[] array)
    {
        int number;
        GameObject target;
        for (int i = 0; i < array.Length; i++)
        {
            number = Random.Range(0, array.Length);
            target = array[number];
            array[number] = array[i];
            array[i] = target;
        }
    }
    
    private void SpawnChampions()
    {
        spawnedChampions = new GameObject[PlayerManager.GetPlayersConnected()];

        for (int i = 0; i < PlayerManager.GetPlayersConnected(); i++)
            if (PlayerManager.players[i].Connected)
            {
                GameObject newChampion = Instantiate(championPrefabs[i].gameObject, spawnPositions[i].position, Quaternion.identity);
                spawnedChampions[i] = newChampion;
                spawnedChampions[i].GetComponent<Champion>().PlayerIndex = 99;
            }
    }

    private void SpawnWeapons()
    {
        for (int i = 0; i < spawnedChampions.Length; i++)
        {
            GameObject newWeapon = Instantiate(weaponPrefabs[i], spawnedChampions[i].gameObject.transform.Find("WeaponHold"));
            newWeapon.name = weaponPrefabs[i].name;
        }
    }

    public void PickChampion(int gamepadIndex, int button)
    {
        for (int i = 0; i < PlayerManager.players.Length; i++)
            if (PlayerManager.players[i].Gamepad == gamepadIndex)
                if (PlayerManager.players[i].HasChampion)
                    return;
                else
                {
                    OnPickChampion(i, button);
                    return;
                }
    }

    private void OnPickChampion(int playerIndex, int champion)
    {
        foreach (GameObject item in spawnedChampions)
            if (item.GetComponent<Champion>().PlayerIndex == playerIndex)
                return;

        if (!spawnedChampions[champion].GetComponent<Champion>().Picked)
        {
            spawnedChampions[champion].transform.position = playerPositions[playerIndex].position;
            spawnedChampions[champion].GetComponent<Champion>().Picked = true;
            spawnedChampions[champion].GetComponent<Champion>().PlayerIndex = playerIndex;
        }
        else if (spawnedChampions[champion].GetComponent<Champion>().Picked)
        {
            spawnedChampions[champion].transform.position = playerPositions[playerIndex].position;
            spawnedChampions[champion].GetComponent<Champion>().PlayerIndex = playerIndex;
        }

        for (int i = 0; i < spawnedChampions.Length; i++)
            if (!spawnedChampions[i].GetComponent<Champion>().Picked)
                return;
            else
                if (IsAllChampionsPicked())
                    OnAllChampionsPicked();
    }

    private bool IsAllChampionsPicked()
    {
        int picked = 0;
        for (int i = 0; i < spawnedChampions.Length; i++)
            if (spawnedChampions[i].GetComponent<Champion>().Picked)
                picked++;
        if (picked == spawnedChampions.Length)
            return true;
        else
            return false;
    }

    private void OnAllChampionsPicked()
    {
        PlayerManager.SetSpawnedPlayersArrayLenght();
        for (int i = 0; i < PlayerManager.players.Length; i++)
            for (int j = 0; j < spawnedChampions.Length; j++)
                if (spawnedChampions[j].GetComponent<Champion>().PlayerIndex == i)
                    SpawnPlayer(i, j);
        
        PlayerManager.SendInfoToInputManager();
        GameStateManager.GetInstance.SetState(GameState.Arena);
    }

    private void SpawnPlayer(int playerIndex, int championIndex)
    {
        GameObject newPlayer = Instantiate(playerPrefab);
        newPlayer.GetComponent<PlayerInfo>().Player = PlayerManager.players[playerIndex].Player;
        newPlayer.GetComponent<PlayerInfo>().Gamepad = PlayerManager.players[playerIndex].Gamepad;

        spawnedChampions[championIndex].transform.SetParent(newPlayer.transform);
        spawnedChampions[championIndex].transform.localPosition = Vector3.zero;

        PlayerManager.AddSpawnedPlayer(newPlayer);
    }
}
