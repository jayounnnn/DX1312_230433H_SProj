using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jayounnnn_HeroBrew
{
    // Rules per building type (cost curve, unlocks, limits)
    public static class BuildingCatalog
    {
        public class Rule
        {
            public string Id;
            public int MaxCount;                         // hard cap (e.g., Gold Mine = 2)
            public int RequiredCastleLevelForFirst = 1; // min Hero Castle level to build at least 1
            public int RequiredCastleLevelForMore = 3;  // min Hero Castle level to exceed initial cap

            // cost curve by number already owned:
            // e.g., costs[0] is the cost for the 1st placement, costs[1] for 2nd, etc.
            public List<(int gold, int crystal)> Costs = new List<(int gold, int crystal)>();
        }

        // Define your building rules here
        private static readonly Dictionary<string, Rule> _rules = new Dictionary<string, Rule>
        {
            {
                "goldmine",
                new Rule {
                    Id = "goldmine",
                    MaxCount = 4,                    // limit to two for now
                    RequiredCastleLevelForFirst = 1, // can build 1–2 at CL1
                    RequiredCastleLevelForMore = 3,  // >2 needs CL3 (future)
                    Costs = new List<(int,int)>
                    {
                        (300, 0),     // 1st gold mine
                        (600, 0),     // 2nd gold mine
                        (900, 0),     // 3rd+ (won’t be allowed until CL3, but defined anyway)
                    }
                }
            },
            {
                "crystalmine",
                new Rule {
                    Id = "crystalmine",
                    MaxCount = 2,                  
                    RequiredCastleLevelForFirst = 1, // lock crystalmine until Hero Castle ≥ 2 (adjust as you like)
                    RequiredCastleLevelForMore = 3,  // after first, future ones require CL3
                    Costs = new List<(int,int)>
                    {
                        (3000, 250), // 1st crystal mine
                        (4500, 400), // 2nd crystal mine has different (higher) costs
                        (6000, 600), // 3rd crystal mine
                    }
                }
            }
        };

        public static bool TryGetRule(string id, out Rule rule) => _rules.TryGetValue(id, out rule);

        public static (bool canBuild, string reason) CanStartBuild(string id)
        {
            if (!TryGetRule(id, out var rule)) return (true, "");

            int owned = GameState.GetCount(id);

            // unlock gate for first one
            if (owned == 0 && GameState.HeroCastleLevel < rule.RequiredCastleLevelForFirst)
                return (false, $"Requires Hero Castle Lv{rule.RequiredCastleLevelForFirst}");

            // additional copies gate
            if (owned >= 1 && GameState.HeroCastleLevel < rule.RequiredCastleLevelForMore)
                return (false, $"Requires Hero Castle Lv{rule.RequiredCastleLevelForMore} to build more");

            // hard cap gate
            if (owned >= rule.MaxCount)
                return (false, $"Limit reached ({rule.MaxCount})");

            return (true, "");
        }

        public static (int gold, int crystal) GetNextCost(string id)
        {
            if (!TryGetRule(id, out var rule)) return (0, 0);

            int owned = GameState.GetCount(id);
            if (owned < rule.Costs.Count) return rule.Costs[owned];

            // if beyond defined curve, repeat last step or ramp
            var last = rule.Costs[rule.Costs.Count - 1];
            return (last.gold + 500 * (owned - (rule.Costs.Count - 1)), last.crystal + 100 * (owned - (rule.Costs.Count - 1)));
        }
    }
}
