namespace jayounnnn_HeroBrew
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using TMPro;
    using UnityEngine.UI;
    using JetBrains.Annotations;

    public class UI_Main : MonoBehaviour
    {
        [SerializeField] public GameObject _elements = null;
        [SerializeField] public TextMeshProUGUI _staminaText = null;
        [SerializeField] public TextMeshProUGUI _goldText = null;
        [SerializeField] public TextMeshProUGUI _crystalText = null;
        [SerializeField] private Button _shopButton = null;

        [SerializeField] public BuildGrid _grid = null;
        [SerializeField] public Building[] _buildingPrefabs = null;
        private static UI_Main _instance = null; public static UI_Main instance { get { return _instance; } }

        private bool _active = true; public bool isActive { get { return _active; } }

        private void Awake()
        {
            _instance = this;
            _elements.SetActive(true); 
        }

        private void Start()
        {
            _shopButton.onClick.AddListener(ShopButtonClicked);

            UpdateCurrencyUI();
        }

        public void UpdateCurrencyUI()
        {
            var player = FindObjectOfType<Player>();
            if (player != null)
            {
                _staminaText.text = player.GetStamina().ToString();
                _goldText.text = player.GetGold().ToString();
                _crystalText.text = player.GetCrystal().ToString();
            }
        }

        private void ShopButtonClicked()
        {
            UI_Build.instance.CancelBuild();
            UI_Shop.instance.SetStatus(true); 
            _elements.SetActive(false);
        }

        public void SetStatus(bool status)
        {
            _active = status;
            _elements.SetActive(status);
        }

       
    }
}