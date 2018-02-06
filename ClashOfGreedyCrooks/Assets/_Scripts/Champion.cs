using UnityEngine;

public class Champion : MonoBehaviour {

    //Variables
    private float health;
    private float movement;
    private float damage;
    private float attackSpeed;

    private int playerIndex;
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

    public int PlayerIndex { get; set; }
    public bool Picked { get; set; }
    public bool Locked { get; set; }

    //Starting values
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
        Health = Random.Range(healthMin, healthMax);
        Movement = Random.Range(movementMin, movementMax);
        Damage = Random.Range(damageMin, damageMax);
        AttackSpeed = Random.Range(attackSpeedMin, attackSpeedMax);
    }
}
