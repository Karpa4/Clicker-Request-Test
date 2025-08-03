using System;
using Configuration;
using UnityEngine;
using Zenject;

namespace Services.Clicker
{
    public interface IGameCurrencyService
    {
        int CurrentCurrency { get; }
    
        event Action<int> CurrencyChangedEvent;
        event Action AutoPickEvent;
    
        void Start();
        void Stop();
        void HandleClick();
    }

    public class GameCurrencyService : IUpdatable, IGameCurrencyService
    {
        private int _currentCurrency;
        private float _lastAutoPickTime;

        [Inject] private IMonoUpdatingService _monoUpdatingService;
        [Inject] private IClickerGameSettings _clickerGameSettings;
        [Inject] private IEnergyService _energyService;

        public int CurrentCurrency => _currentCurrency;
    
        public event Action<int> CurrencyChangedEvent;
        public event Action AutoPickEvent;

        public void Start()
        {
            _currentCurrency = _clickerGameSettings.StartCurrencyCount;
            _lastAutoPickTime = Time.time;
            _monoUpdatingService.Add(this);
        }
    
        public void Stop()
        {
            _monoUpdatingService.Remove(this);
        }

        public void HandleClick()
        {
            if (!_energyService.TrySpendEnergy(_clickerGameSettings.ClickCost))
                return;

            AddCurrency(_clickerGameSettings.ClickReward);
        }

        public void OnUpdate(float _)
        {
            if (Time.time - _lastAutoPickTime < _clickerGameSettings.TimeBetweenAutoPick)
                return;
        
            _lastAutoPickTime = Time.time;
        
            if (!_energyService.TrySpendEnergy(_clickerGameSettings.AutoPickCost))
                return;
        
            AddCurrency(_clickerGameSettings.AutoPickReward);
            AutoPickEvent?.Invoke();
        }
    
        private void AddCurrency(int newCurrency)
        {
            _currentCurrency += newCurrency;
            CurrencyChangedEvent?.Invoke(newCurrency);
        }
    }
}