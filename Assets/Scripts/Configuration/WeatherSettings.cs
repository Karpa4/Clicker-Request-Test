using UnityEngine;

namespace Configuration
{
    public interface IWeatherSettings
    {
        string Url { get; }
        float TimeBetweenRefresh { get; }
    }

    [CreateAssetMenu(fileName = @"WeatherSettings", menuName = @"Cifkor/Configurations/Weather Settings", order = 2)]
    public class WeatherSettings : ScriptableObject, IWeatherSettings
    {
        [SerializeField] private string _url = "https://api.weather.gov/gridpoints/TOP/32,81/forecast";
        [SerializeField] private float _timeBetweenRefresh = 5;

        public string Url => _url;
        public float TimeBetweenRefresh => _timeBetweenRefresh;
    }
}