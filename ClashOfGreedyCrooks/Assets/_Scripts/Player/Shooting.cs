using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Instantiates a bullet in the direction that the player is facing.
/// </summary>
public class Shooting : MonoBehaviour {

    public float projectileSpeed = 20;
    public float damage = 20;

    public Projectile proj;
    
    public Transform bulletSpawnPoint;

    private float timer;
    private float shootDisable = 0.5f;

    private GameObject bulletspawned;

	FMODUnity.StudioEventEmitter a_shoot;

    //TODO: Input should come through the PlayerController / which button activates it should be derived from the PlayerController.
    private void Start()
    {
		InitializeAudio();
        bulletSpawnPoint = transform.Find("Champion").GetChild(0).Find("WeaponHold").GetChild(0).Find("Muzzle");
    }

	private void InitializeAudio() {
		a_shoot = gameObject.AddComponent<FMODUnity.StudioEventEmitter>();
		a_shoot.Event = "event:/Arena/playerShoot";
		a_shoot.Preload = true;
	}

	//TODO: Remove this when real Keyboard support is added.
	private void Update() {
		if (Input.GetKeyDown(KeyCode.P)) {
			Shoot();
		}
	}

	//Spawns the bullet
	public void Shoot()
    {
		a_shoot.Play();

        bulletspawned = Instantiate(proj.gameObject, bulletSpawnPoint.position, Quaternion.identity);
        bulletspawned.transform.rotation = bulletSpawnPoint.transform.rotation;
        bulletspawned.GetComponent<Projectile>().damage = damage;
        bulletspawned.GetComponent<Projectile>().projectileSpeed = projectileSpeed;
        bulletspawned.GetComponent<Projectile>().player = gameObject;
    }
}
