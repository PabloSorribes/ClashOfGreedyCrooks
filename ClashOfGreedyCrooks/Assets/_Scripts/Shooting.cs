using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Instantiates a bullet in the direction that the player is facing.
/// </summary>
public class Shooting : MonoBehaviour {

    public GameObject bullet;
    public GameObject bulletSpawnPoint;

    private float timer;
    private float shootDisable = 0.5f;

    private Transform bulletspawned;

	//TODO: Input should come through the PlayerController / which button activates it should be derived from the PlayerController.
	void Update () {

        timer += Time.deltaTime;

        if (Input.GetButtonDown("Fire1") && timer > shootDisable)
        {
            Shoot();
            timer = 0;
        }
	}

    //Spanws the bullet
    public void Shoot()
    {
        bulletspawned = Instantiate(bullet.transform, bulletSpawnPoint.transform.position, Quaternion.identity);
        bulletspawned.rotation = bulletSpawnPoint.transform.rotation;
    }
}
