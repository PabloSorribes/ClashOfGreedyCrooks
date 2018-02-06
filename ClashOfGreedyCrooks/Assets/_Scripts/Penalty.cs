using UnityEngine;

public enum Nerf { Health, Movement, Damage, AttackSpeed }

public class Penalty : MonoBehaviour {

    private Champion champion;
    private Transform health, movement, damage, attackSpeed;
    private GameObject penaltyButtons;

    private void Start()
    {
        champion = GetComponent<Champion>();
        health = transform.Find("StatsHolder/Health");
        movement = transform.Find("StatsHolder/Movement");
        damage = transform.Find("StatsHolder/Damage");
        attackSpeed = transform.Find("StatsHolder/AttackSpeed");
        penaltyButtons = transform.Find("PenaltyButtons").gameObject;
        penaltyButtons.SetActive(false);
    }

    public void ShowButtons()
    {
        penaltyButtons.SetActive(true);
    }

    public void AddPenalty(Nerf newPenalty, int amount)
    {
        if (newPenalty == Nerf.Health)
        {
            champion.Health -= amount;
            ReduceStat(health, amount);
        }
    }

    private void ReduceStat(Transform stat, int amount)
    {
        int penalties = amount;
        for (int i = stat.childCount - 1; i >= 0; i--)
        {
            if (health.GetChild(i).gameObject.activeInHierarchy && penalties > 0)
            {
                health.GetChild(i).gameObject.SetActive(false);
                penalties--;
            }
        }
    }
}
