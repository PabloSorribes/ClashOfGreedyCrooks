using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The base script for moving the projectiles forward on instantiation by Shooting.cs.
/// </summary>
public class Projectile : MonoBehaviour {

    public float projectileSpeed = 20;
    public float damage = 20;

    private float deathTimer;

    public GameObject player;

    //Time until the bullet is destroyed
    private float defaultTime = 1.2f;

	private void Start()
	{
			player.GetComponent<PlayerInfo>().CurrentRoundShotsFired++;
	}

	private void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * projectileSpeed);

        deathTimer += Time.deltaTime;

        if (deathTimer > defaultTime)
        {
            Destroy(gameObject);

            deathTimer = 0;
        }
    }

    

    public void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Player" && collision.gameObject != this.player)
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);

			this.player.GetComponent<PlayerInfo>().CurrentRoundHits ++;
			this.player.GetComponent<PlayerInfo>().CurrentRoundDamage += damage;

			Destroy(gameObject);
        }

        if(collision.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }



}
