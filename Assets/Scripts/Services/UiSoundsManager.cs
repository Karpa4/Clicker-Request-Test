using System.Collections.Generic;
using Configuration;
using UnityEngine;
using Zenject;

namespace Services
{
    public interface IUiSoundsManager
    {
        void Initialize();
        void PlayOneShot(AudioClip clip);
        void PlayOneShot(UiSoundEffectType effectType);
    }

    public class UiSoundsManager : IUiSoundsManager
    {
        private AudioSource _audioSource;
        private Dictionary<UiSoundEffectType, AudioClip> _audioClips;

        [Inject] private IUiSoundManagerConfiguration _config;

        public void Initialize()
        {
            _audioClips = _config.GetSoundsInfoByType();
            var oneShotAudioSource = Object.Instantiate(_config.UiAudioSourcePrefab);
            oneShotAudioSource.name = "UIAudioSource";
            Object.DontDestroyOnLoad(oneShotAudioSource);
            _audioSource = oneShotAudioSource;
        }

        public void PlayOneShot(AudioClip clip)
        {
            _audioSource.PlayOneShot(clip);
        }

        public void PlayOneShot(UiSoundEffectType effectType)
        {
            if (_audioClips.TryGetValue(effectType, out var clip))
            {
                _audioSource.PlayOneShot(clip);
                return;
            }

            Debug.LogError($"{GetType().Name} AudioClip for {effectType} not found");
        }
    }
}