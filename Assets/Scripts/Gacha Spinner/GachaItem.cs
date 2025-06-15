using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GachaItem : ScriptableObject
{
    [Header("Common Item Data")]
    public string itemName;
    public Sprite itemIcon;
    public string itemDescription;
    [Range(1, 5)] // Ensures star rating is between 1 and 5 in the editor
    public int starRating;
}
