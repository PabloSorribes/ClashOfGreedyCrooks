﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

    private TimeManager timeManager;
    private Rigidbody rigidbody;

    private Slider healthBar;

    private float maxHealth = 100f;
    public float currentHealth;
    
    [Header("DEATH CIRCLE VALUES")]
    public float hurtCooldown = 2;
    private float timer;
    public bool insideDeathCircle;

    // Use this for initialization
    void Start ()
    {
        rigidbody = GetComponent<Rigidbody>();
        
        timeManager = TimeManager.GetInstance;

        currentHealth = maxHealth;

        healthBar = transform.Find("HealthBar").GetChild(0).GetComponent<Slider>();
	}

    public void SetStartHealth(float startHealth)
    {
        maxHealth = currentHealth = startHealth;
        //CalculateHealthPrecentage();
    }
	
	// Update is called once per frame
	void Update ()
    {
        timer += Time.deltaTime;

        if (timer > hurtCooldown && !insideDeathCircle)
        {
            TakeDamageOutside(DeathCircle.GetInstance.deathZoneDamage);
            timer = 0;
        }

        if (rigidbody.IsSleeping())
        {
            rigidbody.WakeUp();
        }

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
        
	}

    /// <summary>
    /// Should be called by bullets etc
    /// </summary>
    /// <param name="p_damage"></param>
    public void TakeDamage (float p_damage)
    {
        currentHealth -= p_damage;
        CalculateHealthPrecentage();
      
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Should be called by DeathCircle
    /// </summary>
    /// <param name="deathZoneDamage"></param>
    public void TakeDamageOutside(int deathZoneDamage)
    {
        currentHealth -= deathZoneDamage;
        CalculateHealthPrecentage();
    }

    private void CalculateHealthPrecentage()
    {
        float healthPrecentage = currentHealth / maxHealth;
        Debug.Log(healthPrecentage);
        healthBar.value = healthPrecentage;
    }
    //TODO: Talk to ArenaManager and what values should i send?
    private void OnDestroy()
    {
        ArenaManager.GetInstance.HandlePlayerDeath(this.gameObject);
    }
}
