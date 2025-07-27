namespace jayounnnn_HeroBrew
{
    using UnityEngine;

    public class Player : MonoBehaviour
    {
        private Data.Player _playerData;

        public int GetGold() => _playerData.gold;
        public int GetCrystal() => _playerData.crystal;
        public int GetStamina() => _playerData.stamina;

        private void Start()
        {
            // Simulate data load
            _playerData = new Data.Player();

            UpdateUI();
        }

        private void UpdateUI()
        {
            UI_Main.instance._staminaText.text = _playerData.stamina.ToString();
            UI_Main.instance._goldText.text = _playerData.gold.ToString();
            UI_Main.instance._crystalText.text = _playerData.crystal.ToString();
        }

        public bool SpendGold(int amount)
        {
            if (_playerData.gold >= amount)
            {
                _playerData.gold -= amount;
                UpdateUI();
                return true;
            }
            return false;
        }

        public bool SpendCrystal(int amount)
        {
            if (_playerData.crystal >= amount)
            {
                _playerData.crystal -= amount;
                UpdateUI();
                return true;
            }
            return false;
        }

        public bool SpendStamina(int amount)
        {
            if (_playerData.stamina >= amount)
            {
                _playerData.stamina -= amount;
                UpdateUI();
                return true;
            }
            return false;
        }

        public void AddGold(int amount)
        {
            _playerData.gold += amount;
            UpdateUI();
        }

        public void AddCrystal(int amount)
        {
            _playerData.crystal += amount;
            UpdateUI();
        }

        public void AddStamina(int amount)
        {
            _playerData.stamina = Mathf.Min(_playerData.stamina + amount, Data.maxStamina);
            UpdateUI();
        }
    }


}


