using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Accessory", menuName = "GachaSystem/Items/Accessory")]
public class Accessory : GachaItem
{
    [Header("Accessory Specific Data")]
    public float defenseBonus;
    public float speedBonus;
}
