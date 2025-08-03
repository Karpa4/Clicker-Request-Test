using UnityEngine;

namespace Configuration
{
    public interface IClickerGameSettings
    {
        int StartCurrencyCount { get; }
        int ClickReward { get; }
        int ClickCost { get; }
        int AutoPickCost { get; }
        int AutoPickReward { get; }
        float TimeBetweenAutoPick { get; }
        int MaxEnergyCount { get; }
        int EnergyIncome { get; }
        float TimeBetweenEnergyIncome { get; }
    }

    [CreateAssetMenu(fileName = @"ClickerSettings", menuName = @"Cifkor/Configurations/Clicker Settings", order = 1)]
    public class ClickerGameSettings : ScriptableObject, IClickerGameSettings
    {
        [SerializeField] private int _startCurrencyCount = 0;
        [Header("Click")]
        [SerializeField] private int _clickReward = 1;
        [SerializeField] private int _clickCost = 1;
        [Header("AutoPick")]
        [SerializeField] private int _autoPickCost = 1;
        [SerializeField] private int _autoPickReward = 1;
        [SerializeField] private float _timeBetweenAutoPick = 3;
        [Header("Energy")]
        [SerializeField] private int _maxEnergyCount = 1000;
        [SerializeField] private int _energyIncome = 10;
        [SerializeField] private float _timeBetweenEnergyIncome = 10;

        public int StartCurrencyCount => _startCurrencyCount;
        public int ClickReward => _clickReward;
        public int ClickCost => _clickCost;
        public int AutoPickCost => _autoPickCost;
        public int AutoPickReward => _autoPickReward;
        public float TimeBetweenAutoPick => _timeBetweenAutoPick;
        public int MaxEnergyCount => _maxEnergyCount;
        public int EnergyIncome => _energyIncome;
        public float TimeBetweenEnergyIncome => _timeBetweenEnergyIncome;
    }
}