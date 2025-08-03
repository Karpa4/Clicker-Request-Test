using Services;
using Services.Clicker;
using UI;
using UnityEngine;
using Zenject;

public class ApplicationStart : MonoBehaviour
{
    [Inject] private IMonoUpdatingService _monoUpdatingService;
    [Inject] private IEnergyService _energyService;
    [Inject] private IGameCurrencyService _gameCurrencyService;
    [Inject] private IViewManager _viewManager;
    [Inject] private IPopupService _popupService;

    private void Awake()
    {
        _monoUpdatingService.Start();
        _energyService.Start();
        _gameCurrencyService.Start();
        _popupService.Initialize();
        _viewManager.Initialize();
    }
}
