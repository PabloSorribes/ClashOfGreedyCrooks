using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The base script for moving the projectiles forward on instantiation.
/// </summary>
public class Projectile : MonoBehaviour
{
	private float damage;
	private float projectileSpeed;
	private float projectileLifeTime;
	private GameObject player;

	//For defining different movement & destroy-behaviours, as well as sounds.
	public enum ProjectileType { buoy, katana, launcher, wand }
	public ProjectileType projectileType;
	public ParticleSystem collisionParticle;

	private void Update()
	{
		transform.Translate(Vector3.forward * Time.deltaTime * projectileSpeed);

		Destroy(gameObject, projectileLifeTime);
	}

	/// <summary>
	/// Values here are set by the Weapon.cs which has the Projectile-prefab attached to it.
	/// </summary>
	/// <param name="damage"></param>
	/// <param name="projectileSpeed"></param>
	/// <param name="projectileLifeTime"></param>
	/// <param name="player"></param>
	public void ProjectileSetup(float damage, float projectileSpeed, float projectileLifeTime, GameObject player)
	{
		this.damage = damage;
		this.projectileSpeed = projectileSpeed;
		this.projectileLifeTime = projectileLifeTime;
		this.player = player;

		player.GetComponent<PlayerInfo>().totalShotsFired++;
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Wall")
		{
			Destroy(this.gameObject);
		}

		if (other.gameObject.tag == "Player" && other.gameObject != player)
		{
			if (player.GetComponent<PlayerInfo>() != null)
			{
				player.GetComponent<PlayerInfo>().totalHits++;
				player.GetComponent<PlayerInfo>().totalDamage += damage;
			}

			other.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage, player.GetComponent<PlayerInfo>());

            Destroy(gameObject);
		}
	}

	private void OnDestroy()
	{
        CollisionParticles();

		//TODO: Add different behaviours for the projectile types.
        switch (projectileType)
		{
			case ProjectileType.buoy:
				break;
			case ProjectileType.katana:
				break;
			case ProjectileType.launcher:
                //TODO: AoE explosion damage.
				break;
			case ProjectileType.wand:
				break;
			default:
				break;
		}

		AudioManager.GetInstance.PlayOneShot3D("event:/Arena/projectileCollision", transform.position, "champ", GetFmodParameter());
	}

	private void CollisionParticles()
	{
		Destroy(Instantiate(collisionParticle.gameObject, this.transform.GetChild(0).position, Quaternion.FromToRotation(Vector3.forward, Vector3.up)) as GameObject, 1.5f);
	}

	private float GetFmodParameter()
	{
		float parameter = 0;

		switch (projectileType)
		{
			case ProjectileType.buoy:
				parameter = 0;
				break;
			case ProjectileType.katana:
				parameter = 1;
				break;
			case ProjectileType.launcher:
				parameter = 2;
				break;
			case ProjectileType.wand:
				parameter = 3;
				break;
			default:
				break;
		}
		return parameter;
	}
}
