using Configuration;
using Services;
using Services.Clicker;
using UI;
using UnityEngine;
using Zenject;

public class MainSceneInstaller : MonoInstaller
{
    [SerializeField] private ClickerGameSettings _clickerGameSettings;
    [SerializeField] private UiSoundManagerConfiguration _soundManagerConfiguration;
    [SerializeField] private WeatherSettings _weatherSettings;
    [SerializeField] private DogSettings _dogSettings;

    public override void InstallBindings()
    {
        Container.Bind<IClickerGameSettings>().To<ClickerGameSettings>().FromInstance(_clickerGameSettings);
        Container.Bind<IWeatherSettings>().To<WeatherSettings>().FromInstance(_weatherSettings);
        Container.Bind<IDogSettings>().To<DogSettings>().FromInstance(_dogSettings);
        Container.Bind<IUiSoundManagerConfiguration>().To<UiSoundManagerConfiguration>().FromInstance(_soundManagerConfiguration);

        Container.Bind<IMonoUpdatingService>().To<MonoUpdatingService>().FromComponentInHierarchy().AsSingle();
        Container.Bind<IRequestSender>().To<QueueRequestSender>().AsSingle();
        Container.Bind<IUiSoundsManager>().To<UiSoundsManager>().AsSingle();
        Container.Bind<IViewManager>().To<ViewManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<IPopupService>().To<PopupService>().FromComponentInHierarchy().AsSingle();
        Container.Bind<IGameCurrencyService>().To<GameCurrencyService>().AsSingle();
        Container.Bind<IEnergyService>().To<EnergyService>().AsSingle();
        Container.Bind<IWeatherUpdatingService>().To<WeatherUpdatingService>().AsSingle();
        Container.Bind<IBreedInfoProvider>().To<BreedInfoProvider>().AsSingle();
    }
}
