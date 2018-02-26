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

	private float timeToEnterArena = 1.5f;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		InitializeAudio();
		pickingResources = GetComponent<PickingResources>();
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

        if (!card.GetComponent<CardComponent>().CanPick())
            return;
		//Pick from pool
		if (!targetChampion.Picked)
		{
            card.GetComponent<MoveCard>().target = pickingResources.pickingPositions.Find("Picked").GetChild(playerIndex).position;
			targetChampion.Picked = true;
			targetChampion.PlayerIndex = playerIndex;
			PlayerManager.connectedPlayers[playerIndex].HasChampion = true;
            CardComponent cc = card.GetComponent<CardComponent>();
            cc.avatarColor.sprite = Resources.Load<Sprite>("UI/PlayerColors/" + PlayerManager.connectedPlayers[playerIndex].AvatarColor);
			card.GetComponent<CardComponent>().avatarSymbol.sprite = Resources.Load<Sprite>("UI/Avatars/" + PlayerManager.connectedPlayers[playerIndex].AvatarSymbol);
			a_pickChamp.Play();
		}

		//Menu for penalties
		else if (targetChampion.Picked)
		{
			if (targetChampion.GetComponent<Penalty>().CanAddPenalty)
				targetChampion.GetComponent<Penalty>().AddPenalty(card.GetComponent<CardComponent>(), pickingResources);
			else
				return;
			targetChampion.PlayerIndex = playerIndex;
            card.GetComponent<MoveCard>().target = pickingResources.pickingPositions.Find("Picked").GetChild(playerIndex).position;
            PlayerManager.connectedPlayers[playerIndex].HasChampion = true;
			PlayerManager.connectedPlayers[targetChampion.LastPlayerIndex].HasChampion = false;
            CardComponent cc = card.GetComponent<CardComponent>();
            cc.avatarColor.sprite = Resources.Load<Sprite>("UI/PlayerColors/" + PlayerManager.connectedPlayers[playerIndex].AvatarColor);
			card.GetComponent<CardComponent>().avatarSymbol.sprite = Resources.Load<Sprite>("UI/Avatars/" + PlayerManager.connectedPlayers[playerIndex].AvatarSymbol);

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
        InvokeRepeating("LockCard", .2f, .5f);        
	}

    int totalCards;
    int nextCard;
    private void LockCard()
    {
        if (totalCards == 0)
            totalCards = pickingResources.cards.Length;

        pickingResources.cards[nextCard].GetComponent<CardComponent>().locked.SetActive(true);

		AudioManager.GetInstance.PlayOneShot3D("event:/Picking/pickingCardLocked", pickingResources.cards[nextCard].transform.position);

        nextCard++;
        if (nextCard == totalCards)
        {
            CancelInvoke();
            Invoke("EndText", 1f);
        }
    }

	/// <summary>
	/// Creates a Sprite-object, with an attached AnimatorController. Uses the "countdown_allready"-file. Shows before entering Arena.
	/// </summary>
	private void EndText()
    {
        GameObject readyObj = new GameObject();
        readyObj.AddComponent<SpriteRenderer>();
        readyObj.GetComponent<SpriteRenderer>().sortingOrder = 10;
        readyObj.GetComponent<SpriteRenderer>().sprite = pickingResources.readySprite;

		RuntimeAnimatorController animController = Resources.Load("Animations/UI_Countdown_Controller") as RuntimeAnimatorController;
		readyObj.AddComponent<Animator>().runtimeAnimatorController = animController;
		readyObj.GetComponent<Animator>().SetTrigger("EndAllready");

		Vector3 pos = new Vector3(0f, 10f, 0f);
        readyObj.transform.position = pos;
        Invoke("EndOfPhase", timeToEnterArena);
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

		GameStateManager.GetInstance.SetState(GameState.LoadingScreen);
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

		//Enable scripts
		newPlayer.GetComponent<PlayerHealth>().enabled = true;

        //Player stats
        Transform healthBarAvatar = newPlayer.transform.Find("HealthBar/Slider/ColorHolder");
        healthBarAvatar.Find("PlayerColor").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/PlayerColors/" + PlayerManager.connectedPlayers[playerIndex].AvatarColor);
        healthBarAvatar.Find("PlayerIcon").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Avatars/" + PlayerManager.connectedPlayers[playerIndex].AvatarSymbol);
        healthBarAvatar.Find("PlayerIcon").GetComponent<Image>().preserveAspect = true;

        newPlayer.GetComponent<PlayerHealth>().SetStartHealth(championScript.Health * 10 + 70);
		newPlayer.GetComponentInChildren<Weapon>().damage = championScript.Damage * 0.8f + 8f;
		newPlayer.GetComponent<PlayerController>().attackSpeed = (1f / championScript.AttackSpeed) + 0.2f;
        newPlayer.GetComponent<PlayerController>().speed = (championScript.Movement * 0.35f) + 3f;

        //Bildfold
        if (champion.GetComponent<Penalty>().specialPenalties[0])
            newPlayer.GetComponentInChildren<Weapon>().blindFolded = true;

        //Drunk
        if (champion.GetComponent<Penalty>().specialPenalties[1])
            newPlayer.GetComponent<PlayerController>().speed = championScript.Movement * -1f;

        //Fat
        if (champion.GetComponent<Penalty>().specialPenalties[2])
            champion.transform.localScale = newPlayer.transform.localScale * 1.5f;
        
		newPlayer.transform.Find("PlayerNav").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UI/PlayerNav/" + playerIndex.ToString());

		newPlayer.name = "Player " + playerIndex;
		PlayerManager.spawnedPlayers[playerIndex] = newPlayer;

	}
}
