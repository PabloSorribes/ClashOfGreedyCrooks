using System.Linq;
using UnityEngine;

public class PickingResources : MonoBehaviour {

    #region LoadsFromResources
    private GameObject pickingPositionsPrefab;
    private GameObject cardPrefab, portraitSetupPrefab;
    public CardComponent[] cards;
    private GameObject[] queen;
    private GameObject[] wizard;
    private GameObject[] bride;
    private GameObject[] hoff;
    private GameObject[] championPrefabs;
    private Sprite backgroundFrame;
    private Sprite[] buttons;
    public Sprite[] avatarSymbols;
    public Sprite[] avatarColors;
    public Sprite[] penaltySprites;
    public Sprite readySprite;
    [HideInInspector] public GameObject playerPrefab;
    #endregion
    
    [HideInInspector] public Transform pickingPositions;
    private Transform[] portraitSetup;
    [HideInInspector] public GameObject[] spawnedChampions;
    private int[] arrayPos = new int[4];

    private void Start()
    {
        LoadResources();
        PickingPositions();
        PortraitSetup();
        BackgroundCardFrames();
        PlayerAvatars();
        InstantiateCards();
        SpawnChampions();
        SetCardGrapthics();
    }

    private void LoadResources()
    {
        pickingPositionsPrefab = Resources.Load("Picking/PickingPositions") as GameObject;
        portraitSetupPrefab = Resources.Load("Picking/PortraitSetup") as GameObject;
        cardPrefab = Resources.Load("Picking/Card") as GameObject;
        queen = Resources.LoadAll("Champions/Queen", typeof(Object)).Cast<GameObject>().ToArray();
        wizard = Resources.LoadAll("Champions/Wizard", typeof(Object)).Cast<GameObject>().ToArray();
        bride = Resources.LoadAll("Champions/Bride", typeof(Object)).Cast<GameObject>().ToArray();
        hoff = Resources.LoadAll("Champions/Hoff", typeof(Object)).Cast<GameObject>().ToArray();
        buttons = Resources.LoadAll("UI/XboxButtons", typeof(Sprite)).Cast<Sprite>().ToArray();
        backgroundFrame = Resources.Load<Sprite>("UI/Frames/Cardslot_frame");
        playerPrefab = Resources.Load("PlayerPrefab") as GameObject;
        avatarSymbols = Resources.LoadAll("UI/Avatars", typeof(Sprite)).Cast<Sprite>().ToArray();
        avatarColors = Resources.LoadAll("UI/PlayerColors", typeof(Sprite)).Cast<Sprite>().ToArray();
        penaltySprites = Resources.LoadAll("UI/Penalties", typeof(Sprite)).Cast<Sprite>().ToArray();
        readySprite = Resources.Load<Sprite>("UI/Picking/countdown_allready");
        LoadChampions();
    }

    private void LoadChampions()
    {
        championPrefabs = new GameObject[4];
        for (int i = 0; i < arrayPos.Length; i++)
        {
            arrayPos[i] = i;
        }
        Shuffle(arrayPos);
        championPrefabs[0] = queen[arrayPos[0]];
        championPrefabs[1] = wizard[arrayPos[1]];
        championPrefabs[2] = bride[arrayPos[2]];
        championPrefabs[3] = hoff[arrayPos[3]];
        Shuffle(championPrefabs);
    }

    /// <summary>
    /// Switching place of items in an array.
    /// </summary>
    /// <param name="array"></param>
    private void Shuffle(int[] array)
    {
        int number;
        int target;
        for (int i = 0; i < array.Length; i++)
        {
            number = Random.Range(0, array.Length);
            target = array[number];
            array[number] = array[i];
            array[i] = target;
        }
    }

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

    private void PickingPositions()
    {
        GameObject newPickingPositions = Instantiate(pickingPositionsPrefab);
        pickingPositions = newPickingPositions.transform;
    }

