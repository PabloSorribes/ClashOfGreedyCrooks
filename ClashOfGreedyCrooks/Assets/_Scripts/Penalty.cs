using UnityEngine;

public enum Nerf { Health, Movement, Damage, AttackSpeed }

public class Penalty : MonoBehaviour
{
    private Champion champion;
    private Transform[] stats = new Transform[4];
    private GameObject penaltyButtons;

    private void Awake()
    {
        champion = GetComponent<Champion>();
        stats[0] = transform.Find("StatsHolder/Health");
        stats[1] = transform.Find("StatsHolder/Movement");
        stats[2] = transform.Find("StatsHolder/Damage");
        stats[3] = transform.Find("StatsHolder/AttackSpeed");
        penaltyButtons = transform.Find("PenaltyButtons").gameObject;
        penaltyButtons.SetActive(false);
    }

    public void Buttons(bool show)
    {
        penaltyButtons.SetActive(show);
    }

    public void SetStartingStats(Nerf stat, int amount)
    {
        ReduceStat(stats[stat.GetHashCode()], amount);
    }

    public void AddPenalty(Nerf newPenalty, int amount)
    {
        amount = amount * -1;
        if (newPenalty == Nerf.Health)
            champion.Health = (float)amount;
        else if (newPenalty == Nerf.Movement)
            champion.Movement = (float)amount;
        else if (newPenalty == Nerf.Damage)
            champion.Damage = (float)amount;
        else if (newPenalty == Nerf.AttackSpeed)
            champion.AttackSpeed = (float)amount;

        ReduceStat(stats[newPenalty.GetHashCode()], Mathf.Abs(amount));
    }

    private void ReduceStat(Transform stat, int amount)
    {
        int penalties = amount;
        for (int i = stat.childCount - 1; i >= 0; i--)
        {
            if (penalties == 0)
                return;
            if (stat.GetChild(i).gameObject.activeInHierarchy)
            {
                stat.GetChild(i).gameObject.SetActive(false);
                penalties--;
            }
        }
    }
}
