using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GachaSystem : MonoBehaviour
{
    [Header("Hero Pool")]
    public List<HeroSO> heroPool;

    [Header("Summon Results")]
    public HeroViewer viewer;

    // Probability weights (must total 1.0 or 100%)
    private readonly Dictionary<int, float> starProbabilities = new Dictionary<int, float>
    {
        { 5, 0.02f }, // 2% for 5 star character
        { 4, 0.10f }, // 10% for 4 star character
        { 3, 0.30f }, // 30% for 3 star character
        { 1, 0.29f }, // 29% for 1 star character
        { 2, 0.29f }, // 29% for 2 star character
    };

    public void SummonHero()
    {
        if (heroPool == null || heroPool.Count == 0)
        {
            Debug.LogWarning("Hero pool is empty.");
            return;
        }

        // Step 1: Roll a star rating based on weight
        int selectedStar = GetWeightedStar();

        // Step 2: Filter heroes by that star rating
        var filtered = heroPool.Where(h => h.hero.starRating == selectedStar).ToList();

        if (filtered.Count == 0)
        {
            Debug.LogWarning($"No heroes found for {selectedStar}★. Retrying...");
            return;
        }

        // Step 3: Randomly pick from that filtered group
        int randomIndex = Random.Range(0, filtered.Count);
        HeroSO selectedHero = filtered[randomIndex];

        Debug.Log($"[Summon] {selectedHero.hero.heroName} ({selectedHero.hero.starRating}★)");
        viewer.DisplayHero(selectedHero.hero);
    }

    private int GetWeightedStar()
    {
        float roll = Random.value;
        float cumulative = 0f;

        foreach (var pair in starProbabilities.OrderBy(p => -p.Key)) // Highest stars checked first
        {
            cumulative += pair.Value;
            if (roll <= cumulative)
                return pair.Key;
        }

        return 1; // Fallback
    }
}