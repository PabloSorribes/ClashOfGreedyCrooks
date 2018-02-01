using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class InputManager : MonoBehaviour
{
	public static InputManager instance;

	private State gameState;

	private PlayerIndex[] gamepadIndex = new PlayerIndex[4];
	private GamePadState[] state = new GamePadState[4];
	private GamePadState[] prevState = new GamePadState[4];

	private PlayerController[] players = new PlayerController[4];

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		gameState = GameStateManager.GetInstance().GetState();

		AddConnectedGamepads();
	}

	/// <summary>
	/// Checks first four gamepad slots and assigns each connected gamepad successively.
	/// </summary>
	private void AddConnectedGamepads()
	{
		//TODO: Change array to be variable in size in relation to connected gamepads
		//TODO: Expand ConnectedGamepads method to detect disconnected/newly connected gamepads and adapt array
		for (int i = 0; i < 3; ++i)
		{
			// PlayerIndex type corresponds to gamepadIndex in project (Can't rename XInput types)
			//Only applicable in this context
			PlayerIndex testGamepadIndex = (PlayerIndex)i;
			GamePadState testState = GamePad.GetState(testGamepadIndex);
			if (testState.IsConnected)
			{
				Debug.Log(string.Format("GamePad found {0}", testGamepadIndex));
				gamepadIndex[i] = testGamepadIndex;
			}
		}
	}

	public void GetPlayers(PlayerController[] players)
	{
		players.CopyTo(this.players, 0);
	}

	void Update()
	{
		//Loop through inputs for every connected gamepad
		for (int i = 0; i < gamepadIndex.Length; ++i)
		{
			prevState[i] = state[i];
			state[i] = GamePad.GetState((PlayerIndex)i);


			switch ((int)gameState)
			{
				case 0: //Main Menu
						//TODO: Poll through inputs on all controllers and call relevant method

					break;

				case 1: //Player Connect

					// Detect if a button was pressed this frame
					if (prevState[i].Buttons.A == ButtonState.Released &&
						state[i].Buttons.A == ButtonState.Pressed)
					{
						PlayerConnectManager.GetInstance().AddPlayer((int)gamepadIndex[i]);
						Debug.Log("Player " + i + " pressed button A on Gamepad " + i);
					}

					if (prevState[i].Buttons.B == ButtonState.Released &&
						state[i].Buttons.B == ButtonState.Pressed)
					{
						PlayerConnectManager.GetInstance().RemovePlayer((int)gamepadIndex[i]);

						Debug.Log("Player " + i + " pressed button B on Gamepad " + i);
					}

					break;

				case 2: //Picking
						//TODO: Poll through inputs on all controllers and call relevant method


					break;

				case 3: //Arena
						//TODO: Poll through inputs on all controllers and call relevant method
					if (prevState[i].Buttons.A == ButtonState.Released &&
					state[i].Buttons.A == ButtonState.Pressed)
					{
						//players[i].Shoot();
						Debug.Log("Player " + i + " pressed button A on Gamepad " + i);
					}

					break;
			}


			if (prevState[i].Buttons.A == ButtonState.Released &&
					state[i].Buttons.A == ButtonState.Pressed)
			{
				Debug.Log("Player " + i + " pressed button A on Gamepad " + i);
			}

			Vector3 leftStick = new Vector3(state[i].ThumbSticks.Left.X, 0f, state[i].ThumbSticks.Left.Y);
			Vector3 rightStick = new Vector3(state[i].ThumbSticks.Right.X, 0f, state[i].ThumbSticks.Right.Y);

			//players[i].SetDirectionalInput(leftStick, rightStick);

		}
	}


	void FixedUpdate()
	{
		for (int i = 0; i < gamepadIndex.Length; ++i)
		{
			//SetVibration should be sent in a slower rate.
			//Set vibration according to triggers
			GamePad.SetVibration((PlayerIndex)i, state[i].Triggers.Left, state[i].Triggers.Right);
		}
	}

	void OnGUI()
	{
		for (int i = 0; i < gamepadIndex.Length; ++i)
		{
			string text = string.Format("Player {0} \n", i);
		text += string.Format("IsConnected {0} Packet #{1}\n", state[i].IsConnected, state[i].PacketNumber);
		text += string.Format("\tTriggers {0} {1}\n", state[i].Triggers.Left, state[i].Triggers.Right);
		text += string.Format("\tD-Pad {0} {1} {2} {3}\n", state[i].DPad.Up, state[i].DPad.Right, state[i].DPad.Down, state[i].DPad.Left);
		text += string.Format("\tButtons Start {0} Back {1} Guide {2}\n", state[i].Buttons.Start, state[i].Buttons.Back, state[i].Buttons.Guide);
		text += string.Format("\tButtons LeftStick {0} RightStick {1} LeftShoulder {2} RightShoulder {3}\n", state[i].Buttons.LeftStick, state[i].Buttons.RightStick, state[i].Buttons.LeftShoulder, state[i].Buttons.RightShoulder);
		text += string.Format("\tButtons A {0} B {1} X {2} Y {3}\n", state[i].Buttons.A, state[i].Buttons.B, state[i].Buttons.X, state[i].Buttons.Y);
		text += string.Format("\tSticks Left {0} {1} Right {2} {3}\n", state[i].ThumbSticks.Left.X, state[i].ThumbSticks.Left.Y, state[i].ThumbSticks.Right.X, state[i].ThumbSticks.Right.Y);
		GUI.Label(new Rect(0 + i * 160, 0 + i * 160, Screen.width, Screen.height), text);
		}

	}
}
