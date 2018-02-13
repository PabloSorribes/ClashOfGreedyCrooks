using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject {

    public new string name;
    public string pathToModel = "Weapons/";
    public int damageMin;
    public int damageMax;
    public int attackSpeedMin;
    public int attackSpeedMax;
    public int range;
    public int projectileSpeed;
    public int projectileSpread;
}
