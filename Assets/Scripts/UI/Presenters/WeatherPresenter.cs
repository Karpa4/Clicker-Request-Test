using Services;
using UnityEngine.UIElements;

namespace UI.Presenters
{
    public class WeatherPresenter : PresenterBase
    {
        private readonly IWeatherUpdatingService _weatherUpdatingService;
        private readonly VisualElement _weatherIcon;
        private readonly Label _weatherLabel;

        public WeatherPresenter(IWeatherUpdatingService weatherUpdatingService, VisualElement weatherIcon, Label weatherLabel)
        {
            _weatherUpdatingService = weatherUpdatingService;
            _weatherIcon = weatherIcon;
            _weatherLabel = weatherLabel;
        }

        public override void Start()
        {
            _weatherUpdatingService.Start();
            _weatherUpdatingService.NewWeatherUpdateEvent += OnWeatherUpdated;
        }

        private void OnWeatherUpdated(WeatherUpdateResult weatherUpdateResult)
        {
            _weatherIcon.style.backgroundImage = new StyleBackground(weatherUpdateResult.Texture);
            _weatherLabel.text = $"Сегодня - {weatherUpdateResult.Temperature}{weatherUpdateResult.TemperatureUnit}";
        }

        public override void Stop()
        {
            _weatherUpdatingService.Stop();
            _weatherUpdatingService.NewWeatherUpdateEvent -= OnWeatherUpdated;
        }
    }
}