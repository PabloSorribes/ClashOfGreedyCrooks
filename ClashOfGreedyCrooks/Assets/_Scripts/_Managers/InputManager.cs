using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class InputManager : GenericSingleton<InputManager>
{

	private GameState gameState;

	private PlayerController[] players = new PlayerController[4];

	private List<PlayerIndex> gamepadIndex = new List<PlayerIndex>();
	private GamePadState[] state = new GamePadState[4];
	private GamePadState[] prevState = new GamePadState[4];


	private void Start()
	{
		OnGameStateChanged(GameStateManager.GetInstance.GetState());
		GameStateManager.GetInstance.GameStateChanged += OnGameStateChanged;

		AddConnectedGamepads();
	}

	private void OnGameStateChanged(GameState newGameState)
	{
		gameState = newGameState;
		//Debug.Log("(IM) State Changed: " + gameState);
	}

	/// <summary>
	/// Checks first four gamepad slots and assigns each connected gamepad successively.
	/// </summary>
	//TODO: Expand to detect disconnected/newly connected gamepads and adapt array accordingly.
	private void AddConnectedGamepads()
	{
		for (int i = 0; i < 3; ++i)
		{
			//Note: "PlayerIndex" enum type corresponds to "gamepadIndex" variable in project since it can't be renamed (XInput).
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
		for (int i = 0; i < gamepadIndex.Count; ++i)
		{
			//SetVibration should be sent in a slower rate.
			//Set vibration according to triggers. FOR FUTURE REFERENCE.
			GamePad.SetVibration((PlayerIndex)i, state[i].Triggers.Left, state[i].Triggers.Right);
		}
	}

	void Update()
	{
		//Loop through inputs for every connected gamepad
		for (int i = 0; i < gamepadIndex.Count; ++i)
		{
			prevState[i] = state[i];
			state[i] = GamePad.GetState((PlayerIndex)i);

			//TODO: Modify to handle inputs with events instead.
			switch (gameState.GetHashCode())
			{
				case 0: //Main Menu

					//TODO: Poll through relevant inputs on all controllers and call relevant methods

					break;

				case 1: //Player Connect Phase

					// Detect if a button was pressed this frame
					if (prevState[i].Buttons.A == ButtonState.Released &&
							state[i].Buttons.A == ButtonState.Pressed)
					{
						PlayerConnectManager.GetInstance.AddPlayer((int)gamepadIndex[i]);
						Debug.Log("Player " + i + " pressed button A on Gamepad " + i);
					}

					if (prevState[i].Buttons.B == ButtonState.Released &&
							state[i].Buttons.B == ButtonState.Pressed)
					{
						PlayerConnectManager.GetInstance.RemovePlayer((int)gamepadIndex[i]);

						Debug.Log("Player " + i + " pressed button B on Gamepad " + i);
					}
					if (prevState[i].Buttons.Start == ButtonState.Released &&
							state[i].Buttons.Start == ButtonState.Pressed)
					{
						//TODO: Call function to start game when all players are ready
					}

					break;

				case 2: //Picking Phase

					if (prevState[i].Buttons.A == ButtonState.Released &&
							state[i].Buttons.A == ButtonState.Pressed)
					{
						PickingManager.GetInstance.PickChampion(i, 0);
						Debug.Log("Player " + i + " pressed button A on Gamepad " + i);
					}

					if (prevState[i].Buttons.B == ButtonState.Released &&
							state[i].Buttons.B == ButtonState.Pressed)
					{
						PickingManager.GetInstance.PickChampion(i, 1);

						Debug.Log("Player " + i + " pressed button B on Gamepad " + i);
					}

					if (prevState[i].Buttons.X == ButtonState.Released &&
							state[i].Buttons.X == ButtonState.Pressed)
					{
						PickingManager.GetInstance.PickChampion(i, 2);

						Debug.Log("Player " + i + " pressed button X on Gamepad " + i);
					}

					if (prevState[i].Buttons.Y == ButtonState.Released &&
							state[i].Buttons.Y == ButtonState.Pressed)
					{
						PickingManager.GetInstance.PickChampion(i, 3);

						Debug.Log("Player " + i + " pressed button Y on Gamepad " + i);
					}


					break;

				case 3: //Arena Phase

					Vector3 leftStick = new Vector3(state[i].ThumbSticks.Left.X, 0f, state[i].ThumbSticks.Left.Y);
					Vector3 rightStick = new Vector3(state[i].ThumbSticks.Right.X, 0f, state[i].ThumbSticks.Right.Y);
					//Send directional input data to each player
					players[i].SetDirectionalInput(leftStick, rightStick);


					if (state[i].Triggers.Left > 0)
					{
						//TODO: Alt. Fire or Skill
					}

					if (state[i].Triggers.Right > 0)
					{
						players[i].Shoot();
					}

					if (prevState[i].Buttons.LeftShoulder == ButtonState.Released &&
							state[i].Buttons.LeftShoulder == ButtonState.Pressed)
					{
						//TODO: Alt. Fire or Skill
					}

					if (prevState[i].Buttons.RightShoulder == ButtonState.Released &&
							state[i].Buttons.RightShoulder == ButtonState.Pressed)
					{
						players[i].Shoot();
					}

					break;
			}
		}
	}

	public void SetPlayerReferences(PlayerController[] players)
	{
		players.CopyTo(this.players, 0);
	}
}
