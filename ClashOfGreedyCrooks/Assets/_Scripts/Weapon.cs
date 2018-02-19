using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	public new string name;
	public float damage = 20f;
	public float projectileSpeed = 20f;

	[Header("This is the min & max spread of projectiles")]
	[Range(-15, 0)]
	public int spreadMin;
	[Range(0, 15)]
	public int spreadMax;

	public bool blindFolded;

	public Projectile proj;
	private Transform bulletSpawnPoint;
	private GameObject bulletspawned;

	FMODUnity.StudioEventEmitter a_shoot;

	public void Start()
	{
		InitializeAudio();

		bulletSpawnPoint = transform.Find("Muzzle");
	}

	public void Shoot(GameObject player)
	{
		a_shoot.Play();

		bulletspawned = Instantiate(proj.gameObject, bulletSpawnPoint.position, bulletSpawnPoint.transform.rotation);

		if (blindFolded)
		{
			bulletspawned.transform.Rotate(new Vector3(0.0f, Random.Range(spreadMin, spreadMax), 0.0f));
		}

		bulletspawned.GetComponent<Projectile>().SetReferences(damage, projectileSpeed, player);
	}

	private void InitializeAudio()
	{
		a_shoot = gameObject.AddComponent<FMODUnity.StudioEventEmitter>();
		a_shoot.Event = "event:/Arena/playerShoot";
		a_shoot.Preload = true;
	}
}
