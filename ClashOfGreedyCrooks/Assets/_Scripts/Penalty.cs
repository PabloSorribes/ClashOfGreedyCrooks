using UnityEngine;

public enum Nerf { Health, Damage, Movement, AttackSpeed }

public class Penalty : MonoBehaviour
{
    private Champion champion;
    private Transform health, movement, damage, attackSpeed;
    private GameObject penaltyButtons;

    private void Awake()
    {
        champion = GetComponent<Champion>();
        health = transform.Find("StatsHolder/Health");
        movement = transform.Find("StatsHolder/Movement");
        damage = transform.Find("StatsHolder/Damage");
        attackSpeed = transform.Find("StatsHolder/AttackSpeed");
        penaltyButtons = transform.Find("PenaltyButtons").gameObject;
        penaltyButtons.SetActive(false);
    }

    public void Buttons(bool show)
    {
        penaltyButtons.SetActive(show);
    }

    public void AddPenalty(Nerf newPenalty, int amount)
    {
        if (newPenalty == Nerf.Health)
        {
            champion.Health -= amount;
            ReduceStat(health, amount);
        }
        else if (newPenalty == Nerf.Movement)
        {
            champion.Movement -= amount;
            ReduceStat(movement, amount);
        }
        else if (newPenalty == Nerf.Damage)
        {
            champion.Damage -= amount;
            ReduceStat(damage, amount);
        }
        else if (newPenalty == Nerf.AttackSpeed)
        {
            champion.AttackSpeed -= amount;
            ReduceStat(attackSpeed, amount);
        }
    }

    private void ReduceStat(Transform stat, int amount)
    {
        for (int i = stat.childCount - 1; i > stat.childCount - (stat.childCount - amount) - 1; i--)
        {
            if (stat.GetChild(i).gameObject.activeInHierarchy)
            {
                stat.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
