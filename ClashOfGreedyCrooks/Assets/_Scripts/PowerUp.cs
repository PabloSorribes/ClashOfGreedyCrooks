using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {
    private GameObject PowerUps;

    private GameObject[] PowerUpChilds;

    float Health = 20f, Damage = 2f, AttackSpeed = 1.5f, Movement = 1.5f;

    int PowerNumber;

    bool powerSpawned;

    float timeNow;

    float powerUpSpawns;

    int flashTimes;

    public void Start()
    {
        powerUpSpawns = Random.Range(25, 50);
        powerSpawned = false;

        PowerUpChilds = new GameObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            PowerUpChilds[i] = transform.GetChild(i).gameObject;
        }
    }

    private void OnEnable()
    {
        //TODO: Insert Particle here.
    }

    private void LateUpdate()
    {
        timeNow = TimeManager.GetInstance.trackTime;

        if (timeNow <= 50f && powerSpawned == false)
        {
            PowerNumber = GetNewPowerUp();
            GeneratePowerUp();
            powerSpawned = true;
        }
            
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (PowerNumber == 0)
            {
                other.GetComponentInChildren<Weapon>().damage *= Damage;

            }
            if (PowerNumber == 1)
            {
                other.GetComponent<PlayerController>().attackSpeed /= AttackSpeed;

            }
            if (PowerNumber == 2)
            {
                other.GetComponent<PlayerHealth>().Heal(Health);

            }
            if (PowerNumber == 3)
            {
                other.GetComponent<PlayerController>().speed *= Movement;

            }
            if(PowerNumber == 4)
            {
                for (int i = 0; i < GetComponent<Penalty>().specialPenalties.Length; i++)
                {
                    if (GetComponent<Penalty>().specialPenalties[i] == true)
                    {

                    }
                }
            }
            
            

            PowerUpChilds[PowerNumber].SetActive(false);
            Destroy(this.gameObject);
        }
    }
    

    int GetNewPowerUp()
    {
        int powerUpNumber = (int)Random.Range(0, 3);

        return powerUpNumber;
    }

    void GeneratePowerUp()
    {

        PowerUpChilds[PowerNumber].SetActive(true);

        Invoke("FlashPowerUp", 0.1f);
       
     }

    void FlashPowerUp()
    {
        if(flashTimes < 5)
        {
            PowerUpChilds[PowerNumber].SetActive(false);
            Invoke("GeneratePowerUp", 0.1f);
            flashTimes++;
        }
    }
    
    void SpawnParticles()
    {
        //TODO: Create spawn particle effect with symbol.
    }

}
