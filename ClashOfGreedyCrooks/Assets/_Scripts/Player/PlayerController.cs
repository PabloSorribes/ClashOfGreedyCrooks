using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

	public float speed = 5f;
	private float aimSpeed = 0.3f;
	private float inputAngle = 0f;
	private float viewAngle = 0f;

	public float attackSpeed;
	private bool cooldown;

	private Vector3 movement;
	private Vector3 directionalInputLeftStick;
	private Vector3 directionalInputRightStick;

	private Shooting shooting;
	private Rigidbody rb;

	private Animator animator;
	private AnimatorOverrideController animatorOverride;
	

	private void Start()
	{
		animator = GetComponent<Animator>();

		//animator.runtimeAnimatorController = Resources.Load<AnimatorOverrideController>(
		//	GetComponentInChildren<Champion>().name + GetComponentInChildren<Weapon>().name) as AnimatorOverrideController;

		directionalInputLeftStick = Vector3.zero;
		directionalInputRightStick = Vector3.zero;
		movement = Vector3.zero;

		rb = GetComponent<Rigidbody>();
		shooting = GetComponent<Shooting>();

		DontDestroyOnLoad(gameObject);
	}

	private void FixedUpdate()
	{
		MovePlayer();
		AimPlayer();
	}

	private void MovePlayer()
	{
		movement = directionalInputLeftStick * speed;
		rb.velocity = Vector3.Lerp(rb.velocity, movement, 0.7f);

		if (directionalInputLeftStick != Vector3.zero)
		{
			animator.SetBool("isRunning", true);
		}
		else
		{
			animator.SetBool("isRunning", false);
		}

	}

	private void AimPlayer()
	{
		//Calculate directional input to rotation in degrees
		inputAngle = Mathf.Atan2(directionalInputRightStick.x, directionalInputRightStick.z) * Mathf.Rad2Deg;

		//Make the aiming more accurate(slower) on smaller inputs
		float aimSpeedMod = 1f;
		if (Mathf.Abs(Mathf.DeltaAngle(viewAngle, inputAngle)) < 10f)
			aimSpeedMod = 0.6f;
		else if (Mathf.Abs(Mathf.DeltaAngle(viewAngle, inputAngle)) < 25f)
			aimSpeedMod = 0.8f;
		
		//Smoothly apply the input rotation to the view rotation and rotate the player
		viewAngle = Mathf.LerpAngle(viewAngle, inputAngle, aimSpeed * aimSpeedMod);
		rb.rotation = Quaternion.AngleAxis(viewAngle, Vector3.up);
	}

	public void Shoot()
	{
		if (!cooldown)
		{
			animator.speed = 1f;
			if (directionalInputLeftStick != Vector3.zero)
			{
				animator.SetTrigger("toShooting");
			}
			else
			{
				animator.SetTrigger("toShooting");
			}

			shooting.Shoot();
			cooldown = true;
			Invoke("CooldownTimer", attackSpeed);

			animator.speed = 1f;
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
