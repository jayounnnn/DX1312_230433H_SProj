using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GachaDrop
{
    public GachaItem item;
    [Tooltip("The higher the weight, the more common the item.")]
    public int weight;
}

[CreateAssetMenu(fileName = "New Gacha Pool", menuName = "GachaSystem/Gacha Pool")]
public class GachaPool : ScriptableObject
{
    [Header("Gacha Pool Items")]
    [Tooltip("The list of all possible items to get from this pool and their weights.")]
    public List<GachaDrop> possibleDrops;
}
