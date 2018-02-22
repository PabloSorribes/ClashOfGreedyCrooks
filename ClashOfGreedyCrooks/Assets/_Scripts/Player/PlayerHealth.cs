using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
	private Rigidbody rb;
	private Slider healthBar;
	public ParticleSystem ps;

	private float maxHealth = 100f;
	public float currentHealth;

	[Header("DEATH CIRCLE VALUES")]
	private float hurtCooldown = 1.2f;
	private float timer;
	public bool insideDeathCircle = true;

	private string champName;
	private float parameterFmod;

	void Start()
	{
		insideDeathCircle = true;
		rb = GetComponent<Rigidbody>();

		currentHealth = maxHealth;

		healthBar = transform.Find("HealthBar").GetChild(0).GetComponent<Slider>();

		champName = GetComponentInChildren<Champion>().name;

		parameterFmod = GetFmodParameter();
	}

	private float GetFmodParameter()
	{
		float parameter = 0;

		if (champName == "TheBride")
			parameter = 0;
		if (champName == "TheQueen")
			parameter = 1;
		if (champName == "TheHoff")
			parameter = 2;
		if (champName == "TheWizard")
			parameter = 3;

		return parameter;
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

		KeepRigidBodyAwake();

		//if (currentHealth <= 0)
		//{
		//	KillPlayer();
		//}
	}

	/// <summary>
	/// Should be run in Update()
	/// </summary>
	private void KeepRigidBodyAwake()
	{
		if (rb.IsSleeping())
		{
			rb.WakeUp();
		}
	}

	private void KillPlayer()
	{
		KillSound();
		DeathParticles();

		Camera.main.GetComponent<NewCameraController>().RemoveTarget(gameObject.name);
		ArenaManager.GetInstance.HandlePlayerDeath(gameObject.GetComponent<PlayerInfo>());

		//Destroy(gameObject);
	}

	private void DeathParticles()
	{
		Destroy(Instantiate(ps.gameObject, this.transform.position, Quaternion.FromToRotation(Vector3.forward, Vector3.up)) as GameObject, 2f);
	}

	/// <summary>
	/// Should be called by bullets etc
	/// </summary>
	/// <param name="damage"></param>
	public void TakeDamage(float damage, PlayerInfo playerThatShot)
	{
		currentHealth -= damage;
		CalculateHealthPrecentage();
		HurtSound();

		if (currentHealth <= 0)
		{
			//if (playerThatShot != null)
				playerThatShot.totalKills++;

			KillPlayer();
		}
	}

	public void Heal(float amount)
	{
		currentHealth += amount;
		CalculateHealthPrecentage();
		HealSound();

		if (currentHealth >= maxHealth)
		{
			currentHealth = maxHealth;
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
		//Debug.Log(healthPrecentage);
		healthBar.value = healthPrecentage;
	}

	private void KillSound()
	{
		AudioManager.GetInstance.PlayOneShot3D("event:/Arena/playerDeath", this.transform.position, "champ", parameterFmod);
	}

	private void HurtSound()
	{
		AudioManager.GetInstance.PlayOneShot3D("event:/Arena/playerHurt", transform.position, "champ", parameterFmod);
	}

	private void HealSound()
	{
		AudioManager.GetInstance.PlayOneShot3D("event:/Arena/playerHeal", transform.position);
	}
}
