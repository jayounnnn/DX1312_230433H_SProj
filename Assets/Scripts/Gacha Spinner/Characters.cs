using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "GachaSystem/Items/Character")]
public class Character : GachaItem
{
    [Header("Character Specific Data")]
    public int attackPower;
    public int health;
    public string skillName;
}



