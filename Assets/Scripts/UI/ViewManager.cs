using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public interface IViewManager
    {
        void Initialize();
    }

    public class ViewManager : MonoBehaviour, IViewManager
    {
        [SerializeField] private UIDocument _uiDocument;
        [SerializeField] private PanelInfo[] _panelInfos;
        
        private VisualElement _tabContent;
        private int _activeButtonIndex = -1;
        private readonly List<Button> _navigationButtons = new();

        public void Initialize()
        {
            var root = _uiDocument.rootVisualElement;
            _tabContent = root.Q<VisualElement>("TabsContainer");

            foreach (var panelInfo in _panelInfos)
                AddNewButton(root, panelInfo.buttonName);
            
            ShowNewTab(0);
        }

        private void AddNewButton(VisualElement root, string buttonName)
        {
            var button = root.Q<Button>(buttonName);
            var index = _navigationButtons.Count;
            button.clicked += () => ShowNewTab(index);
            _navigationButtons.Add(button);
        }

        private void ShowNewTab(int tabIndex)
        {
            if (_activeButtonIndex == tabIndex)
                return;

            if (_activeButtonIndex >= 0 && _activeButtonIndex < _navigationButtons.Count)
            {
                _panelInfos[_activeButtonIndex].panel.Deactivate();
                _navigationButtons[_activeButtonIndex].RemoveFromClassList("nav-button-active");
                _navigationButtons[_activeButtonIndex].AddToClassList("nav-button");
            }

            _activeButtonIndex = tabIndex;
            _tabContent.Clear();
            var newPanelInfo = _panelInfos[_activeButtonIndex];
            newPanelInfo.virtualTreeAsset.CloneTree(_tabContent);
            newPanelInfo.panel.Initialize(_uiDocument);
            newPanelInfo.panel.Activate();
            _navigationButtons[_activeButtonIndex].RemoveFromClassList("nav-button");
            _navigationButtons[_activeButtonIndex].AddToClassList("nav-button-active");
        }
        
        [Serializable]
        private class PanelInfo
        {
            public BasePanel panel;
            public VisualTreeAsset virtualTreeAsset;
            public string buttonName;
        }
    }
}