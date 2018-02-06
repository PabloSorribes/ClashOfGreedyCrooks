using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Instantiates a bullet in the direction that the player is facing.
/// </summary>
public class Shooting : MonoBehaviour {

    public Projectile proj;

    
    public GameObject bulletSpawnPoint;

    private float timer;
    private float shootDisable = 0.5f;

    private GameObject bulletspawned;

	//TODO: Input should come through the PlayerController / which button activates it should be derived from the PlayerController.
	void Update () {

        timer += Time.deltaTime;

        if (Input.GetButtonDown("Fire1") && timer > shootDisable)
        {
            Shoot();
            timer = 0;
        }
	}

    //Spawns the bullet
    public void Shoot()
    {
        bulletspawned = Instantiate(proj.gameObject, bulletSpawnPoint.transform.position, Quaternion.identity);
        bulletspawned.transform.rotation = bulletSpawnPoint.transform.rotation;

        
    }
}
