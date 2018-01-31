using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public int speed;
    private int dmg;
    GameObject player;

    private float deathTimer;

    //Time until the bullet is destroyed
    public float defaultTime = 5;


    private void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);

        deathTimer += Time.deltaTime;

        if (deathTimer > defaultTime)
        {
            Destroy(gameObject);

            deathTimer = 0;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag=="Player")
        {
            player = collision.gameObject;
            //player.GetComponent<Player>().health-=dmg;
        }
      
    }
}
