using UnityEngine;
using UnityEngine.UI;

public class CardComponent : MonoBehaviour {

    [HideInInspector] public Champion champion;
    [HideInInspector] public Weapon weapon;

    [HideInInspector] public SpriteRenderer background;
    [HideInInspector] public RawImage portraitRawImage;
    [HideInInspector] public SpriteRenderer frame;
    [HideInInspector] public SpriteRenderer avatarColor;
    [HideInInspector] public SpriteRenderer avatarSymbol;
    [HideInInspector] public SpriteRenderer xboxButton;
    [HideInInspector] public SpriteRenderer[] penalties;
    [HideInInspector] public Text healthText;
    [HideInInspector] public Text movementText;
    [HideInInspector] public Text damageText;
    [HideInInspector] public Text attackSpeedText;
    [HideInInspector] public GameObject locked;

    private bool pickingCooldown;

    private void Awake()
    {
        background = transform.Find("Background").GetComponent<SpriteRenderer>();
        portraitRawImage = transform.Find("PortraitCanvas/Portrait/RawImage").GetComponent<RawImage>();
        frame = transform.Find("Frame").GetComponent<SpriteRenderer>();
        avatarColor = transform.Find("Avatar/Color").GetComponent<SpriteRenderer>();
        avatarSymbol = transform.Find("Avatar/Symbol").GetComponent<SpriteRenderer>();
        xboxButton = transform.Find("XboxButton").GetComponent<SpriteRenderer>();
        Transform penaltiesParen = transform.Find("Penalties").transform;
        penalties = new SpriteRenderer[penaltiesParen.childCount];
        for (int i = 0; i < penaltiesParen.childCount; i++)
            penalties[i] = penaltiesParen.GetChild(i).GetComponent<SpriteRenderer>();
        healthText = transform.Find("StatsCanvas/Health").GetComponent<Text>();
        movementText = transform.Find("StatsCanvas/Movement").GetComponent<Text>();
        damageText = transform.Find("StatsCanvas/Damage").GetComponent<Text>();
        attackSpeedText = transform.Find("StatsCanvas/AttackSpeed").GetComponent<Text>();
        locked = transform.Find("LockedCanvas").gameObject;
        locked.SetActive(false);
    }

    public bool CanPick()
    {
        if (pickingCooldown)
            return false;
        else
        {
            pickingCooldown = true;
            Invoke("ResetPickingCooldown", .2f);
            return true;
        }
    }

    private void ResetPickingCooldown()
    {
        pickingCooldown = false;
    }

    public void SetTextColor(Text text, int value)
    {
        if (value > 7)
            text.color = Color.green;
        else if (value > 4 && value < 8)
            text.color = Color.yellow;
        else if (value < 4)
            text.color = Color.red;
    }
}
