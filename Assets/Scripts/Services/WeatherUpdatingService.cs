using System;
using System.Threading;
using Configuration;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

namespace Services
{
    public interface IWeatherUpdatingService
    {
        event Action<WeatherUpdateResult> NewWeatherUpdateEvent;
        
        void Start();
        void Stop();
    }
    
    public class WeatherUpdatingService : IWeatherUpdatingService, IUpdatable
    {
        private CancellationTokenSource _cts;
        private bool _isActive;
        private bool _updateInProcess;
        private float _lastRefreshTime;
        
        [Inject] private IMonoUpdatingService _monoUpdatingService;
        [Inject] private IWeatherSettings _weatherSettings;
        [Inject] private IRequestSender _requestSender;

        public event Action<WeatherUpdateResult> NewWeatherUpdateEvent;
    
        public void Stop()
        {
            _monoUpdatingService.Remove(this);
            _isActive = false;
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
            _requestSender.ClearCanceledRequests();
        }

        public void Start()
        {
            if (_isActive)
                return;

            _cts = new CancellationTokenSource();
            _isActive = true;
            _monoUpdatingService.Add(this);
        }
        
        public void OnUpdate(float _)
        {
            if (!_isActive || _updateInProcess)
                return;
            
            if (_lastRefreshTime != 0 && Time.time - _lastRefreshTime < _weatherSettings.TimeBetweenRefresh)
                return;

            _updateInProcess = true;
            RefreshWeather().Forget();
        }
        
        private async UniTaskVoid RefreshWeather()
        {
            var request = UnityWebRequest.Get(_weatherSettings.Url);
            var weatherData = await _requestSender.SendRequest(request, _cts, webRequest => JsonUtility.FromJson<WeatherData>(webRequest.downloadHandler.text));
            
            if (weatherData?.properties?.periods == null || weatherData.properties.periods.Length == 0)
            {
                Debug.LogError($"[{GetType().Name}] Invalid weather data format");
                return;
            }
            
            var todayWeather = weatherData.properties.periods[0];
            var iconRequest = UnityWebRequestTexture.GetTexture(todayWeather.icon);
            var weatherIcon = await _requestSender.SendRequest(iconRequest, _cts, DownloadHandlerTexture.GetContent);
            
            if (weatherIcon == null)
            {
                Debug.LogError($"[{GetType().Name}] Invalid weather data format");
                return;
            }
            
            var weatherUpdateResult = new WeatherUpdateResult(todayWeather.temperature, todayWeather.temperatureUnit, weatherIcon);
            _updateInProcess = false;
            _lastRefreshTime = Time.time;
            NewWeatherUpdateEvent?.Invoke(weatherUpdateResult);
        }
    }
    
    [Serializable]
    public class WeatherData
    {
        public WeatherProperties properties;
    }

    [Serializable]
    public class WeatherProperties
    {
        public WeatherPeriod[] periods;
    }

    [Serializable]
    public class WeatherPeriod
    {
        public string icon;
        public int temperature;
        public string temperatureUnit;
    }
    
    public class WeatherUpdateResult
    {
        public int Temperature { get; }
        public string TemperatureUnit { get; }
        public Texture2D Texture { get; }
            
        public WeatherUpdateResult(int temperature, string temperatureUnit, Texture2D texture)
        {
            Temperature = temperature;
            TemperatureUnit = temperatureUnit;
            Texture = texture;
        }
    }
}
