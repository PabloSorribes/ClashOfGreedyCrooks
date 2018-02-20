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

    public void Start()
    {
        
        PowerUpChilds = new GameObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            PowerUpChilds[i] = transform.GetChild(i).gameObject;
        }

        
        
    }
    private void LateUpdate()
    {
        timeNow = TimeManager.GetInstance.trackTime;

        if (timeNow <= 50f && powerSpawned == false)
        {
            PowerNumber = GetNewPowerUp();
            GeneratePowerUp(PowerNumber);
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

            

            PowerUpChilds[PowerNumber].SetActive(false);
            Destroy(this.gameObject);
        }
    }
    

    int GetNewPowerUp()
    {
        int powerUpNumber = (int)Random.Range(0, 3);

        return powerUpNumber;
    }

    void GeneratePowerUp(int number)
    {
        PowerUpChilds[number].SetActive(true);
    }


}
