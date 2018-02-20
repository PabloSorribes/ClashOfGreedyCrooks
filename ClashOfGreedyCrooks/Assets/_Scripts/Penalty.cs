using System.Collections;
using UnityEngine;

public class Penalty : MonoBehaviour
{
    private Champion champion;
    private bool canAddPenalty;
    public bool[] specialPenalties;
    private CardComponent card;
    private PickingResources resources;
    private int specialPenaltiesAdded;
    private int totalPenaltiesAdded;
    private int picksWithNoSpecialPenalty;
    private int randomSpecial;

    private void Awake()
    {
        specialPenalties = new bool[3];
        champion = GetComponent<Champion>();
        CanAddPenalty = true;
        randomSpecial = 3;
    }

    public bool CanAddPenalty { get; set; }

    public void AddPenalty(CardComponent card, PickingResources resources)
    {
        this.card = card;
        this.resources = resources;

        int random = Random.Range(0, randomSpecial - picksWithNoSpecialPenalty);

        if (random == 0 && specialPenaltiesAdded < 2)
        {
            Special();
            picksWithNoSpecialPenalty = 0;
            randomSpecial = 3 + specialPenaltiesAdded;
        }
        else
        {
            Stats();
            picksWithNoSpecialPenalty++;
        }

        totalPenaltiesAdded++;
        if (totalPenaltiesAdded == 5)
        {
            Lock();
        }
    }

    private void Special()
    {
        int specialNerf = Random.Range(0, 3);
        while (specialPenalties[specialNerf] == true)
        {
            specialNerf = Random.Range(0, 3);
        }
        specialPenalties[specialNerf] = true;
        card.penalties[specialNerf].sprite = resources.penaltySprites[specialNerf];
        card.penalties[specialNerf].sortingOrder = 4 + specialPenaltiesAdded;
        card.penalties[specialNerf].GetComponent<Animator>().SetTrigger("AddPenalty");
        specialPenaltiesAdded++;
    }

    private void Stats()
    {
        int firstStat = Random.Range(0, 4);
        int secondStat = Random.Range(0, 4);
        while (secondStat == firstStat)
        {
            secondStat = Random.Range(0, 4);
        }
        EditChampionStats(firstStat, 2f);
        EditChampionStats(secondStat, 1f);
    }

    private void EditChampionStats(int number, float amount)
    {
        switch (number)
        {
            case 0:
                card.healthText.GetComponent<Animator>().SetTrigger("AddPenalty");
                break;
            case 1:
                card.movementText.GetComponent<Animator>().SetTrigger("AddPenalty");
                break;
            case 2:
                card.damageText.GetComponent<Animator>().SetTrigger("AddPenalty");
                break;
            case 3:
                card.attackSpeedText.GetComponent<Animator>().SetTrigger("AddPenalty");
                break;
        }
        StartCoroutine(ReduceStats(number, amount));
    }

    private IEnumerator ReduceStats(int number, float amount)
    {
        yield return new WaitForSeconds(.75f);
        switch (number)
        {
            case 0:
                champion.Health -= amount;
                if (champion.Health <= 0)
                    champion.Health = 1;
                card.healthText.text = champion.Health.ToString();
                card.SetTextColor(card.healthText, champion.Health);
                break;
            case 1:
                champion.Movement -= amount;
                if (champion.Movement <= 0)
                    champion.Movement = 1;
                card.movementText.text = champion.Movement.ToString();
                card.SetTextColor(card.movementText, champion.Movement);
                break;
            case 2:
                champion.Damage -= amount;
                if (champion.Damage <= 0)
                    champion.Damage = 1;
                card.damageText.text = champion.Damage.ToString();
                card.SetTextColor(card.damageText, champion.Damage);
                break;
            case 3:
                champion.AttackSpeed -= amount;
                if (champion.AttackSpeed <= 0)
                    champion.AttackSpeed = 1;
                card.attackSpeedText.text = champion.AttackSpeed.ToString();
                card.SetTextColor(card.attackSpeedText, champion.AttackSpeed);
                break;
        }
    }

    public void Lock()
    {
        CanAddPenalty = false;
        card.locked.SetActive(true);
    }
}
