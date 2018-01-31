using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour {

    public GameObject bullet;
    public GameObject bulletSpawnPoint;


	// Use this for initialization
	void Start () {
        
	}
	
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
        Instantiate(bullet.transform, bulletSpawnPoint.transform.position, Quaternion.identity);

    }
}
