namespace jayounnnn_HeroBrew
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class UI_Building : MonoBehaviour
    {
        [SerializeField] private int _prefabIndex = 0;
        [SerializeField] private Button _button = null;

        private void Start()
        {
            _button.onClick.AddListener(Clicked);
        }

        private void Clicked() 
        {
            UI_Shop.instance.SetStatus(false);
            UI_Main.instance.SetStatus(true);

            Vector3 position = Vector3.zero;

            Building building = Instantiate(UI_Main.instance._buildingPrefabs[_prefabIndex], position, Quaternion.identity);

            building.BuildingID = "goldmine"; 
            building.CostGold = 300;
            building.CostCrystal = 0;
            building.CostStamina = 0;

            building.PlaceOnGrid(20, 20);

            Building.instance = building;
            CameraController.instance.isPlacingBuilding = true;

            UI_Build.instance.SetStatus(true);
        }

    }
}
