using UnityEngine;

/// <summary>
/// Keeps stats for every champion.
/// </summary>
public class Champion : MonoBehaviour {

    private Penalty penalty;
    
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
    public float Health
    {
        get
        {
            return health;
        }

        set
        {
            health += value;
        }
    }
    public float Movement
    {
        get
        {
            return movement;
        }

        set
        {
            movement += value;
        }
    }
    public float Damage
    {
        get
        {
            return damage;
        }

        set
        {
            damage += value;
        }
    }
    public float AttackSpeed
    {
        get
        {
            return attackSpeed;
        }

        set
        {
            attackSpeed += value;
        }
    }

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
    
    private void Start()
    {
        penalty = GetComponent<Penalty>();

        Health = (int)Random.Range(healthMin, healthMax + 1);
        Movement = (int)Random.Range(movementMin, movementMax + 1);
        Damage = (int)Random.Range(damageMin, damageMax + 1);
        AttackSpeed = (int)Random.Range(attackSpeedMin, attackSpeedMax + 1);

        UpdateStartingStatsUI();

        //Debug.Log("Health: " + Health);
        //Debug.Log("Damage: " + Damage);
        //Debug.Log("Movement: " + Movement);
        //Debug.Log("AttackSpeed: " + AttackSpeed);
    }

    private void UpdateStartingStatsUI()
    {
        penalty.SetStartingStats(Nerf.Health, 10 - (int)Health);
        penalty.SetStartingStats(Nerf.Movement, 10 - (int)Movement);
        penalty.SetStartingStats(Nerf.Damage, 10 - (int)Damage);
        penalty.SetStartingStats(Nerf.AttackSpeed, 10 - (int)AttackSpeed);
    }
}
