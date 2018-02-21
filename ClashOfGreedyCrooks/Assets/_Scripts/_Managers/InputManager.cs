using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using XInputDotNetPure;

public class InputManager : GenericSingleton<InputManager>
{
	private GameState gameState;

	private PlayerController[] players = new PlayerController[4];

	//Gamepad variables
	private List<PlayerIndex> gamepadIndex = new List<PlayerIndex>();
	private GamePadState[] state = new GamePadState[4];
	private GamePadState[] prevState = new GamePadState[4];

	public bool freezeInput;

	private void Start()
	{
		OnGameStateChanged(GameStateManager.GetInstance.GetState());
		GameStateManager.GetInstance.GameStateChanged += OnGameStateChanged;

		AddConnectedGamepads();
	}

	private void OnGameStateChanged(GameState newGameState)
	{
		gameState = newGameState;
	}

	/// <summary>
	/// Checks first four gamepad slots and assigns each connected gamepad successively.
	/// </summary>
	//TODO: Expand to detect disconnected/newly connected gamepads and adapt array accordingly.
	private void AddConnectedGamepads()
	{
		for (int i = 0; i <= 3; ++i)
		{
			//Note: "PlayerIndex" enum type corresponds to "gamepadIndex" variable in this project since it can't be renamed (XInput).
			//Only applicable in this context
			PlayerIndex testGamepadIndex = (PlayerIndex)i;
			GamePadState testState = GamePad.GetState(testGamepadIndex);
			if (testState.IsConnected)
			{
				Debug.Log(string.Format("GamePad found {0}", testGamepadIndex));
				gamepadIndex.Add(testGamepadIndex);
			}
		}
	}

	void FixedUpdate()
	{
		//TODO: Make into own function to be able to trigger Vibration on demand.
		for (int i = 0; i < gamepadIndex.Count; ++i)
		{
			//SetVibration should be sent in a slower rate.
			//Set vibration according to triggers. FOR FUTURE REFERENCE.
			GamePad.SetVibration((PlayerIndex)i, state[i].Triggers.Left, state[i].Triggers.Right);
		}
	}

	void Update()
	{
		DebugKeys();

		if (freezeInput)
			return;

		//Loop through inputs for every connected gamepad
		for (int i = 0; i < gamepadIndex.Count; ++i)
		{
			prevState[i] = state[i];
			state[i] = GamePad.GetState((PlayerIndex)i, GamePadDeadZone.None);

			//TODO: Modify to handle inputs with events instead of calling specific functions in other scripts.
			switch (gameState)
			{
				case GameState.MainMenu:
					break;

				case GameState.PlayerConnect:

					if (prevState[i].Buttons.A == ButtonState.Released && state[i].Buttons.A == ButtonState.Pressed)
						PlayerConnectManager.GetInstance.AddPlayer((int)gamepadIndex[i]);

					if (prevState[i].Buttons.B == ButtonState.Released && state[i].Buttons.B == ButtonState.Pressed)
						PlayerConnectManager.GetInstance.RemovePlayer((int)gamepadIndex[i]);

					if (prevState[i].Buttons.Start == ButtonState.Released && state[i].Buttons.Start == ButtonState.Pressed)
						PlayerConnectManager.GetInstance.AddPlayer((int)gamepadIndex[i]);

					break;

				case GameState.Picking:

					if (prevState[i].Buttons.A == ButtonState.Released && state[i].Buttons.A == ButtonState.Pressed)
						PickingManager.GetInstance.PickChampion(i, 0);

					if (prevState[i].Buttons.B == ButtonState.Released && state[i].Buttons.B == ButtonState.Pressed)
						PickingManager.GetInstance.PickChampion(i, 1);

					if (prevState[i].Buttons.X == ButtonState.Released && state[i].Buttons.X == ButtonState.Pressed)
						PickingManager.GetInstance.PickChampion(i, 2);

					if (prevState[i].Buttons.Y == ButtonState.Released && state[i].Buttons.Y == ButtonState.Pressed)
						PickingManager.GetInstance.PickChampion(i, 3);

					if (prevState[i].Buttons.Start == ButtonState.Released && state[i].Buttons.Start == ButtonState.Pressed)
					{
                        GameStateManager.GetInstance.PauseGame();
					}

					break;

				case GameState.Arena:

					float deadzoneRightStick = 0.4f;
					float deadzoneLeftStick = 0.2f;

					Vector3 leftStick = new Vector3(state[i].ThumbSticks.Left.X, 0f, state[i].ThumbSticks.Left.Y);
					Vector3 rightStick = new Vector3(state[i].ThumbSticks.Right.X, 0f, state[i].ThumbSticks.Right.Y);

					//Make sure leftstick diagonal input is never more than 1
					if (leftStick.magnitude > 1f)
						leftStick /= leftStick.magnitude;

					//Leftstick scaled radial deadzone implementation
					if (leftStick.magnitude < deadzoneLeftStick)
						leftStick = Vector3.zero;
					else
						leftStick = leftStick.normalized * ((leftStick.magnitude - deadzoneLeftStick) / (1 - deadzoneLeftStick));

					//Rightstick scaled radial deadzone implementation
					if (rightStick.magnitude < deadzoneRightStick)
						rightStick = Vector3.zero;
					else
						rightStick = rightStick.normalized * ((rightStick.magnitude - deadzoneRightStick) / (1 - deadzoneRightStick));

					//Send directional input data to each player
					players[i].SetDirectionalInput(leftStick, rightStick);


					if (prevState[i].Buttons.A == ButtonState.Released && state[i].Buttons.A == ButtonState.Pressed)
						if (players[i] != null)
							players[i].Shoot();

					if (state[i].Triggers.Right >= 0.1f)
						if (players[i] != null)
							players[i].Shoot();

					if (prevState[i].Buttons.RightShoulder == ButtonState.Released && state[i].Buttons.RightShoulder == ButtonState.Pressed)
						if (players[i] != null)
							players[i].Shoot();


					if (prevState[i].Buttons.Start == ButtonState.Released && state[i].Buttons.Start == ButtonState.Pressed)
					{
						if (ArenaManager.GetInstance.roundHasEnded)
							ArenaManager.GetInstance.NextRound();
						else
                            GameStateManager.GetInstance.PauseGame();
                    }

					break;

				default:
					break;
			}
		}
	}

	public void SetPlayerReferences(PlayerController[] players)
	{
		players.CopyTo(this.players, 0);
	}

	/// <summary>
	/// Keys to load scenes and other debug stuff.
	/// </summary>
	private void DebugKeys()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
			GameStateManager.GetInstance.SetState(GameState.MainMenu);

		if (Input.GetKeyDown(KeyCode.Alpha2))
			GameStateManager.GetInstance.SetState(GameState.PlayerConnect);

		if (Input.GetKeyDown(KeyCode.Alpha3))
			GameStateManager.GetInstance.SetState(GameState.Picking);

		if (Input.GetKeyDown(KeyCode.Alpha4))
			GameStateManager.GetInstance.SetState(GameState.Arena);
	}
}
