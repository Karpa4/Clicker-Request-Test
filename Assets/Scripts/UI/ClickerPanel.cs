using Services;
using Services.Clicker;
using UI.Presenters;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace UI
{
    public class ClickerPanel : BasePanel
    {
        [SerializeField] private ClickEffectHandler _clickEffectHandler;
        
        [Inject] private IEnergyService _energyService;
        [Inject] private IUiSoundsManager _soundsManager;
        [Inject] private IGameCurrencyService _gameCurrencyService;
        
        public override void Initialize(UIDocument uiDocument)
        {
            _presenters.Add(CreateCurrencyPresenter(uiDocument));
            _presenters.Add(CreateEnergyPresenter(uiDocument));
        }

        private PresenterBase CreateCurrencyPresenter(UIDocument uiDocument)
        {
            var currencyLabel = uiDocument.rootVisualElement.Q<Label>("CurrencyLabel");
            var clickButton = uiDocument.rootVisualElement.Q<Button>("ClickButton");
            return new CurrencyPresenter(_gameCurrencyService, _clickEffectHandler, _soundsManager, currencyLabel, clickButton);
        }
        
        private PresenterBase CreateEnergyPresenter(UIDocument uiDocument)
        {
            var energyLabel = uiDocument.rootVisualElement.Q<Label>("EnergyLabel");
            return new EnergyPresenter(_energyService, energyLabel);
        }
    }
}