using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pet", menuName = "GachaSystem/Items/Pet")]
public class Pet : GachaItem
{
    [Header("Pet Specific Data")]
    public string specialAbility;
    public float cooldown;
}
