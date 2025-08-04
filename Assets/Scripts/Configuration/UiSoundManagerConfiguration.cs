using System;
using System.Collections.Generic;
using UnityEngine;

namespace Configuration
{
    public interface IUiSoundManagerConfiguration
    {
        AudioSource UiAudioSourcePrefab { get; }
        Dictionary<UiSoundEffectType, AudioClip> GetSoundsInfoByType();
    }

    [CreateAssetMenu(fileName = @"UiSoundManagerConfiguration", menuName = @"Cifkor/Configurations/Ui Sound Manager Configuration", order = 4)]
    public class UiSoundManagerConfiguration : ScriptableObject, IUiSoundManagerConfiguration
    {
        [SerializeField] private CLipWithType[] _soundsInfo;
        [SerializeField] private AudioSource _audioSourcePrefab;

        public AudioSource UiAudioSourcePrefab => _audioSourcePrefab;

        public Dictionary<UiSoundEffectType, AudioClip> GetSoundsInfoByType()
        {
            var res = new Dictionary<UiSoundEffectType, AudioClip>(_soundsInfo.Length);

            foreach (var infoWithType in _soundsInfo)
                res[infoWithType.Type] = infoWithType.Info;
            
            return res;
        }
        
        [Serializable]
        private class CLipWithType
        {
            public UiSoundEffectType Type;
            public AudioClip Info;
        }
    }
    
    public enum UiSoundEffectType
    {
        CurrencyAdded
    }
}
