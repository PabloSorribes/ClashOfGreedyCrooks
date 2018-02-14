using System.Collections.Generic;
using UnityEngine;

public enum Nerf { Blind, Drunk, Fat, Stat }

public class Penalty : MonoBehaviour
{
    private Champion champion;
    private bool canAddPenalty;
    private bool statPenalty;
    private bool[] specialPenalties;
    private int specialPenaltiesAdded;

    private void Awake()
    {
        specialPenalties = new bool[3];
        champion = GetComponent<Champion>();
        statPenalty = true;
        CanAddPenalty = true;
    }

    public bool CanAddPenalty { get; set; }

    public void AddPenalty(CardComponent card, PickingResources resources)
    {
        if (!statPenalty)
        {
            int specialNerf = Random.Range(0, 3);
            while (specialPenalties[specialNerf] == true)
            {
                specialNerf = Random.Range(0, 3);
            }
            specialPenalties[specialNerf] = true;
            card.penalties[specialNerf].sprite = resources.penaltySprites[specialNerf];
            specialPenaltiesAdded++;
            if (specialPenaltiesAdded == 3)
            {
                CanAddPenalty = false;
                card.locked.SetActive(true);
            }
            statPenalty = true;
        }
        else
        {
            int firstStat = Random.Range(0, 4);
            int secondStat = Random.Range(0, 4);
            while (secondStat == firstStat)
            {
                secondStat = Random.Range(0, 4);
            }
            EditChampionStats(firstStat, 2f, card);
            EditChampionStats(secondStat, 2f, card);
            statPenalty = false;
        }
    }

    private void EditChampionStats(int number, float amount, CardComponent card)
    {
        switch (number)
        {
            case 0:
                champion.Health -= amount;
                if (champion.Health <= 0)
                    champion.Health = 0;
                card.healthText.text = champion.Health.ToString();
                card.healthText.color = Color.red;
                break;
            case 1:
                champion.Movement -= amount;
                if (champion.Movement <= 0)
                    champion.Movement = 0;
                card.movementText.text = champion.Movement.ToString();
                card.movementText.color = Color.red;
                break;
            case 2:
                champion.Damage -= amount;
                if (champion.Damage <= 0)
                    champion.Damage = 0;
                card.damageText.text = champion.Damage.ToString();
                card.damageText.color = Color.red;
                break;
            case 3:
                champion.AttackSpeed -= amount;
                if (champion.AttackSpeed <= 0)
                    champion.AttackSpeed = 0;
                card.attackSpeedText.text = champion.AttackSpeed.ToString();
                card.attackSpeedText.color = Color.red;
                break;
        }
        
    }
}
