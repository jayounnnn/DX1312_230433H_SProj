using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace jayounnnn_HeroBrew
{
    public class UI_Production : MonoBehaviour
    {
        public static UI_Production instance { get; private set; }

        [Header("Panel")]
        [SerializeField] private GameObject panel;

        [Header("Widgets")]
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] private Slider progress;
        [SerializeField] private Button collectButton;
        [SerializeField] private Button closeButton;

        private BuildingProduction target;

        private void Awake()
        {
            instance = this;
            panel.SetActive(false);
        }

        private void Start()
        {
            collectButton.onClick.AddListener(Collect);
            closeButton.onClick.AddListener(Hide);
        }

        private void Update()
        {
            if (!panel.activeSelf || target == null) return;

            amountText.text = $"Stored: {target.AmountInt} / {target.maxCapacity}";
            progress.value = target.Fill01;
            collectButton.interactable = target.AmountInt > 0;
        }

        public void ShowFor(BuildingProduction prod, string displayName)
        {
            target = prod;
            if (titleText) titleText.text = displayName;
            panel.SetActive(true);
        }

        public void Hide()
        {
            target = null;
            panel.SetActive(false);
        }

        private void Collect()
        {
            if (target == null) return;
            target.Collect();
            UI_Main.instance.UpdateCurrencyUI();
        }
    }
}