using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public interface IPopupService
    {
        void Initialize();
        void ShowPopup(string title, string text);
    }

    public class PopupService : MonoBehaviour, IPopupService
    {
        [SerializeField] private UIDocument _uiDocument;
        [SerializeField] private VisualTreeAsset _popup;

        private VisualElement _content;
        
        public void Initialize()
        {
            _content = _uiDocument.rootVisualElement.Q<VisualElement>("MainContent");
            _content.pickingMode = PickingMode.Ignore;
        }
        
        public void ShowPopup(string title, string text)
        {
            _content.Clear();
            _content.pickingMode = PickingMode.Position;
            _popup.CloneTree(_content);
            _content.Q<Label>("PopupTitle").text = title;
            _content.Q<Label>("PopupDescription").text = text;
            var closeButton = _content.Q<Button>("CloseButton");
            closeButton.clicked += OnCloseButtonClicked;
        }

        private void OnCloseButtonClicked()
        {
            _content.Clear();
            _content.pickingMode = PickingMode.Ignore;
        }
    }
}
