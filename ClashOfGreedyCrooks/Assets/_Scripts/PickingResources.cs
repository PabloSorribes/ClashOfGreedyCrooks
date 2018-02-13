using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PickingResources : MonoBehaviour {

    #region LoadsFromResources
    //private GameObject arena, spawnedArena;
    private GameObject pickingPositionsPrefab;
    private GameObject cardPrefab, portraitSetupPrefab;
    private CardComponent[] cards;
    private GameObject[] championPrefabs;
    private GameObject[] weaponPrefabs;
    private Sprite[] buttons;
    [HideInInspector] public GameObject playerPrefab;
    #endregion

    //private Transform[] avatars = new Transform[4];
    //private Transform[] spawnPositions = new Transform[4];
    [HideInInspector] public Transform[] playerPositions = new Transform[4];
    [HideInInspector] public Transform pickingPositions;
    private Transform portraitSetup;
    [HideInInspector] public GameObject[] spawnedChampions;

    private void Start()
    {
        LoadResources();
        GameObject newPickingPositions = Instantiate(pickingPositionsPrefab);
        pickingPositions = newPickingPositions.transform;
        PortraitSetup();
        InstantiateCards();
        //GameObject newArena = Instantiate(arena);
        //spawnedArena = newArena;
        //GetChilds(spawnPositions, "Waypoints/Pool");
        //GetChilds(playerPositions, "Waypoints/Picked");
        //GetChilds(avatars, "Avatars");
        //SetAvatarsPlayers();
        Shuffle(championPrefabs);
        Shuffle(weaponPrefabs);
        SpawnChampions();
        SpawnWeapons();
        AssignChampionButton();
    }

    private void LoadResources()
    {
        //arena = Resources.Load("Arenas/PickingTestArena") as GameObject;
        pickingPositionsPrefab = Resources.Load("Picking/PickingPositions") as GameObject;
        portraitSetupPrefab = Resources.Load("Picking/PortraitSetup") as GameObject;
        cardPrefab = Resources.Load("Picking/Card") as GameObject;
        championPrefabs = Resources.LoadAll("Champions", typeof(Object)).Cast<GameObject>().ToArray();
        weaponPrefabs = Resources.LoadAll("Weapons", typeof(Object)).Cast<GameObject>().ToArray();
        buttons = Resources.LoadAll("Picking/Sprites/Buttons", typeof(Sprite)).Cast<Sprite>().ToArray();
        playerPrefab = Resources.Load("PlayerPrefab") as GameObject;
    }

    private void PortraitSetup()
    {
        GameObject newPortraitSetup = Instantiate(portraitSetupPrefab);
        portraitSetup = newPortraitSetup.transform;
    }

    private void InstantiateCards()
    {
        cards = new CardComponent[PlayerManager.GetPlayersConnected()];
        for (int i = 0; i < PlayerManager.GetPlayersConnected(); i++)
        {
            Material mat = new Material(Shader.Find("Standard"));
            RenderTexture rt = new RenderTexture(512, 512, 16);
            mat.mainTexture = rt;
            Vector3 pos = pickingPositions.Find("Pool").GetChild(i).position;
            GameObject newCard = Instantiate(cardPrefab);
            newCard.transform.position = pos;
            cards[i] = newCard.GetComponent<CardComponent>();
            cards[i].portraitRawImage.material = mat;
            portraitSetup.GetChild(i).Find("Camera").GetComponent<Camera>().targetTexture = rt;
        }
    }
    

    //private void GetChilds(Transform[] array, string path)
    //{
    //    Transform waypointsParent;
    //    waypointsParent = spawnedArena.transform.Find(path);

    //    for (int i = 0; i < waypointsParent.childCount; i++)
    //        array[i] = waypointsParent.GetChild(i);
    //}

    //private void SetAvatarsPlayers()
    //{
    //    for (int i = 0; i < PlayerManager.players.Length; i++)
    //        if (PlayerManager.players[i].Connected)
    //            avatars[i].GetComponent<SpriteRenderer>().color = PlayerManager.players[i].Avatar;
    //        else
    //            avatars[i].gameObject.SetActive(false);
    //}

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
                Vector3 pos = portraitSetup.GetChild(i).Find("Champion").position;
                Quaternion rot = portraitSetup.GetChild(i).Find("Champion").rotation;
                GameObject newChampion = Instantiate(championPrefabs[i].gameObject, pos, rot);
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


    /// <summary>
    /// Instantiates a button sprite above champion head.
    /// </summary>
	private void AssignChampionButton()
    {
        for (int i = 0; i < spawnedChampions.Length; i++)
        {
            Transform buttonParent = spawnedChampions[i].transform.Find("ChampionButton");
            SpriteRenderer championButton = buttonParent.gameObject.AddComponent<SpriteRenderer>();
            Sprite newButton = Instantiate(buttons[i], buttonParent);
            championButton.sprite = newButton;
        }
    }
}
