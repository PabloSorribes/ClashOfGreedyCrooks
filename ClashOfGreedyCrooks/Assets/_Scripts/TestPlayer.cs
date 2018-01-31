using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{

	private Vector3 directionalInputLeftStick;
	private Vector3 directionalInputRightStick;

	public float speed = 5f;
	private float aimAngle = 0f;
	private float dt = 0f;
	Vector3 movement;

	private void Start()
	{
		directionalInputLeftStick = Vector3.zero;
		directionalInputRightStick = Vector3.zero;
		movement = Vector3.zero;
	}

	private void Update()
	{
		dt = Time.deltaTime;

		CalculateMovement();
		MovePlayer();
	}

	private void CalculateMovement()
	{
		movement = new Vector3(directionalInputLeftStick.x, 0f, directionalInputLeftStick.z);
		aimAngle = Mathf.Atan2(directionalInputRightStick.x, directionalInputRightStick.z) * Mathf.Rad2Deg;
	}

	private void MovePlayer()
	{
		transform.position += movement * dt * speed;
		transform.rotation = Quaternion.AngleAxis(aimAngle, Vector3.up);
	}

	public void Shoot()
	{

	}

	public void SetDirectionalInput(Vector3 leftStick, Vector3 rightStick)
	{
		directionalInputLeftStick = leftStick;
		directionalInputRightStick = rightStick;
	}
}