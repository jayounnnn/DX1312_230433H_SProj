namespace jayounnnn_HeroBrew
{
    using System;

    public static class Data
    {
        public static readonly int maxStamina = 60;
        public static readonly int defaultGold = 5000;
        public static readonly int defaultCrystal = 300;
        public static readonly int defaultStamina = 30;

        public static readonly int staminaRechargeMinutes = 5;
        public static readonly int staminaPerBattle = 10;

        [Serializable]
        public class Player
        {
            public int gold = defaultGold;
            public int crystal = defaultCrystal;
            public int stamina = defaultStamina;

            public string name = "Relic Brewer";
            public int level = 1;
            public int exp = 0;

            // Add more if needed later (e.g., inventory, owned heroes)
        }

        public static T Clone<T>(this T source)
        {
            return UnityEngine.JsonUtility.FromJson<T>(UnityEngine.JsonUtility.ToJson(source));
        }
    }
}