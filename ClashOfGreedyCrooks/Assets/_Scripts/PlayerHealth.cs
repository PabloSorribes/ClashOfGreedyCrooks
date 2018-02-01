using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    private int maxHealth = 100;
    public int currentHealth;
    Projectile projectile;

    // Use this for initialization
    void Start () {
        currentHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }       
	}

    public void TakeDamage (int p_damage)
    {
        currentHealth -= p_damage;            
    }
}
