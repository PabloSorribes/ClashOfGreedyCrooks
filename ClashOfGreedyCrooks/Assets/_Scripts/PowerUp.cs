using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {
    private GameObject PowerUps;

    private GameObject[] PowerUpChilds;

    float Health = 20f, Damage = 2f, AttackSpeed = 1.5f, Movement = 1.5f;

    int PowerNumber;


    public void Start()
    {
        PowerUpChilds = new GameObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            PowerUpChilds[i] = transform.GetChild(i).gameObject;
        }
        PowerNumber = GetNewPowerUp();
        GeneratePowerUp(PowerNumber);
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (PowerNumber == 0)
            {
                PowerUps.GetComponentInChildren<Weapon>().damage += Damage;

            }
            if (PowerNumber == 1)
            {
                PowerUps.GetComponent<PlayerController>().attackSpeed += AttackSpeed;

            }
            if (PowerNumber == 2)
            {
                PowerUps.GetComponent<PlayerHealth>().currentHealth += Health;

            }
            if (PowerNumber == 3)
            {
                PowerUps.GetComponent<PlayerController>().speed += Movement;

            }

            Destroy(this.gameObject);
        }
    }
    

    int GetNewPowerUp()
    {
        int powerUpNumber = (int)Random.Range(1, 3);

        return powerUpNumber;
    }

    void GeneratePowerUp(int number)
    {
        PowerUpChilds[number].SetActive(true);
    }


}
