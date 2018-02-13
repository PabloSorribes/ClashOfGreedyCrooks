using UnityEngine;
using UnityEngine.UI;

public class CardComponent : MonoBehaviour {

    [HideInInspector] public Character character;
    [HideInInspector] public Weapon weapon;

    [HideInInspector] public Image avatarImage;
    [HideInInspector] public RawImage portraitRawImage;
    [HideInInspector] public Text characterNameText;
    [HideInInspector] public Text weaponNameText;
    [HideInInspector] public Text healthText;
    [HideInInspector] public Text movementText;
    [HideInInspector] public Text damageText;
    [HideInInspector] public Text attackSpeedText;

    private void Start()
    {
        avatarImage = transform.Find("Canvas/Avatar/Image").GetComponent<Image>();
        portraitRawImage = transform.Find("Canvas/Portrait/RawImage").GetComponent<RawImage>();
        characterNameText = transform.Find("Canvas/NamePanel/Character/Text").GetComponent<Text>();
        weaponNameText = transform.Find("Canvas/NamePanel/Weapon/Text").GetComponent<Text>();
        healthText = transform.Find("Canvas/StatsPanel/Health/Health").GetComponent<Text>();
        movementText = transform.Find("Canvas/StatsPanel/Movement/Movement").GetComponent<Text>();
        damageText = transform.Find("Canvas/StatsPanel/Damage/Damage").GetComponent<Text>();
        attackSpeedText = transform.Find("Canvas/StatsPanel/AttackSpeed/AttackSpeed").GetComponent<Text>();
    }
}
