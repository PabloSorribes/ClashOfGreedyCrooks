using UnityEngine;

public static class PlayerManager
{
    public static PlayerInfo[] players;
    public static GameObject[] spawnedPlayers;

    public static void FillPlayersArray()
    {
        players = new PlayerInfo[4];
        for (int i = 0; i < players.Length; i++)
            players[i] = new PlayerInfo(99, 99);
    }

    public static int GetPlayersConnected()
    {
        int count = 0;
        for (int i = 0; i < players.Length; i++)
            if (players[i].Connected)
                count++;

        return count;
    }

    public static void SetSpawnedPlayersArrayLenght()
    {
        spawnedPlayers = new GameObject[GetPlayersConnected()];
    }

    public static void AddSpawnedPlayer(GameObject player)
    {
        for (int i = 0; i < spawnedPlayers.Length; i++)
            if (spawnedPlayers[i] == null)
            {
                spawnedPlayers[i] = player;
                return;
            }
    }

    public static void SendInfoToInputManager()
    {
        GameObject[] newPlayersArray = new GameObject[spawnedPlayers.Length];

        //Copy Players to newPlayersArray
        for (int i = 0; i < spawnedPlayers.Length; i++)
            newPlayersArray[i] = spawnedPlayers[i].gameObject;

        //Sort by gamepadIndex
        GameObject playerToSort;
        for (int i = 0; i < newPlayersArray.Length - 1; i++)
            for (int j = i + 1; j < newPlayersArray.Length; j++)
                if (newPlayersArray[i].GetComponent<PlayerInfo>().Gamepad > newPlayersArray[j].GetComponent<PlayerInfo>().Gamepad)
                {
                    playerToSort = newPlayersArray[j];
                    newPlayersArray[j] = newPlayersArray[i];
                    newPlayersArray[i] = playerToSort;
                }

        PlayerController[] pcArray = new PlayerController[GetPlayersConnected()];
        for (int i = 0; i < pcArray.Length; i++)
            pcArray[i] = newPlayersArray[i].GetComponent<PlayerController>();

        InputManager.GetInstance.SetPlayerReferences(pcArray);
    }
}
