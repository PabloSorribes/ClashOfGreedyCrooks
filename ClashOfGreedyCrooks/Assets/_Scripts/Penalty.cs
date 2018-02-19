using UnityEngine;

public class Penalty : MonoBehaviour
{
    private Champion champion;
    private bool canAddPenalty;
    public bool[] specialPenalties;
    private int specialPenaltiesAdded;
    private int totalPenaltiesAdded;
    private CardComponent card;
    private PickingResources resources;

    private void Awake()
    {
        specialPenalties = new bool[3];
        champion = GetComponent<Champion>();
        CanAddPenalty = true;
    }

    public bool CanAddPenalty { get; set; }

    public void AddPenalty(CardComponent card, PickingResources resources)
    {
        this.card = card;
        this.resources = resources;

        int random = Random.Range(0, 3);

        if (random == 0 && specialPenaltiesAdded < 2)
            Special();
        else
            Stats();

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
                champion.Health -= amount;
                if (champion.Health <= 0)
                    champion.Health = 1;
                card.healthText.text = champion.Health.ToString();
                card.healthText.color = Color.red;
                break;
            case 1:
                champion.Movement -= amount;
                if (champion.Movement <= 0)
                    champion.Movement = 1;
                card.movementText.text = champion.Movement.ToString();
                card.movementText.color = Color.red;
                break;
            case 2:
                champion.Damage -= amount;
                if (champion.Damage <= 0)
                    champion.Damage = 1;
                card.damageText.text = champion.Damage.ToString();
                card.damageText.color = Color.red;
                break;
            case 3:
                champion.AttackSpeed -= amount;
                if (champion.AttackSpeed <= 0)
                    champion.AttackSpeed = 1;
                card.attackSpeedText.text = champion.AttackSpeed.ToString();
                card.attackSpeedText.color = Color.red;
                break;
        }
        
    }

    public void Lock()
    {
        CanAddPenalty = false;
        card.locked.SetActive(true);
    }
}
