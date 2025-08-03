using Cysharp.Threading.Tasks;
using Services;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Presenters
{
    public class BreedsPresenter : PresenterBase
    {
        private const float ROTATION_SPEED = 90f;
        private const float FONT_SIZE = 25f;
        private const float ICON_SIZE = 50f;
        private const float ELEMENT_HEIGHT = 70f;
        
        private readonly IBreedInfoProvider _breedInfoProvider;
        private readonly IPopupService _popupService;
        private readonly VisualElement _breedList;
        private readonly VisualElement _panelLoader;
        private readonly Texture2D _loadingTexture;
        
        private bool _isRotating;
        private string _currentLoadingBreedId;

        public BreedsPresenter(IBreedInfoProvider breedInfoProvider, IPopupService popupService, UIDocument uiDocument, Texture2D loadingTexture)
        {
            _breedInfoProvider = breedInfoProvider;
            _popupService = popupService;
            _loadingTexture = loadingTexture;
            _breedList = uiDocument.rootVisualElement.Q<ScrollView>("BreedList");
            _panelLoader = uiDocument.rootVisualElement.Q<VisualElement>("PanelLoader");
            _panelLoader.style.backgroundImage = new StyleBackground(_loadingTexture);
        }

        public override void Start()
        {
            RotateLoader().Forget();
            ShowBreeds().Forget();
        }

        public override void Stop()
        {
            _currentLoadingBreedId = null;
            _isRotating = false;
            _breedInfoProvider.Clear();
            _breedList.Clear();
        }
        
        private async UniTaskVoid RotateLoader()
        {
            var rotation = 0f;
            
            while (_isRotating)
            {
                rotation = (rotation + ROTATION_SPEED / 60f) % 360f;
                _panelLoader.style.rotate = new StyleRotate(new Rotate(rotation));
                await UniTask.DelayFrame(1);
            }
        }

        private async UniTaskVoid ShowBreeds()
        {
            _isRotating = true;
            _panelLoader.AddToClassList("active");
            _breedList.RemoveFromClassList("active");
            var allBreeds = await _breedInfoProvider.GetAllBreeds();
            _panelLoader.RemoveFromClassList("active");
            _isRotating = false;
            _breedList.AddToClassList("active");
            
            if (allBreeds == null || allBreeds.Count == 0)
                return;

            _breedList.Clear();

            for (int i = 0; i < allBreeds.Count; i++)
            {
                var breed = allBreeds[i];
                var button = new Button { name = $"BreedButton_{breed.Id}", text = "" };
                button.AddToClassList("breed-button");
                button.style.height = ELEMENT_HEIGHT;
                var numberLabel = new Label((i + 1).ToString()) { name = $"BreedNumber_{breed.Id}" };
                numberLabel.AddToClassList("breed-number");
                numberLabel.style.fontSize = FONT_SIZE;
                var nameLabel = new Label(breed.Name) { name = $"BreedName_{breed.Id}" };
                nameLabel.AddToClassList("breed-name");
                nameLabel.style.fontSize = FONT_SIZE;
                var loadingElement = new VisualElement { name = $"BreedLoading_{breed.Id}" };
                loadingElement.AddToClassList("breed-loading");
                loadingElement.style.width = ICON_SIZE;
                loadingElement.style.height = ICON_SIZE;
                loadingElement.style.backgroundImage = new StyleBackground(_loadingTexture);
                button.Add(numberLabel);
                button.Add(nameLabel);
                button.Add(loadingElement);
                button.clicked += () => OnBreedClicked(breed.Id, loadingElement).Forget();
                _breedList.Add(button);
            }
        }
        
        private async UniTaskVoid OnBreedClicked(string breedId, VisualElement loadingElement)
        {
            if (_currentLoadingBreedId == breedId)
                return;
            
            if (_currentLoadingBreedId != breedId)
                _breedInfoProvider.Clear();
            
            _currentLoadingBreedId = breedId;
            loadingElement.AddToClassList("active");
            var dogBreed = await _breedInfoProvider.GetBreed(breedId);
            loadingElement.RemoveFromClassList("active");

            if (_currentLoadingBreedId != breedId)
                return;
            
            _currentLoadingBreedId = null;
            
            if (dogBreed == null)
                return;
            
            _popupService.ShowPopup(dogBreed.Name, dogBreed.Description);
        }
    }
}