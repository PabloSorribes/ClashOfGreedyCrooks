using UnityEngine;

public class GameManager : GenericSingleton<GameManager>
{
	private int roundsPlayed = 0;

	public int RoundsPlayed { get; set; }
}
