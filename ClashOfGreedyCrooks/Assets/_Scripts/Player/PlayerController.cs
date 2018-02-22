using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float speed = 5f;
	private float aimSpeed = 0.5f;
	private float inputAngle = 0f;
	private float viewAngle = 0f;

	private float moveSpeed;
	public float attackSpeed;
	private bool cooldown;

	private Vector3 movement;
	private Vector3 directionalInputLeftStick;
	private Vector3 directionalInputRightStick;

    public bool victorious;

    private Weapon weapon;
	private Rigidbody rb;

	public Animator animator;
	private AnimatorOverrideController animatorOverride;


	private void Start()
	{
		animator = GetComponentInChildren<Animator>();

		directionalInputLeftStick = Vector3.zero;
		directionalInputRightStick = Vector3.zero;
		movement = Vector3.zero;
		moveSpeed = speed;

		rb = GetComponent<Rigidbody>();
		weapon = GetComponentInChildren<Weapon>();

		DontDestroyOnLoad(gameObject);
	}

	private void FixedUpdate()
	{
        if (victorious)
        {
            rb.velocity = Vector3.zero; 
            speed = 0;
            moveSpeed = 0;
            movement = Vector3.zero;
            return;
        }
		MovePlayer();
		AimPlayer();
	}

	private void MovePlayer()
	{
		movement = directionalInputLeftStick * moveSpeed;
		rb.velocity = Vector3.Lerp(rb.velocity, movement, 0.3f);

		if (directionalInputLeftStick != Vector3.zero && !cooldown)
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
		if (directionalInputRightStick.magnitude > 0)
		{

			//Convert directional input to rotation in degrees
			inputAngle = Mathf.Atan2(directionalInputRightStick.x, directionalInputRightStick.z) * Mathf.Rad2Deg;

			//Make the aiming more accurate(slower) on smaller inputs
			float aimSpeedMod = 1f;
			if (Mathf.Abs(Mathf.DeltaAngle(viewAngle, inputAngle)) < 10f)
				aimSpeedMod = 0.25f;
			//else if (Mathf.Abs(Mathf.DeltaAngle(viewAngle, inputAngle)) < 25f)
			//	aimSpeedMod = 0.5f;

			//Smoothly apply the input rotation to the view rotation and rotate the player
			viewAngle = Mathf.LerpAngle(viewAngle, inputAngle, aimSpeed * aimSpeedMod);
			rb.rotation = Quaternion.AngleAxis(viewAngle, Vector3.up);

		}
	}

	public void Shoot()
	{
		if (!cooldown)
		{
			//Make player slow when shooting
			moveSpeed = moveSpeed * 0.65f;
			//change shoot animation speed depending on attackspeed
			animator.speed = 1 / attackSpeed;

			animator.SetTrigger("toShooting");

			weapon.Shoot(gameObject);
			cooldown = true;
			Invoke("CooldownTimer", attackSpeed);
		}
	}

	private void CooldownTimer()
	{
		animator.speed = 1f;
		moveSpeed = speed;
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
