using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeroViewer : MonoBehaviour
{
    public HeroSO heroSO;

    [Header("UI References")]
    public Image portraitImage;
    public TMP_Text nameText;
    public TMP_Text roleText;
    public TMP_Text elementText;
    public TMP_Text starRatingText;
    public TMP_Text passiveSkillText;
    public TMP_Text activeSkillText;

    void Start()
    {
        if (heroSO != null)
        {
            DisplayHero(heroSO.hero);
        }
    }

    public void DisplayHero(HeroData hero)
    {
        portraitImage.sprite = hero.heroPortrait;
        nameText.text = hero.heroName;
        roleText.text = hero.role.ToString();
        elementText.text = hero.element.ToString();
        starRatingText.text = $"{hero.starRating}";
        passiveSkillText.text = hero.passiveSkillDescription;
        activeSkillText.text = hero.activeSkillDescription;
    }
}