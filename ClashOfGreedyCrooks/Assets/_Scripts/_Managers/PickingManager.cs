using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

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

	private PickingResources pickingResources;

	StudioEventEmitter a_pickChamp;
	StudioEventEmitter a_pickPenalty;
	StudioEventEmitter a_pickingToArena;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		InitializeAudio();
		pickingResources = GetComponent<PickingResources>();

		if (GameStateManager.GetInstance.RoundsPlayed > 0)
			PlayerManager.NextPickingPhase();
	}

	private void InitializeAudio()
	{
		a_pickChamp = gameObject.AddComponent<FMODUnity.StudioEventEmitter>();
		a_pickChamp.Event = "event:/Picking/pickChamp";

		a_pickPenalty = gameObject.AddComponent<FMODUnity.StudioEventEmitter>();
		a_pickPenalty.Event = "event:/Picking/pickPenalty";

		a_pickingToArena = gameObject.AddComponent<FMODUnity.StudioEventEmitter>();
		a_pickingToArena.Event = "event:/Picking/pickingToArena";
	}

	/// <summary>
	/// Called from InputManager.
	/// </summary>
	/// <param name="gamepadIndex"></param>
	/// <param name="button"></param>
	public void PickChampion(int gamepadIndex, int button)
	{
		for (int i = 0; i < PlayerManager.connectedPlayers.Length; i++)
			if (PlayerManager.connectedPlayers[i].Gamepad == gamepadIndex)
				if (PlayerManager.connectedPlayers[i].HasChampion)
					return;
				else
				{
					OnPickChampion(i, button);
					return;
				}
	}

	private void OnPickChampion(int playerIndex, int button)
	{
		Transform card = pickingResources.cards[button].transform;
		Champion targetChampion = card.GetComponent<CardComponent>().champion;

		//Pick from pool
		if (!targetChampion.Picked)
		{
            card.GetComponent<MoveCard>().target = pickingResources.pickingPositions.Find("Picked").GetChild(playerIndex).position;
			targetChampion.Picked = true;
			targetChampion.PlayerIndex = playerIndex;
			PlayerManager.connectedPlayers[playerIndex].HasChampion = true;
            CardComponent cc = card.GetComponent<CardComponent>();
            cc.avatarColor.sprite = pickingResources.avatarColors[playerIndex];
            //cc.avatarColor.sprite.rect = cc.avatarColor.sprite.rect.size * 2f;
            for (int i = 0; i < pickingResources.avatarSymbols.Length; i++)
			{
				if (pickingResources.avatarSymbols[i].name == PlayerManager.connectedPlayers[playerIndex].AvatarSymbol)
					card.GetComponent<CardComponent>().avatarSymbol.sprite = pickingResources.avatarSymbols[i];
			}

			a_pickChamp.Play();
		}

		//Menu for penalties
		else if (targetChampion.Picked /*&& !targetChampion.Locked*/)
		{
			if (targetChampion.GetComponent<Penalty>().CanAddPenalty)
				targetChampion.GetComponent<Penalty>().AddPenalty(card.GetComponent<CardComponent>(), pickingResources);
			else
				return;
			targetChampion.PlayerIndex = playerIndex;
            card.GetComponent<MoveCard>().target = pickingResources.pickingPositions.Find("Picked").GetChild(playerIndex).position;
            PlayerManager.connectedPlayers[playerIndex].HasChampion = true;
			PlayerManager.connectedPlayers[targetChampion.LastPlayerIndex].HasChampion = false;
			for (int j = 0; j < pickingResources.avatarSymbols.Length; j++)
			{
				if (pickingResources.avatarSymbols[j].name == PlayerManager.connectedPlayers[playerIndex].AvatarSymbol)
					card.GetComponent<CardComponent>().avatarSymbol.sprite = pickingResources.avatarSymbols[j];
			}

			a_pickPenalty.Play();
			a_pickChamp.Play();
		}

		if (IsAllChampionsPicked())
			OnAllChampionsPicked();
	}

	private bool IsAllChampionsPicked()
	{
		int picked = 0;
		for (int i = 0; i < pickingResources.spawnedChampions.Length; i++)
			if (pickingResources.spawnedChampions[i].GetComponent<Champion>().Picked)
				picked++;
		if (picked == pickingResources.spawnedChampions.Length)
			return true;
		else
			return false;
	}

	private void OnAllChampionsPicked()
	{
        GameObject readyObj = new GameObject();
        readyObj.AddComponent<SpriteRenderer>();
        readyObj.GetComponent<SpriteRenderer>().sortingOrder = 10;
        readyObj.GetComponent<SpriteRenderer>().sprite = pickingResources.readySprite;
        Vector3 pos = new Vector3(0f, 12f, 0f);
        readyObj.transform.position = pos;
        readyObj.transform.localScale = readyObj.transform.localScale * .6f;
        Invoke("EndOfPhase", 5f);
	}

    private void EndOfPhase()
    {
		if (PlayerManager.spawnedPlayers == null)
			PlayerManager.SetSpawnedPlayersArrayLenght();

		for (int i = 0; i < PlayerManager.connectedPlayers.Length; i++)
		{
			for (int j = 0; j < pickingResources.spawnedChampions.Length; j++)
				if (pickingResources.spawnedChampions[j].GetComponent<Champion>().PlayerIndex == i)
				{
					SpawnPlayer(i, j);
				}
		}

		a_pickingToArena.Play();
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
		GameObject newPlayer = Instantiate(pickingResources.playerPrefab);

		//References
		PlayerInfo playerInfo = newPlayer.GetComponent<PlayerInfo>();
		Transform champion = pickingResources.spawnedChampions[championIndex].transform;
		Champion championScript = pickingResources.spawnedChampions[championIndex].GetComponent<Champion>();

		//Player
		playerInfo.Player = PlayerManager.connectedPlayers[playerIndex].Player;
		playerInfo.Gamepad = PlayerManager.connectedPlayers[playerIndex].Gamepad;

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
        Transform healthBarAvatar = newPlayer.transform.Find("HealthBar/Slider/ColorHolder");
        healthBarAvatar.Find("PlayerColor").GetComponent<Image>().sprite = pickingResources.avatarColors[playerIndex];
        for (int j = 0; j < pickingResources.avatarSymbols.Length; j++)
        {
            if (pickingResources.avatarSymbols[j].name == PlayerManager.connectedPlayers[playerIndex].AvatarSymbol)
                healthBarAvatar.Find("PlayerIcon").GetComponent<Image>().sprite = pickingResources.avatarSymbols[playerIndex];
        }       
        newPlayer.GetComponent<PlayerHealth>().SetStartHealth(championScript.Health * 10);
		newPlayer.GetComponent<Shooting>().damage = championScript.Damage + 5f;
		newPlayer.GetComponent<PlayerController>().speed = championScript.Movement * 0.5f + 3f;
		newPlayer.GetComponent<PlayerController>().attackSpeed = 1f / championScript.AttackSpeed;

		newPlayer.name = "Player " + playerIndex;
		PlayerManager.spawnedPlayers[playerIndex] = newPlayer;
	}
}
