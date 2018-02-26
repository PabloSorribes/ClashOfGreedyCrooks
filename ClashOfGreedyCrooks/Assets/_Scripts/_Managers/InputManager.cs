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

	Vector3 leftStick;
	Vector3 rightStick;

	private bool leftStickHold = false;
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
	private void AddConnectedGamepads()
	{
		//TODO: Expand to detect disconnected/newly connected gamepads and adapt accordingly.
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
		if (freezeInput)
			return;

		//Loop through each connected gamepad and get inputs
		for (int i = 0; i < gamepadIndex.Count; ++i)
		{
			prevState[i] = state[i];
			state[i] = GamePad.GetState((PlayerIndex)i, GamePadDeadZone.None);

			CalculateDirectionalInput(i);
			PollButtonInputs(i);
		}
	}

	/// <summary>
	/// Gets values from gamepad analog sticks and calculates deadzones
	/// </summary>
	/// <param name="gamepad"></param>
	private void CalculateDirectionalInput(int gamepad)
	{
		float deadzoneRightStick = 0.4f;
		float deadzoneLeftStick = 0.2f;

		leftStick = new Vector3(state[gamepad].ThumbSticks.Left.X, 0f, state[gamepad].ThumbSticks.Left.Y);
		rightStick = new Vector3(state[gamepad].ThumbSticks.Right.X, 0f, state[gamepad].ThumbSticks.Right.Y);

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
	}

	private void PollButtonInputs(int gamepad)
	{
		//TODO: Modify to handle inputs with events instead of calling specific functions in other scripts.
		switch (gameState)
		{
			default:
				break;

			case GameState.PlayerConnect:

				if (prevState[gamepad].Buttons.A == ButtonState.Released && state[gamepad].Buttons.A == ButtonState.Pressed)
					PlayerConnectManager.GetInstance.AddPlayer((int)gamepadIndex[gamepad]);

				if (prevState[gamepad].Buttons.B == ButtonState.Released && state[gamepad].Buttons.B == ButtonState.Pressed)
					PlayerConnectManager.GetInstance.RemovePlayer((int)gamepadIndex[gamepad]);

				if (prevState[gamepad].Buttons.Start == ButtonState.Released && state[gamepad].Buttons.Start == ButtonState.Pressed)
                    PlayerConnectManager.GetInstance.GoToPickingPhase();


                if (leftStick.x != 0 && !leftStickHold)
				{
					PlayerConnectManager.GetInstance.ChangeSymbol(leftStick.x, (int)gamepadIndex[gamepad]);

					leftStickHold = true;
				}
				else
				{
					leftStickHold = false;
				}

				break;

			case GameState.Picking:

				if (prevState[gamepad].Buttons.A == ButtonState.Released && state[gamepad].Buttons.A == ButtonState.Pressed)
					PickingManager.GetInstance.PickChampion(gamepad, 0);

				if (prevState[gamepad].Buttons.B == ButtonState.Released && state[gamepad].Buttons.B == ButtonState.Pressed)
					PickingManager.GetInstance.PickChampion(gamepad, 1);

				if (prevState[gamepad].Buttons.X == ButtonState.Released && state[gamepad].Buttons.X == ButtonState.Pressed)
					PickingManager.GetInstance.PickChampion(gamepad, 2);

				if (prevState[gamepad].Buttons.Y == ButtonState.Released && state[gamepad].Buttons.Y == ButtonState.Pressed)
					PickingManager.GetInstance.PickChampion(gamepad, 3);


				if (prevState[gamepad].Buttons.Start == ButtonState.Released && state[gamepad].Buttons.Start == ButtonState.Pressed)
					GameStateManager.GetInstance.PauseToggle();

				break;

			case GameState.Arena:

				//Send directional input data to each player
				players[gamepad].SetDirectionalInput(leftStick, rightStick);

				if (state[gamepad].Triggers.Right >= 0.1f)
					if (players[gamepad] != null)
						players[gamepad].Shoot();

				if (prevState[gamepad].Buttons.Start == ButtonState.Released && state[gamepad].Buttons.Start == ButtonState.Pressed)
				{
					if (ArenaManager.GetInstance.roundHasEnded)
						ArenaManager.GetInstance.NextRound();
					else
						GameStateManager.GetInstance.PauseToggle();
				}

				break;

		}
	}

	public void SetPlayerReferences(PlayerController[] players)
	{
		players.CopyTo(this.players, 0);
	}
}
