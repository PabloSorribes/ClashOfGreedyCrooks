using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public int speed;
    private int dmg;
    GameObject player;


    private void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    public void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag=="Player")
        {
            player = collision.gameObject;
            //player.GetComponent<Player>().health-=dmg;
        }
        else
        {

        }
    }
}
