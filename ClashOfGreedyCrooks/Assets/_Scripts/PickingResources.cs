using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PickingResources : MonoBehaviour {

    #region LoadsFromResources
    private GameObject pickingPositionsPrefab;
    private GameObject cardPrefab, portraitSetupPrefab;
    public CardComponent[] cards;
    private GameObject[] championPrefabs;
    private GameObject[] weaponPrefabs;
    private Sprite[] buttons;
    [HideInInspector] public GameObject playerPrefab;
    #endregion
    
    [HideInInspector] public Transform[] playerPositions = new Transform[4];
    [HideInInspector] public Transform pickingPositions;
    private Transform[] portraitSetup;
    [HideInInspector] public GameObject[] spawnedChampions;

    private void Start()
    {
        LoadResources();
        GameObject newPickingPositions = Instantiate(pickingPositionsPrefab);
        pickingPositions = newPickingPositions.transform;
        PortraitSetup();
        InstantiateCards();
        Shuffle(championPrefabs);
        Shuffle(weaponPrefabs);
        SpawnChampions();
        SpawnWeapons();
        AssignChampionButton();
    }

    private void LoadResources()
    {
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
        portraitSetup = new Transform[PlayerManager.GetPlayersConnected()];
        for (int i = 0; i < PlayerManager.GetPlayersConnected(); i++)
        {
            GameObject newPortraitSetup = Instantiate(portraitSetupPrefab);
            newPortraitSetup.transform.position = pickingPositions.Find("Portrait").position;
            portraitSetup[i] = newPortraitSetup.transform;
        }
        
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
            portraitSetup[i].Find("Camera").GetComponent<Camera>().targetTexture = rt;
            portraitSetup[i].Find("Camera").GetComponent<Camera>().depth = i;
        }
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
                Vector3 pos = portraitSetup[i].Find("Champion").position;
                Quaternion rot = portraitSetup[i].Find("Champion").rotation;
                GameObject newChampion = Instantiate(championPrefabs[i].gameObject, pos, rot);
                spawnedChampions[i] = newChampion;
                spawnedChampions[i].GetComponent<Champion>().PlayerIndex = 99;
                cards[i].champion = newChampion.GetComponent<Champion>();
                Sprite cardBack = Resources.Load<Sprite>("UI/Cardbacks/" + spawnedChampions[i].GetComponent<Champion>().name);
                cards[i].GetComponent<SpriteRenderer>().sprite = cardBack;                    
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
