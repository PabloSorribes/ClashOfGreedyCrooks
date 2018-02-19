using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The base script for moving the projectiles forward on instantiation.
/// </summary>
public class Projectile : MonoBehaviour
{
	private float projectileSpeed = 20;
	private float damage = 20;
	private float bulletLifeTime = 1.2f;

	private GameObject player;


	private void Start()
	{
		//TODO: Remove this @fippan
		//player.GetComponent<PlayerInfo>().CurrentRoundShotsFired++;
	}

	private void Update()
	{
		transform.Translate(Vector3.forward * Time.deltaTime * projectileSpeed);

		Destroy(gameObject, bulletLifeTime);
	}

	public void SetReferences(float damage, float projectileSpeed, GameObject player)
	{
		this.damage = damage;
		this.projectileSpeed = projectileSpeed;
		this.player = player;
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Wall")
		{
			Destroy(this.gameObject);
		}

		if (other.gameObject.tag == "Player" && other.gameObject != player)
		{
			other.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);

			//this.player.GetComponent<PlayerInfo>().CurrentRoundHits++;
			//this.player.GetComponent<PlayerInfo>().CurrentRoundDamage += damage;

			Destroy(gameObject);
		}
	}
}
