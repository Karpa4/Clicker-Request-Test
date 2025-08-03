using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Configuration;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

namespace Services
{
    public interface IBreedInfoProvider
    {
        UniTask<List<DogBreedShort>> GetAllBreeds();
        UniTask<DogBreedFull> GetBreed(string breedId);
        void Clear();
    }
    
    public class BreedInfoProvider : IBreedInfoProvider
    {
        private CancellationTokenSource _cts;
        
        [Inject] private IDogSettings _dogSettings;
        [Inject] private IRequestSender _requestSender;

        public async UniTask<List<DogBreedShort>> GetAllBreeds()
        {
            _cts ??= new CancellationTokenSource();
            var request = UnityWebRequest.Get(_dogSettings.Url);
            var manyBreedResponse = await _requestSender.SendRequest(request, _cts, webRequest => JsonUtility.FromJson<ManyBreedResponse>(webRequest.downloadHandler.text));

            if (manyBreedResponse != null) 
                return GetShortBreeds(manyBreedResponse.data);
            
            Debug.LogError($"{GetType().Name} Invalid dog breeds format");
            return new List<DogBreedShort>();
        }

        public async UniTask<DogBreedFull> GetBreed(string breedId)
        {
            _cts ??= new CancellationTokenSource();
            var request = UnityWebRequest.Get(_dogSettings.Url + $"/{breedId}");
            var singleBreedResponse = await _requestSender.SendRequest(request, _cts, webRequest => JsonUtility.FromJson<SingleBreedResponse>(webRequest.downloadHandler.text));
            
            if (singleBreedResponse != null) 
                return GetFullBreed(singleBreedResponse.data);
            
            Debug.LogError($"{GetType().Name} Invalid dog breeds format");
            return null;
        }

        public void Clear()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
            _requestSender.ClearCanceledRequests();
        }

        private static DogBreedFull GetFullBreed(DogBreed data)
        {
            return new DogBreedFull(data.id, data.attributes.name, data.attributes.description);
        }
        
        private static List<DogBreedShort> GetShortBreeds(List<DogBreed> breeds)
        {
            return breeds.Select(breed => new DogBreedShort(breed.id, breed.attributes.name)).ToList();
        }
        
        [Serializable]
        private class ManyBreedResponse
        {
            public List<DogBreed> data;
            public Meta meta;
        }
        
        [Serializable]
        private class SingleBreedResponse
        {
            public DogBreed data;
        }

        [Serializable]
        private class Meta
        {
            public Pagination pagination;
        }

        [Serializable]
        private class Pagination
        {
            public int current_page;
            public int prev_page;
            public int next_page;
            public int total_pages;
        }
        
        [Serializable]
        private class DogBreed
        {
            public string id;
            public DogAttributes attributes;
        }

        [Serializable]
        private class DogAttributes
        {
            public string name;
            public string description;
        }
    }

    public class DogBreedShort
    {
        public string Id { get; }
        public string Name { get; }
        
        public DogBreedShort(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
    
    public class DogBreedFull : DogBreedShort
    {
        public string Description { get; }
        
        public DogBreedFull(string id, string name, string description) : base(id, name)
        {
            Description = description;
        }
    }
}