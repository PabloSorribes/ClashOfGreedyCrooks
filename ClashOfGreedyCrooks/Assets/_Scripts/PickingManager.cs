using UnityEngine;
using System.Linq;

public class PickingManager : MonoBehaviour {

    private GameObject arena;
    private Transform[] spawnPositions;
    private Transform[] playerPositions;
    private GameObject[] championPrefabs;
    private GameObject[] weaponPrefabs;
    private Champion[] spawnedChampions;
    private int playersConnected;

    private void Start()
    {
        playersConnected = GameManager.GetInstance().GetPlayersCount();
        LoadResources();
        Instantiate(arena);
        GetPositions();
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
        championPrefabs = Resources.LoadAll("Champions", typeof(Object)).Cast<GameObject>().ToArray();
        weaponPrefabs = Resources.LoadAll("Weapons", typeof(Object)).Cast<GameObject>().ToArray();
    }

    /// <summary>
    /// Finds the childs in waypoint holder in the instantiated arena.
    /// </summary>
    private void GetPositions()
    {
        Transform waypointsParent;
        waypointsParent = arena.transform.Find("Waypoints");
        spawnPositions = new Transform[waypointsParent.childCount / 2];
        playerPositions = new Transform[waypointsParent.childCount / 2];
        for (int i = 0; i < spawnPositions.Length; i++)
            spawnPositions[i] = waypointsParent.GetChild(i);
        for (int i = 0; i < playerPositions.Length; i++)
            playerPositions[i] = waypointsParent.GetChild(i + spawnPositions.Length);
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
        spawnedChampions = new Champion[playersConnected];
        for (int i = 0; i < playersConnected; i++)
        {
            GameObject newChampion = Instantiate(championPrefabs[i], spawnPositions[i].position, Quaternion.identity);
            spawnedChampions[i].champion = newChampion;
            spawnedChampions[i].playerIndex = 99;
            spawnedChampions[i].health = 100;
        }
    }

    private void SpawnWeapons()
    {
        for (int i = 0; i < spawnedChampions.Length; i++)
        {
            Instantiate(weaponPrefabs[i], spawnedChampions[i].champion.transform.Find("WeaponHold"));
        }
    }

    public void PickChampion(int playerIndex, int champion)
    {
        foreach (Champion item in spawnedChampions)
        {
            if (item.playerIndex == playerIndex)
                return;
        }

        if (!spawnedChampions[champion].picked)
        {
            spawnedChampions[champion].champion.transform.position = playerPositions[playerIndex].position;
            spawnedChampions[champion].picked = true;
            spawnedChampions[champion].playerIndex = playerIndex;
        }
        else if (spawnedChampions[champion].picked)
        {
            spawnedChampions[champion].champion.transform.position = playerPositions[playerIndex].position;
            spawnedChampions[champion].playerIndex = playerIndex;
            spawnedChampions[champion].health *= .5f;
        }
    }

    private void Update()
    {
        //Player1
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PickChampion(0, 0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PickChampion(0, 1);
        }

        //Player2
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            PickChampion(1, 0);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            PickChampion(1, 1);
        }
    }

    private struct Champion
    {
        public GameObject champion;
        public bool picked;
        public int playerIndex;
        public float health;
        public float movement;
        public float damage;
        public float attackSpeed;
    }

}
