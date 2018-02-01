using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Instantiates a bullet in the direction that the player is facing.
/// </summary>
public class Shooting : MonoBehaviour {

    public GameObject bullet;
    public GameObject bulletSpawnPoint;

    private Transform bulletspawned;

	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
	}

    //Spanws the bullet
    public void Shoot()
    {
        bulletspawned = Instantiate(bullet.transform, bulletSpawnPoint.transform.position, Quaternion.identity);
        bulletspawned.rotation = bulletSpawnPoint.transform.rotation;
    }
}
