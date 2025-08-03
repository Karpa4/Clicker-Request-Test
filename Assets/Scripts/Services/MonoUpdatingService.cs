using System.Collections.Generic;
using UnityEngine;

namespace Services
{
    public interface IMonoUpdatingService
    {
        void Start();
        void Stop();
        void Add(IUpdatable item);
        void Remove(IUpdatable item);
    }

    public class MonoUpdatingService : MonoBehaviour, IMonoUpdatingService
    {
        private bool _needUpdate;
        private readonly HashSet<IUpdatable> _toUpdate = new();
        private readonly Dictionary<IUpdatable, bool> _updatablesForChange = new();
        
        public void Start()
        {
            _needUpdate = true;
        }
    
        public void Stop()
        {
            _needUpdate = false;
        }

        public void Add(IUpdatable item)
        {
            if (_toUpdate.Contains(item))
            {
                if (_updatablesForChange.TryGetValue(item, out bool toAdd))
                {
                    if (!toAdd)
                        _updatablesForChange.Remove(item);
                    else
                        Debug.LogError($"[{GetType().Name}] Duplicate updating item: {item}");
                }
                else
                    Debug.LogError($"[{GetType().Name}] Duplicate updating item: {item}");
                
                return;
            }

            if (item != null)
                _updatablesForChange[item] = true;
            else
                Debug.LogError($"[{GetType().Name}] Trying to add NULL item!");
        }

        public void Remove(IUpdatable item)
        {
            if (item != null)
                _updatablesForChange[item] = false;
            else
                Debug.LogError($"[{GetType().Name}] Trying to remove NULL item!");
        }
    
        private void Update()
        {
            if (!_needUpdate)
                return;
        
            if (_updatablesForChange.Count != 0)
            {
                foreach (KeyValuePair<IUpdatable, bool> pair in _updatablesForChange)
                {
                    if (pair.Value)
                        _toUpdate.Add(pair.Key);
                    else
                        _toUpdate.Remove(pair.Key);
                }

                _updatablesForChange.Clear();
            }
        
            foreach (IUpdatable item in _toUpdate)
                item.OnUpdate(Time.deltaTime);
        }
    }

    public interface IUpdatable
    {
        void OnUpdate(float deltaTime);
    }
}