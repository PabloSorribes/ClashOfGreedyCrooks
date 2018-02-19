using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public new string name;
    public float damage = 20;
    public float projectileSpeed = 20;

    [Header ("This is the min & max spread of projectiles")]
    [Range(-15, 0)]
    public int spreadMin;
    [Range(0, 15)]
    public int spreadMax;

    public bool blindFolded;

    //public string pathToModel = "Weapons/";
    //public int damageMin;
    //public int damageMax;
    //public int attackSpeedMin;
    //public int attackSpeedMax;
    //public int range;
    //public int projectileSpeed;
    //public int projectileSpread;

    public Projectile proj;
    private Transform bulletSpawnPoint;
    private GameObject bulletspawned;


    public void Start()
    {
        bulletSpawnPoint = transform.Find("Muzzle");

        //TODO: Uncomment after nerf has been implemented fully
        //PlayerTokens.GetInstance.Drunk += BlindfoldedShot;
    }

    public void InstantiateBullet(GameObject player)
    {
        bulletspawned = Instantiate(proj.gameObject, bulletSpawnPoint.position, bulletSpawnPoint.transform.rotation);

        if (blindFolded)
        {
            bulletspawned.transform.Rotate(new Vector3(0.0f, Random.Range(spreadMin, spreadMax), 0.0f));
        }


        bulletspawned.GetComponent<Projectile>().damage = damage;
        bulletspawned.GetComponent<Projectile>().projectileSpeed = projectileSpeed;
        bulletspawned.GetComponent<Projectile>().player = player;
    }

    //A player token nerf
    //TODO: Maked the shot shoot random inside of an angle
    //public void BlindfoldedShot()
    //{
    //	bulletspawned = Instantiate(proj.gameObject, bulletSpawnPoint.position, Quaternion.identity);
    //	bulletspawned.transform.rotation = bulletSpawnPoint.transform.rotation;
    //	bulletspawned.GetComponent<Projectile>().damage = damage;
    //	bulletspawned.GetComponent<Projectile>().projectileSpeed = projectileSpeed;
    //	bulletspawned.GetComponent<Projectile>().player = gameObject;
    //}
}
