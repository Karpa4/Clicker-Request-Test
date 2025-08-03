using Services;
using UI.Presenters;
using UnityEngine.UIElements;
using Zenject;

namespace UI
{
    public class WeatherPanel : BasePanel
    {
        [Inject] private IWeatherUpdatingService _weatherUpdatingService;
        
        public override void Initialize(UIDocument uiDocument)
        {
            _presenters.Add(CreateWeatherPresenter(uiDocument));
        }

        private PresenterBase CreateWeatherPresenter(UIDocument uiDocument)
        {
            var weatherLabel = uiDocument.rootVisualElement.Q<Label>("WeatherLabel");
            var weatherIcon = uiDocument.rootVisualElement.Q<VisualElement>("WeatherIcon");
            return new WeatherPresenter(_weatherUpdatingService, weatherIcon, weatherLabel);
        }
    }
}
