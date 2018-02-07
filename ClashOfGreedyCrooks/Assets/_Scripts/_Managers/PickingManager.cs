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

    #region LoadsFromResources
    private GameObject arena, spawnedArena;
	private GameObject playerPrefab;
	private GameObject[] championPrefabs;
	private GameObject[] weaponPrefabs;
	private Sprite[] buttons;
    #endregion

	private Transform[] spawnPositions = new Transform[4];
	private Transform[] playerPositions = new Transform[4];
	private Transform[] avatars = new Transform[4];
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
		AssignChampionButton();
	}
    
	private void LoadResources()
	{
		arena = Resources.Load("Arenas/PickingTestArena") as GameObject;
		playerPrefab = Resources.Load("PlayerPrefab") as GameObject;
		championPrefabs = Resources.LoadAll("Champions", typeof(Object)).Cast<GameObject>().ToArray();
		weaponPrefabs = Resources.LoadAll("Weapons", typeof(Object)).Cast<GameObject>().ToArray();
		buttons = Resources.LoadAll("Picking/Sprites/Buttons", typeof(Sprite)).Cast<Sprite>().ToArray();
	}
    
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
				GameObject newChampion = Instantiate(championPrefabs[i].gameObject, spawnPositions[i].position, spawnPositions[i].rotation);
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

    /// <summary>
    /// Called from InputManager.
    /// </summary>
    /// <param name="gamepadIndex"></param>
    /// <param name="button"></param>
	public void PickChampion(int gamepadIndex, int button)
	{
		for (int i = 0; i < PlayerManager.players.Length; i++)
			if (PlayerManager.players[i].Gamepad == gamepadIndex)
				if (PlayerManager.players[i].ChoosingPenalty)
				{
					ChoosePenalty(i, gamepadIndex, button);
					PlayerManager.players[i].ChoosingPenalty = false;
					return;
				}
				else if (PlayerManager.players[i].HasChampion)
					return;
				else
				{
					OnPickChampion(i, button);
					return;
				}
	}

	private void OnPickChampion(int playerIndex, int button)
	{
		if (!spawnedChampions[button].GetComponent<Champion>().Picked)
		{
			spawnedChampions[button].transform.position = playerPositions[playerIndex].position;
			spawnedChampions[button].GetComponent<Champion>().Picked = true;
			spawnedChampions[button].GetComponent<Champion>().PlayerIndex = playerIndex;
			PlayerManager.players[playerIndex].HasChampion = true;
		}
		else if (spawnedChampions[button].GetComponent<Champion>().Picked && !spawnedChampions[button].GetComponent<Champion>().Locked)
		{
			spawnedChampions[button].GetComponent<Champion>().Locked = true;
			spawnedChampions[button].GetComponent<Champion>().PlayerIndex = playerIndex;
			spawnedChampions[button].GetComponent<Penalty>().Buttons(true);
			PlayerManager.players[playerIndex].ChoosingPenalty = true;
			PlayerManager.players[playerIndex].HasChampion = true;
		}

		if (IsAllChampionsPicked())
			OnAllChampionsPicked();
	}

	private void ChoosePenalty(int playerIndex, int gamepadIndex, int button)
	{
		for (int i = 0; i < spawnedChampions.Length; i++)
			if (spawnedChampions[i].GetComponent<Champion>().PlayerIndex == playerIndex)
			{
				spawnedChampions[i].GetComponent<Penalty>().AddPenalty((Nerf)button, 2);
				spawnedChampions[i].GetComponent<Champion>().Locked = false;
				spawnedChampions[i].transform.position = playerPositions[playerIndex].position;
				spawnedChampions[i].GetComponent<Penalty>().Buttons(false);
				PlayerManager.players[spawnedChampions[i].GetComponent<Champion>().LastPlayerIndex].HasChampion = false;
				return;
			}
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

    /// <summary>
    /// Instantiates a player prefab and attach a champion to it.
    /// </summary>
    /// <param name="playerIndex"></param>
    /// <param name="championIndex"></param>
	private void SpawnPlayer(int playerIndex, int championIndex)
	{
		GameObject newPlayer = Instantiate(playerPrefab);

        //References
        PlayerInfo playerInfo = newPlayer.GetComponent<PlayerInfo>();
        Transform champion = spawnedChampions[championIndex].transform;
        Champion championScript = spawnedChampions[championIndex].GetComponent<Champion>();

        //Player
        playerInfo.Player = PlayerManager.players[playerIndex].Player;
        playerInfo.Gamepad = PlayerManager.players[playerIndex].Gamepad;
        playerInfo.SetDontDestroyOnLoad();

        //Champion
        champion.SetParent(newPlayer.transform.Find("Champion"));
        champion.localPosition = Vector3.zero;
        champion.rotation = newPlayer.transform.rotation;
        champion.Find("StatsHolder").gameObject.SetActive(false);
        champion.Find("ChampionButton").gameObject.SetActive(false);

        //Enable scripts
		newPlayer.GetComponent<Shooting>().enabled = true;
		newPlayer.GetComponent<PlayerHealth>().enabled = true;

        //Player stats
		newPlayer.GetComponent<PlayerHealth>().SetStartHealth(championScript.Health * 10);
		newPlayer.GetComponent<Shooting>().damage = championScript.Damage;
		newPlayer.GetComponent<PlayerController>().speed = championScript.Movement;
		newPlayer.GetComponent<PlayerController>().attackSpeed = championScript.AttackSpeed * .1f;

        PlayerManager.AddSpawnedPlayer(newPlayer);
	}
}
