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
        private Building _building;

        // (Optional if you’re keeping a world “bubble” on the prefab)
        [SerializeField] private GameObject collectButtonBubble;  // can be null
        [SerializeField] private TextMeshProUGUI amountTextBubble; // can be null

        public float Fill01 => Mathf.Clamp01(maxCapacity <= 0 ? 0 : currentAmount / maxCapacity);
        public int AmountInt => Mathf.FloorToInt(currentAmount);

        private void Start()
        {
            lastUpdateTime = Time.time;
            UpdateBubble();
        }

        private void Awake()
        {
            _building = GetComponent<Building>();
        }

        private void Update()
        {
            if (_building == null || !_building.Placed) return;

            float delta = Time.time - lastUpdateTime;
            lastUpdateTime = Time.time;

            currentAmount = Mathf.Min(currentAmount + delta * productionRate, maxCapacity);
            UpdateBubble();
        }

        private void UpdateBubble()
        {
            if (amountTextBubble) amountTextBubble.text = AmountInt.ToString();
            if (collectButtonBubble) collectButtonBubble.SetActive(currentAmount >= 1f);
        }

        public void Collect()
        {
            int amount = AmountInt;
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
            UpdateBubble();
        }
    }
}