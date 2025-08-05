using UnityEngine;
using TMPro;

namespace jayounnnn_HeroBrew
{
    public class BuildingProduction : MonoBehaviour
    {
        public enum ResourceType { Gold, Crystal, Stamina }
        public ResourceType type = ResourceType.Gold;

        public int maxCapacity = 500;
        public float productionRate = 1f; // units per second
        public float currentAmount = 0f;

        private float lastUpdateTime;

        [SerializeField] private GameObject collectButton;
        [SerializeField] private TextMeshProUGUI amountText;

        private void Start()
        {
            lastUpdateTime = Time.time;
            UpdateDisplay();
        }

        private void Update()
        {
            if (Building.instance != this.GetComponent<Building>() || !Building.instance.Placed)
                return;

            float delta = Time.time - lastUpdateTime;
            lastUpdateTime = Time.time;

            currentAmount += delta * productionRate;
            currentAmount = Mathf.Min(currentAmount, maxCapacity);

            UpdateDisplay();

            if (collectButton != null)
                collectButton.SetActive(currentAmount >= 1f);
        }

        private void UpdateDisplay()
        {
            if (amountText != null)
                amountText.text = Mathf.FloorToInt(currentAmount).ToString();
        }

        public void Collect()
        {
            int amount = Mathf.FloorToInt(currentAmount);
            if (amount <= 0) return;

            var player = FindObjectOfType<Player>();
            if (player != null)
            {
                switch (type)
                {
                    case ResourceType.Gold: player.AddGold(amount); break;
                    case ResourceType.Crystal: player.AddCrystal(amount); break;
                    case ResourceType.Stamina: player.AddStamina(amount); break;
                }
            }

            currentAmount = 0;
            UpdateDisplay();
            if (collectButton != null)
                collectButton.SetActive(false);
        }
    }
}