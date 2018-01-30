using UnityEngine;

public class GameManager : MonoBehaviour {

    private static GameManager instance;
    public static GameManager GetInstance()
    {
        return instance;
    }

    private PlayerController[] players = new PlayerController[4];

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        if (instance == null)
        {
            instance = this;
        }
        else if (FindObjectOfType<GameStateManager>().gameObject != this.gameObject)
        {
            Destroy(FindObjectOfType<GameStateManager>().gameObject);
        }
    }


    public void AddPlayer()
    {

    }

    public void RemovePlayer()
    {

    }

    public int GetPlayersCount()
    {
        int count = 0;
        foreach (PlayerController pc in players)
        {
            if (pc != null)
                count++;
        }
        return count;
    }

    public void ResetGame()
    {
        
    }
}
