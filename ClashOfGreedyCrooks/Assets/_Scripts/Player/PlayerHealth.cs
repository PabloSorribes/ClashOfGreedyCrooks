﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
	private Rigidbody rb;

	private Slider healthBar;

	public ParticleSystem ps;
	private bool emit;

	private float maxHealth = 100f;
	public float currentHealth;

	[Header("DEATH CIRCLE VALUES")]
	public float hurtCooldown = 2;
	private float timer;
	public bool insideDeathCircle;

	FMODUnity.StudioEventEmitter a_deathSound;

	// Use this for initialization
	void Start()
	{
		InitializeAudio();

		rb = GetComponent<Rigidbody>();

		currentHealth = maxHealth;

		healthBar = transform.Find("HealthBar").GetChild(0).GetComponent<Slider>();
	}

	private void InitializeAudio()
	{
		a_deathSound = gameObject.AddComponent<FMODUnity.StudioEventEmitter>();
		a_deathSound.Event = "event:/Arena/playerDeath";
		a_deathSound.Preload = true;
	}

	public void SetStartHealth(float startHealth)
	{
		maxHealth = currentHealth = startHealth;
		//CalculateHealthPrecentage();
	}

	// Update is called once per frame
	void Update()
	{
		timer += Time.deltaTime;

		if (timer > hurtCooldown && !insideDeathCircle)
		{
			TakeDamageOutside(DeathCircle.GetInstance.deathZoneDamage);
			timer = 0;
		}

		if (rb.IsSleeping())
		{
			rb.WakeUp();
		}

		if (currentHealth <= 0)
		{
			KillPlayer();
		}
	}

	private void KillPlayer()
	{
		Camera.main.GetComponent<NewCameraController>().RemoveTarget(gameObject.name);
		a_deathSound.Play();
		DeathParticles();
		if (ArenaManager.GetInstance != null)
		{
			ArenaManager.GetInstance.HandlePlayerDeath(gameObject);
		}

		Destroy(gameObject);
	}

	/// <summary>
	/// Should be called by bullets etc
	/// </summary>
	/// <param name="p_damage"></param>
	public void TakeDamage(float p_damage)
	{
		currentHealth -= p_damage;
		CalculateHealthPrecentage();

		if (currentHealth <= 0)
		{
			KillPlayer();
		}
	}

	/// <summary>
	/// Should be called by DeathCircle
	/// </summary>
	/// <param name="deathZoneDamage"></param>
	public void TakeDamageOutside(int deathZoneDamage)
	{
		currentHealth -= deathZoneDamage;
		CalculateHealthPrecentage();
	}

	private void CalculateHealthPrecentage()
	{
		float healthPrecentage = currentHealth / maxHealth;
		Debug.Log(healthPrecentage);
		healthBar.value = healthPrecentage;
	}
	//TODO: Talk to ArenaManager and what values should i send?
	//private void OnDestroy()
	//{
	//	a_deathSound.Play();
	//	DeathParticles();
	//	if (ArenaManager.GetInstance != null)
	//	{
	//		ArenaManager.GetInstance.HandlePlayerDeath(gameObject);
	//	}
	//}

	private void DeathParticles()
	{
		Destroy(Instantiate(ps.gameObject, this.transform.position, Quaternion.FromToRotation(Vector3.forward, Vector3.up)) as GameObject, 2f);
	}
}