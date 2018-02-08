using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

    private TimeManager timeManager;
    private Rigidbody rb;

    private Slider healthBar;

    public ParticleSystem ps;
    private bool emit;

    private float maxHealth = 100f;
    public float currentHealth;
    
    [Header("DEATH CIRCLE VALUES")]
    public float hurtCooldown = 2;
    private float timer;
    public bool insideDeathCircle;

    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        
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

        if (rb.IsSleeping())
        {
            rb.WakeUp();
        }

        if (currentHealth <= 0)
        {
			gameObject.SetActive(false);
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
			gameObject.SetActive(false);
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
    private void OnDisable()
    {
        DeathParticles();
		ArenaManager.GetInstance.HandlePlayerDeath(this.gameObject);
    }

    private void DeathParticles ()
    {
        Destroy(Instantiate(ps.gameObject, this.transform.position, Quaternion.FromToRotation(Vector3.forward, Vector3.up)) as GameObject, 2f);
    }
}
