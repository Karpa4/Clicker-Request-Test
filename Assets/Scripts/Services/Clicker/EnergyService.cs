using System;
using Configuration;
using UnityEngine;
using Zenject;

namespace Services.Clicker
{
    public interface IEnergyService
    {
        int CurrentEnergy { get; }
    
        event Action EnergyChangedEvent;
    
        void Start();
        void Stop();
        bool TrySpendEnergy(int energyCount);
    }

    public class EnergyService : IUpdatable, IEnergyService
    {
        private int _currentEnergy;
        private float _lastEnergyIncomeTime;
    
        [Inject] private IClickerGameSettings _clickerGameSettings;
        [Inject] private IMonoUpdatingService _monoUpdatingService;

        public int CurrentEnergy => _currentEnergy;
    
        public event Action EnergyChangedEvent;

        public void Start()
        {
            _currentEnergy = _clickerGameSettings.MaxEnergyCount;
            _lastEnergyIncomeTime = Time.time;
            _monoUpdatingService.Add(this);
        }

        public bool TrySpendEnergy(int energyCount)
        {
            var energyAfterChange = _currentEnergy - energyCount;
        
            if (energyAfterChange < 0)
                return false;
        
            _currentEnergy = energyAfterChange;
            EnergyChangedEvent?.Invoke();
            return true;
        }

        public void Stop()
        {
            _monoUpdatingService.Remove(this);
        }

        public void OnUpdate(float _)
        {
            if (Time.time - _lastEnergyIncomeTime < _clickerGameSettings.TimeBetweenEnergyIncome)
                return;
        
            if (_currentEnergy >= _clickerGameSettings.MaxEnergyCount)
                return;
        
            _lastEnergyIncomeTime = Time.time;
            _currentEnergy = Math.Clamp(_currentEnergy + _clickerGameSettings.EnergyIncome, 0, _clickerGameSettings.MaxEnergyCount);
            EnergyChangedEvent?.Invoke();
        }
    }
}