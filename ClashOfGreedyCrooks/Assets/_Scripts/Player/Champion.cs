using UnityEngine;

/// <summary>
/// Keeps stats for every champion.
/// </summary>
public class Champion : MonoBehaviour {

    private Penalty penalty;

    public new string name;

    //Variables
    private float health;
    private float movement;
    private float damage;
    private float attackSpeed;

    private int playerIndex;
    private int lastPlayerIndex;
    private bool picked;
    private bool locked;

    //Properties
    public float Health { get; set; }
    public float Movement { get; set; }
    public float Damage { get; set; }
    public float AttackSpeed { get; set; }

    public int PlayerIndex
    {
        get
        {
            return playerIndex;
        }
        set
        {
            LastPlayerIndex = PlayerIndex;
            playerIndex = value;
        }
    }
    public int LastPlayerIndex { get; set; }
    public bool Picked { get; set; }
    public bool Locked { get; set; }

    //Starting values (set in inspector)
    [SerializeField] private float healthMin;
    [SerializeField] private float healthMax;
    [SerializeField] private float movementMin;
    [SerializeField] private float movementMax;
    [SerializeField] private float damageMin;
    [SerializeField] private float damageMax;
    [SerializeField] private float attackSpeedMin;
    [SerializeField] private float attackSpeedMax;

    public void SetStatingStats()
    {
        Health = (int)Random.Range(healthMin, healthMax + 1);
        Movement = (int)Random.Range(movementMin, movementMax + 1);
        Damage = (int)Random.Range(damageMin, damageMax + 1);
        AttackSpeed = (int)Random.Range(attackSpeedMin, attackSpeedMax + 1);
    }
}
