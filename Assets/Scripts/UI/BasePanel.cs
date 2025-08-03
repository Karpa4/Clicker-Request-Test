using System.Collections.Generic;
using UI.Presenters;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public abstract class BasePanel : MonoBehaviour
    {
        protected List<PresenterBase> _presenters = new();

        public abstract void Initialize(UIDocument uiDocument);
        
        public void Activate()
        {
            foreach (var presenter in _presenters)
                presenter.Start();
        }
        
        public void Deactivate()
        {
            foreach (var presenter in _presenters)
                presenter.Stop();
            
            _presenters.Clear();
        }
    }
}