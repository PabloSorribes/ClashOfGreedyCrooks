using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class Character : ScriptableObject
{
    public new string name;
    public string pathToModel = "Champions/";
    public int healthMin;
    public int healthMax;
    public int movementMin;
    public int movementMax;
}
