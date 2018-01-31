using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class InputManager : MonoBehaviour
{
	private TestPlayer player;


	bool playerIndexSet = false;
	PlayerIndex playerIndex;
	GamePadState state;
	GamePadState prevState;


	void Start()
	{
		player = GetComponent<TestPlayer>();
	}

	void FixedUpdate()
	{
		// SetVibration should be sent in a slower rate.
		// Set vibration according to triggers
		GamePad.SetVibration(playerIndex, state.Triggers.Left, state.Triggers.Right);
	}

	void Update()
	{
		// Find a PlayerIndex, for a single player game
		// Will find the first controller that is connected ans use it
		if (!playerIndexSet || !prevState.IsConnected)
		{
			for (int i = 0; i < 4; ++i)
			{
				PlayerIndex testPlayerIndex = (PlayerIndex)i;
				GamePadState testState = GamePad.GetState(testPlayerIndex);
				if (testState.IsConnected)
				{
					Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
					playerIndex = testPlayerIndex;
					playerIndexSet = true;
				}
			}
		}

		prevState = state;
		state = GamePad.GetState(playerIndex);

		Vector3 leftStick = new Vector3(state.ThumbSticks.Left.X, 0f, state.ThumbSticks.Left.Y);
		Vector3 rightStick = new Vector3(state.ThumbSticks.Right.X, 0f, state.ThumbSticks.Right.Y);

		player.SetDirectionalInput(leftStick, rightStick);


		//// Detect if a button was pressed this frame
		//if (prevState.Buttons.A == ButtonState.Released && state.Buttons.A == ButtonState.Pressed)
		//{
		//	GetComponent<Renderer>().material.color = new Color(Random.value, Random.value, Random.value, 1.0f);
		//}
		//// Detect if a button was released this frame
		//if (prevState.Buttons.A == ButtonState.Pressed && state.Buttons.A == ButtonState.Released)
		//{
		//	GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
		//}
	}

	void OnGUI()
	{
		string text = "Use left stick\n";
		text += string.Format("IsConnected {0} Packet #{1}\n", state.IsConnected, state.PacketNumber);
		text += string.Format("\tTriggers {0} {1}\n", state.Triggers.Left, state.Triggers.Right);
		text += string.Format("\tD-Pad {0} {1} {2} {3}\n", state.DPad.Up, state.DPad.Right, state.DPad.Down, state.DPad.Left);
		text += string.Format("\tButtons Start {0} Back {1} Guide {2}\n", state.Buttons.Start, state.Buttons.Back, state.Buttons.Guide);
		text += string.Format("\tButtons LeftStick {0} RightStick {1} LeftShoulder {2} RightShoulder {3}\n", state.Buttons.LeftStick, state.Buttons.RightStick, state.Buttons.LeftShoulder, state.Buttons.RightShoulder);
		text += string.Format("\tButtons A {0} B {1} X {2} Y {3}\n", state.Buttons.A, state.Buttons.B, state.Buttons.X, state.Buttons.Y);
		text += string.Format("\tSticks Left {0} {1} Right {2} {3}\n", state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y, state.ThumbSticks.Right.X, state.ThumbSticks.Right.Y);
		GUI.Label(new Rect(0, 0, Screen.width, Screen.height), text);
	}
}
