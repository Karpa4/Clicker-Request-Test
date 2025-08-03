using Services.Clicker;
using UnityEngine.UIElements;

namespace UI.Presenters
{
    public class CurrencyPresenter : PresenterBase
    {
        private readonly IGameCurrencyService _gameCurrencyService;
        private readonly ClickEffectHandler _clickEffectHandler;
        private readonly Label _currencyLabel;
        private readonly Button _clickButton;

        public CurrencyPresenter(IGameCurrencyService gameCurrencyService, ClickEffectHandler clickEffectHandler, Label currencyLabel, Button clickButton)
        {
            _gameCurrencyService = gameCurrencyService;
            _clickEffectHandler = clickEffectHandler;
            _currencyLabel = currencyLabel;
            _clickButton = clickButton;
        }

        public override void Start()
        {
            _clickEffectHandler.Initialize();
            _currencyLabel.text = _gameCurrencyService.CurrentCurrency.ToString();
            _clickButton.clicked += OnClicked;
            _gameCurrencyService.CurrencyChangedEvent += UpdateUI;
        }
        
        public override void Stop()
        {
            _clickButton.clicked -= OnClicked;
            _gameCurrencyService.CurrencyChangedEvent -= UpdateUI;
        }

        private void OnClicked()
        {
            _gameCurrencyService.HandleClick();
        }
        
        private void UpdateUI(int delta)
        {
            _clickEffectHandler.ShowUIEffects(delta);
            _currencyLabel.text = _gameCurrencyService.CurrentCurrency.ToString();
        }
    }
}