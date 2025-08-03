using Services;
using UI.Presenters;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace UI
{
    public class BreedsPanel : BasePanel
    {
        [SerializeField] private Texture2D _loadingTexture;
        
        [Inject] private IBreedInfoProvider _breedInfoProvider;
        [Inject] private IPopupService _popupService;
        
        public override void Initialize(UIDocument uiDocument)
        {
            _presenters.Add(CreateBreedsPresenter(uiDocument));
        }
        
        private PresenterBase CreateBreedsPresenter(UIDocument uiDocument)
        {
            return new BreedsPresenter(_breedInfoProvider, _popupService, uiDocument, _loadingTexture);
        }
    }
}
