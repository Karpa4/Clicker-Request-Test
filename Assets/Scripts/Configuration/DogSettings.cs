using UnityEngine;

namespace Configuration
{
    public interface IDogSettings
    {
        string Url { get; }
        int ItemsCount { get; }
    }

    [CreateAssetMenu(fileName = @"DogSettings", menuName = @"Cifkor/Configurations/Dog Settings", order = 3)]
    public class DogSettings : ScriptableObject, IDogSettings
    {
        [SerializeField] private string _url = "https://dogapi.dog/api/v2/breeds";
        [SerializeField] private int _itemsCount = 10;

        public string Url => _url;
        public int ItemsCount => _itemsCount;
    }
}