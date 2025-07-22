using UnityEngine;

[System.Serializable]
public class HeroData
{
    public string heroName;
    public HeroRole role;
    public HeroElement element;
    public int starRating; // 1 to 5
    public Sprite heroPortrait; // For UI display
    public GameObject heroModelPrefab; // For 2.5D display

    public int baseHP;
    public int baseATK;
    public int baseDEF;

    public string passiveSkillDescription;
    public string activeSkillDescription;
}

public enum HeroRole
{
    Tank,
    DPS,
    Support,
    Control
}

public enum HeroElement
{
    Fire,
    Water,
    Earth,
    Light,
    Dark
}