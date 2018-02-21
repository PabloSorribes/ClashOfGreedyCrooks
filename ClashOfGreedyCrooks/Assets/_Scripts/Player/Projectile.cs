using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The base script for moving the projectiles forward on instantiation.
/// </summary>
public class Projectile : MonoBehaviour
{
	private float projectileSpeed;
	private float damage;
	private float bulletLifeTime;

	private GameObject player;

	private void Update()
	{
		transform.Translate(Vector3.forward * Time.deltaTime * projectileSpeed);

		Destroy(gameObject, bulletLifeTime);
	}

	public void ProjectileSetup(float damage, float projectileSpeed, float bulletLifeTime, GameObject player)
	{
		this.damage = damage;
		this.projectileSpeed = projectileSpeed;
		this.player = player;
        this.bulletLifeTime = bulletLifeTime;

		player.GetComponent<PlayerInfo>().TotalShotsFired++;
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Wall")
		{
			Destroy(this.gameObject);
		}

		if (other.gameObject.tag == "Player" && other.gameObject != player)
		{
			player.GetComponent<PlayerInfo>().TotalHits++;
			player.GetComponent<PlayerInfo>().TotalDamage += damage;

			other.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage, player.GetComponent<PlayerInfo>());

			Destroy(gameObject);
		}
	}
}
