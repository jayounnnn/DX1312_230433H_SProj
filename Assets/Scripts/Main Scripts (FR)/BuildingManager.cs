using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jayounnnn_HeroBrew
{
    public static class BuildingManager
    {
        // Preflight: check rules, compute dynamic cost, stamp the Building instance costs
        public static bool PrepareBuild(string buildingId, Building building, out string failReason)
        {
            var (ok, reason) = BuildingCatalog.CanStartBuild(buildingId);
            if (!ok)
            {
                failReason = reason;
                return false;
            }

            var (gold, crystal) = BuildingCatalog.GetNextCost(buildingId);
            building.SetPurchaseTerms(buildingId, gold, crystal, 0);

            failReason = "";
            return true;
        }

        // Call this after ConfirmBuild succeeds
        public static void FinalizePlacement(Building building)
        {
            GameState.Increment(building.BuildingID);
        }
    }
}
