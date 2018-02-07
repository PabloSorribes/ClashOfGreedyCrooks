using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float projectileSpeed = 20;
    public float damage = 20;

    private float deathTimer;

    public GameObject player;

    //Time until the bullet is destroyed
    private float defaultTime = 5;

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

            
        }

        Destroy(gameObject);
      
    }
}
