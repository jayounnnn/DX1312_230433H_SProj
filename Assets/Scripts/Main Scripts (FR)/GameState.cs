using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jayounnnn_HeroBrew
{
    public static class GameState
    {
        // TODO: hook this to your real Main Building once implemented
        public static int HeroCastleLevel = 1;

        // how many of each building the player owns (placed)
        public static readonly Dictionary<string, int> BuildingCounts = new Dictionary<string, int>();

        public static int GetCount(string id)
        {
            return BuildingCounts.TryGetValue(id, out var c) ? c : 0;
        }

        public static void Increment(string id)
        {
            BuildingCounts[id] = GetCount(id) + 1;
        }
    }
}
