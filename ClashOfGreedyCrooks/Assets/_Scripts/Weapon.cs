using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	public new string name;
	[HideInInspector] public float damage;
	public float projectileSpeed;
    public float bulletLifeTime;
    public int magSize;
    private int projectilesInMag;

	[Header("This is the min & max spread of projectiles")]
	[Range(-15, 0)]
	public int spreadMin;
	[Range(0, 15)]
	public int spreadMax;

	public bool blindFolded;

    public bool victorious;

    public Projectile proj;
    public ParticleSystem particle;
	private Transform bulletSpawnPoint;
	private GameObject bulletspawned;

	FMODUnity.StudioEventEmitter a_shoot;

	public void Start()
	{
		InitializeAudio();

		bulletSpawnPoint = transform.Find("Muzzle");
	}

	private void InitializeAudio()
	{
		string eventPath = "event:/Arena/playerShootBuoy";

		if (name == "Buoy")
			eventPath = "event:/Arena/playerShootBuoy";
		if (name == "Katana")
			eventPath = "event:/Arena/playerShootKatana";
		if (name == "CorgiLauncher")
			eventPath = "event:/Arena/playerShootLauncher";
		if (name == "Wand")
			eventPath = "event:/Arena/playerShootWand";

		a_shoot = gameObject.AddComponent<FMODUnity.StudioEventEmitter>();
		a_shoot.Event = eventPath;
		a_shoot.Preload = true;
	}

	public void Shoot(GameObject player)
	{
        if (victorious)
            return;

		a_shoot.Play();

		bulletspawned = Instantiate(proj.gameObject, bulletSpawnPoint.position, bulletSpawnPoint.transform.rotation);

        Destroy(Instantiate(particle.gameObject, bulletSpawnPoint.position, bulletSpawnPoint.transform.rotation) as GameObject, 1.5f);

        if (blindFolded)
		{
			bulletspawned.transform.Rotate(new Vector3(0.0f, Random.Range(spreadMin, spreadMax), 0.0f));
		}

		bulletspawned.GetComponent<Projectile>().ProjectileSetup(damage, projectileSpeed, bulletLifeTime, player);
	}

}
