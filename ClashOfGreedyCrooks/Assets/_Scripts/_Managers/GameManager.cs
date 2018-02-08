using UnityEngine;

public class GameManager : GenericSingleton<GameManager>
{
	private int roundsPlayed;

	public int RoundsPlayed { get; set; }
}
