namespace jayounnnn_HeroBrew
{
    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class UI_Building : MonoBehaviour
    {
        [SerializeField] private int _prefabIndex = 0;
        [SerializeField] private Button _button = null;

        [Header("Building Meta")]
        [SerializeField] private string _buildingId = "goldmine"; // set per-button in Inspector
        [SerializeField] private Text _lockLabel; // optional: drag a small TMP/Text for reason
        [SerializeField] private TextMeshProUGUI _costGoldText;   
        [SerializeField] private TextMeshProUGUI _costCrystalText;

        private void Start()
        {
            _button.onClick.AddListener(Clicked);
            RefreshButtonStateAndCost();
        }

        private void Clicked() 
        {
            // Shop -> Main
            UI_Shop.instance.SetStatus(false);
            UI_Main.instance.SetStatus(true);

            Vector3 position = Vector3.zero;

            Building building = Instantiate(UI_Main.instance._buildingPrefabs[_prefabIndex], position, Quaternion.identity);

            // Prepare dynamic cost + rules
            if (!BuildingManager.PrepareBuild(_buildingId, building, out var reason))
            {
                Debug.LogWarning($"Cannot build {_buildingId}: {reason}");
                Object.Destroy(building.gameObject);
                return;
            }

            // Optional: show cost preview in UI (before placing)
            Debug.Log($"Next {_buildingId} cost — Gold: {building.CostGold}, Crystal: {building.CostCrystal}");


            building.PlaceOnGrid(20, 20);

            Building.instance = building;
            CameraController.instance.isPlacingBuilding = true;

            UI_Build.instance.SetStatus(true);
        }

        // Call this whenever shop opens or currency/castle level changes
        public void RefreshButtonStateAndCost()
        {
            // 1) Lock checks (castle level, limit…)
            var (canByRules, reason) = BuildingCatalog.CanStartBuild(_buildingId);
            if (_lockLabel) _lockLabel.text = canByRules ? "" : reason;

            // 2) Cost (next copy)
            var (goldCost, crystalCost) = BuildingCatalog.GetNextCost(_buildingId);
            if (_costGoldText) _costGoldText.text = goldCost.ToString();
            if (_costCrystalText) _costCrystalText.text = crystalCost.ToString();

            // 3) Affordability (compare with current player wallet)
            bool canAfford = true;
            var player = FindObjectOfType<Player>();
            if (player != null)
            {
                if (player.GetGold() < goldCost) canAfford = false;
                if (player.GetCrystal() < crystalCost) canAfford = false;
            }

            // 4) Final interactable state
            _button.interactable = canByRules && canAfford;

            // (Optional) style text red if not affordable
            if (_costGoldText) _costGoldText.color = (player != null && player.GetGold() < goldCost) ? Color.red : Color.white;
            if (_costCrystalText) _costCrystalText.color = (player != null && player.GetCrystal() < crystalCost) ? Color.red : Color.white;
        }

    }
}
