using Services.Clicker;
using UnityEngine.UIElements;

namespace UI.Presenters
{
    public class EnergyPresenter : PresenterBase
    {
        private readonly IEnergyService _energyService;
        private readonly Label _energyLabel;

        public EnergyPresenter(IEnergyService energyService, Label energyLabel)
        {
            _energyService = energyService;
            _energyLabel = energyLabel;
        }

        public override void Start()
        {
            UpdateValue();
            _energyService.EnergyChangedEvent += UpdateValue;
        }

        public override void Stop()
        {
            _energyService.EnergyChangedEvent -= UpdateValue;
        }
        
        private void UpdateValue()
        {
            _energyLabel.text = _energyService.CurrentEnergy.ToString();
        }
    }
}