    private void PortraitSetup()
    {
        portraitSetup = new Transform[PlayerManager.GetPlayersConnected()];
        for (int i = 0; i < PlayerManager.GetPlayersConnected(); i++)
        {
            GameObject newPortraitSetup = Instantiate(portraitSetupPrefab);
            newPortraitSetup.transform.position = pickingPositions.Find("Portrait").GetChild(i).position;
            portraitSetup[i] = newPortraitSetup.transform;
        }        
    }

    private void BackgroundCardFrames()
    {
        for (int i = 0; i < PlayerManager.GetPlayersConnected(); i++)
        {
            Vector3 poolPos = pickingPositions.Find("Pool").GetChild(i).position;
            GameObject poolFrame = new GameObject();
            poolFrame.AddComponent<SpriteRenderer>();
            poolFrame.GetComponent<SpriteRenderer>().sortingOrder = -5;
            poolFrame.transform.position = poolPos;
            poolFrame.GetComponent<SpriteRenderer>().sprite = backgroundFrame;

            Vector3 pickedPos = pickingPositions.Find("Picked").GetChild(i).position;
            GameObject pickedFrame = new GameObject();
            pickedFrame.AddComponent<SpriteRenderer>();
            pickedFrame.GetComponent<SpriteRenderer>().sortingOrder = -5;
            pickedFrame.transform.position = pickedPos;
            pickedFrame.GetComponent<SpriteRenderer>().sprite = backgroundFrame;
        }
    }

    private void PlayerAvatars()
    {
        for (int i = 0; i < PlayerManager.GetPlayersConnected(); i++)
        {
            Vector3 pos = pickingPositions.Find("Picked").GetChild(i).position;
            GameObject avatarColor = new GameObject();
            avatarColor.AddComponent<SpriteRenderer>();
            avatarColor.GetComponent<SpriteRenderer>().sortingOrder = -2;
            avatarColor.transform.position = pos;
            avatarColor.transform.localScale = avatarColor.transform.localScale * 2.2f;
            avatarColor.GetComponent<SpriteRenderer>().sprite = avatarColors[i];

            GameObject avatarSymbol = new GameObject();
            avatarSymbol.AddComponent<SpriteRenderer>();
            avatarSymbol.GetComponent<SpriteRenderer>().sortingOrder = -1;
            avatarSymbol.transform.position = pos;
            for (int j = 0; j < avatarSymbols.Length; j++)
            {
                if (avatarSymbols[j].name == PlayerManager.players[i].AvatarSymbol)
                    avatarSymbol.GetComponent<SpriteRenderer>().sprite = avatarSymbols[j];
            }
        }
    }

    private void InstantiateCards()
    {
        cards = new CardComponent[PlayerManager.GetPlayersConnected()];
        for (int i = 0; i < PlayerManager.GetPlayersConnected(); i++)
        {
            RenderTexture rt = new RenderTexture(512, 512, 24);
            Vector3 pos = pickingPositions.Find("Pool").GetChild(i).position;
            GameObject newCard = Instantiate(cardPrefab);
            newCard.transform.position = pos;
            newCard.GetComponent<MoveCard>().target = pos;
            cards[i] = newCard.GetComponent<CardComponent>();
            cards[i].portraitRawImage.texture = rt;
            portraitSetup[i].Find("Camera").GetComponent<Camera>().targetTexture = rt;
            portraitSetup[i].Find("Camera").GetComponent<Camera>().depth = i;
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
                cards[i].champion.SetStatingStats();
            }
    }

    private void SetCardGrapthics()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].background.sprite = Resources.Load<Sprite>("UI/Cardbacks/" + spawnedChampions[i].GetComponent<Champion>().name + "_BG");
            cards[i].frame.sprite = Resources.Load<Sprite>("UI/Frames/" + spawnedChampions[i].GetComponent<Champion>().name + "_Frame");
            cards[i].healthText.text = cards[i].champion.Health.ToString();
            cards[i].movementText.text = cards[i].champion.Movement.ToString();
            cards[i].damageText.text = cards[i].champion.Damage.ToString();
            cards[i].attackSpeedText.text = cards[i].champion.AttackSpeed.ToString();
            cards[i].xboxButton.sprite = buttons[i];
        }
    }

    
}
