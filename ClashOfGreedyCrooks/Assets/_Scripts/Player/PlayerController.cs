using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[HideInInspector] public Shooting shootS;

	private float deadzone;
	public float speed = 5f;
	private float aimSensitivity = 0.1f;
	private float inputAngle = 0f;
	private float viewAngle = 0f;
	private float dt = 0f;

	public float attackSpeed;
	private bool cooldown;

	private Vector3 movement;
	private Vector3 directionalInputLeftStick;
	private Vector3 directionalInputRightStick;


	private void Start()
	{
		directionalInputLeftStick = Vector3.zero;
		directionalInputRightStick = Vector3.zero;
		movement = Vector3.zero;

		shootS = GetComponent<Shooting>();
	}

	private void Update()
	{
		dt = Time.deltaTime;

		MovePlayer();
	}

	private void MovePlayer()
	{
		movement = directionalInputLeftStick.normalized * speed * dt;
		transform.position += movement;
		print(directionalInputLeftStick.normalized);

		inputAngle = Mathf.Atan2(directionalInputRightStick.x, directionalInputRightStick.z) * Mathf.Rad2Deg;
		viewAngle = Mathf.LerpAngle(viewAngle, inputAngle, 0.1f);

		transform.rotation = Quaternion.AngleAxis(viewAngle, Vector3.up);
	}

	public void Shoot()
	{
		if (!cooldown)
		{
			shootS.Shoot();
			cooldown = true;
			Invoke("CooldownTimer", attackSpeed);

		}
	}

	private void CooldownTimer()
	{
		cooldown = false;
	}

	public void SetDirectionalInput(Vector3 leftStick, Vector3 rightStick)
	{
		directionalInputLeftStick = leftStick;

		if (rightStick.magnitude > 0)
		{
			directionalInputRightStick = rightStick;
		}
	}
}
