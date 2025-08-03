using Configuration;
using Services;
using Services.Clicker;
using UI;
using UnityEngine;
using Zenject;

public class MainSceneInstaller : MonoInstaller
{
    [SerializeField] private ClickerGameSettings _clickerGameSettings;
    [SerializeField] private WeatherSettings _weatherSettings;
    [SerializeField] private DogSettings _dogSettings;
    [SerializeField] private MonoUpdatingService _monoUpdatingService;
    [SerializeField] private PopupService _popupService;
    [SerializeField] private ViewManager _viewManager;

    public override void InstallBindings()
    {
        Container.Bind<IClickerGameSettings>().To<ClickerGameSettings>().FromInstance(_clickerGameSettings);
        Container.Bind<IWeatherSettings>().To<WeatherSettings>().FromInstance(_weatherSettings);
        Container.Bind<IDogSettings>().To<DogSettings>().FromInstance(_dogSettings);
        
        Container.Bind<IMonoUpdatingService>().To<MonoUpdatingService>().FromInstance(_monoUpdatingService);
        Container.Bind<IRequestSender>().To<QueueRequestSender>().AsSingle();
        Container.Bind<IViewManager>().To<ViewManager>().FromInstance(_viewManager);
        Container.Bind<IPopupService>().To<PopupService>().FromInstance(_popupService);
        Container.Bind<IGameCurrencyService>().To<GameCurrencyService>().AsSingle();
        Container.Bind<IEnergyService>().To<EnergyService>().AsSingle();
        Container.Bind<IWeatherUpdatingService>().To<WeatherUpdatingService>().AsSingle();
        Container.Bind<IBreedInfoProvider>().To<BreedInfoProvider>().AsSingle();
    }
}
