﻿using UnityEngine;

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

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        pickingResources = GetComponent<PickingResources>();

        if (GameManager.GetInstance.RoundsPlayed > 0)
            PlayerManager.NextPickingPhase();
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
                if (PlayerManager.connectedPlayers[i].ChoosingPenalty)
                {
                    ChoosePenalty(i, gamepadIndex, button);
                    PlayerManager.connectedPlayers[i].ChoosingPenalty = false;
                    return;
                }
                else if (PlayerManager.connectedPlayers[i].HasChampion)
                    return;
                else
                {
                    OnPickChampion(i, button);
                    return;
                }
    }

    private void OnPickChampion(int playerIndex, int button)
    {
        Champion targetChampion = pickingResources.spawnedChampions[button].GetComponent<Champion>();
        if (!targetChampion.Picked)
        {
            targetChampion.transform.position = pickingResources.playerPositions[playerIndex].position;
            targetChampion.Picked = true;
            targetChampion.PlayerIndex = playerIndex;
            PlayerManager.connectedPlayers[playerIndex].HasChampion = true;
        }
        else if (targetChampion.Picked && !targetChampion.Locked)
        {
            targetChampion.Locked = true;
            targetChampion.PlayerIndex = playerIndex;
            targetChampion.GetComponent<Penalty>().Buttons(true);
            PlayerManager.connectedPlayers[playerIndex].ChoosingPenalty = true;
            PlayerManager.connectedPlayers[playerIndex].HasChampion = true;
        }

        if (IsAllChampionsPicked())
            OnAllChampionsPicked();
    }

    private void ChoosePenalty(int playerIndex, int gamepadIndex, int button)
    {
        for (int i = 0; i < pickingResources.spawnedChampions.Length; i++)
        {
            Champion targetChampion = pickingResources.spawnedChampions[i].GetComponent<Champion>();
            if (targetChampion.PlayerIndex == playerIndex)
            {
                targetChampion.GetComponent<Penalty>().AddPenalty((Nerf)button, 2);
                targetChampion.GetComponent<Penalty>().Buttons(false);
                targetChampion.transform.position = pickingResources.playerPositions[playerIndex].position;
                targetChampion.Locked = false;
                PlayerManager.connectedPlayers[targetChampion.LastPlayerIndex].HasChampion = false;
                return;
            }
        }
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

        Debug.Log("Before");
        Debug.Log("Health: " + championScript.Health);
        Debug.Log("Damage: " + championScript.Damage);
        Debug.Log("Movement: " + championScript.Movement);
        Debug.Log("AttackSpeed: " + championScript.AttackSpeed);
        //Player stats
        newPlayer.GetComponent<PlayerHealth>().SetStartHealth(championScript.Health * 10);
        newPlayer.GetComponent<Shooting>().damage = championScript.Damage;
        newPlayer.GetComponent<PlayerController>().speed = championScript.Movement;
        newPlayer.GetComponent<PlayerController>().attackSpeed = championScript.AttackSpeed * .1f;
        Debug.Log(" ");
        Debug.Log("After");
        Debug.Log("Health: " + newPlayer.GetComponent<PlayerHealth>().currentHealth);
        Debug.Log("Damage: " + newPlayer.GetComponent<Shooting>().damage);
        Debug.Log("Movement: " + newPlayer.GetComponent<PlayerController>().speed);
        Debug.Log("AttackSpeed: " + newPlayer.GetComponent<PlayerController>().attackSpeed);
        Debug.Log(" ");

        newPlayer.name = "Player " + playerIndex;
        PlayerManager.spawnedPlayers[playerIndex] = newPlayer;
    }
}
