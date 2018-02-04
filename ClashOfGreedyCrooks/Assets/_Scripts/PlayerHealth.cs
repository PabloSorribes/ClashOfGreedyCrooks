using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    private int maxHealth = 100;
    public int currentHealth;
    TimeManager timeManager;

    // Use this for initialization
    void Start () {
        timeManager = TimeManager.GetInstance;

        currentHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
        if (currentHealth <= 0)
        {
            //timeManager.StartFreezeFrame();

            
            Destroy(gameObject);
        }       
	}

    public void TakeDamage (int p_damage)
    {
        currentHealth -= p_damage;            
    }
    //TODO: Fix freeze frame!

    //private void OnDestroy()
    //{
    //    TimeManager.GetInstance().StartFreezeFrame();
    //}
}
