﻿using UnityEngine;
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

	#region Audio Strings
	private string pickChampSound = "event:/Picking/pickChamp";
	private string pickPenaltySound = "event:/Picking/pickPenalty";
	private string cardLockedSound = "event:/Picking/pickingCardLocked";
	private string allPlayersReadySound = "event:/Picking/pickingAllPlayersReady";
	private string pickingToArenaSound = "event:/Picking/pickingToArena";
	#endregion Audio Strings

	private float timeToEnterArena = 1.5f;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		pickingResources = GetComponent<PickingResources>();
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
				{
					return;
				}
				else
				{
                    InputManager.GetInstance.Rumble(gamepadIndex, .5f, 1f, .1f);
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

			AudioManager.GetInstance.PlayOneShotAttached(pickChampSound, pickingResources.cards[nextCard].gameObject);
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

			AudioManager.GetInstance.PlayOneShotAttached(pickPenaltySound, pickingResources.cards[nextCard].gameObject);
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

		AudioManager.GetInstance.PlayOneShot3D(cardLockedSound, pickingResources.cards[nextCard].transform.position);

        InputManager.GetInstance.Rumble(PlayerManager.players[nextCard].Gamepad, .5f, 1f, .1f);

        nextCard++;
        if (nextCard == totalCards)
        {
            CancelInvoke();
            Invoke("EndText", 1f);
        }
    }

	/// <summary>
	/// Creates a Sprite-object, with an attached AnimatorController and plays a timed sound. Uses the "countdown_allready"-file. Shows before entering Arena.
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

		AudioManager.GetInstance.PlayOneShot(allPlayersReadySound);

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

		AudioManager.GetInstance.PlayOneShot(pickingToArenaSound);
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

        //Blindfold
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
