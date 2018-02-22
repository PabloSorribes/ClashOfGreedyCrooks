using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherBlast : MonoBehaviour {

    private float projectileSpeed;
    private float damage;
    private float bulletLifeTime;

    private GameObject player;

    public void ProjectileSetup(float damage, float projectileSpeed, float bulletLifeTime, GameObject player)
    {
        this.damage = damage;
        this.projectileSpeed = projectileSpeed;
        this.player = player;
        this.bulletLifeTime = bulletLifeTime;

        player.GetComponent<PlayerInfo>().totalShotsFired++;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player" && other.gameObject != player)
        {
            player.GetComponent<PlayerInfo>().totalHits++;
            player.GetComponent<PlayerInfo>().totalDamage += damage;

            other.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage, player.GetComponent<PlayerInfo>());

            Destroy(gameObject);
        }
    }
}
