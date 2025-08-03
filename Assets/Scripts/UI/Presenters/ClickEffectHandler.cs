using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Presenters
{
    public class ClickEffectHandler : MonoBehaviour
    {
        [SerializeField] private UIDocument _uiDocument;
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private RenderTexture _particleTexture;
        [SerializeField] private float _flyHeight = 150f;
        [SerializeField] private float _distanceToCamera = 10f;
        [SerializeField] private float _durationSec = 0.4f;
    
        private VisualElement _effectsContainer;
        private Button _clickButton;
        
        public void Initialize()
        {
            _effectsContainer = _uiDocument.rootVisualElement.Q<VisualElement>("EffectContainer");
            _clickButton = _uiDocument.rootVisualElement.Q<Button>("ClickButton");
        }

        public void ShowUIEffects(int currencyCount)
        {
            var animLabel = new Label($"+{currencyCount}");
            animLabel.AddToClassList("effect-label");
            _effectsContainer.Add(animLabel);
            var particleImage = new Image();
            particleImage.image = _particleTexture;
            particleImage.style.position = Position.Absolute;
            particleImage.pickingMode = PickingMode.Ignore;
            particleImage.style.left = 0;
            particleImage.style.right = 0;
            particleImage.style.top = 0;
            particleImage.style.bottom = 0;
            _effectsContainer.Add(particleImage);
            _particleSystem.Play();
            var buttonRect = _clickButton.worldBound;
            var buttonCenter = new Vector2(buttonRect.x + buttonRect.width/2, buttonRect.y + buttonRect.height/2);
            var localPos = _effectsContainer.WorldToLocal(buttonCenter);
            animLabel.style.bottom = _effectsContainer.resolvedStyle.height - localPos.y;
            animLabel.style.opacity = 1;
            StartAnimation(animLabel);
        }

        private void StartAnimation(VisualElement label)
        {
            label.experimental.animation.Start(label.style.bottom.value.value, label.style.bottom.value.value + _flyHeight, (int)(_durationSec * 1000), (el,val) => el.style.bottom = val);
            var valueAnimation = label.experimental.animation.Start(1, 0, (int)(_durationSec * 1000), (el,val) => el.style.opacity = val);
            valueAnimation.onAnimationCompleted = () => _effectsContainer.Remove(label);
        }
    }
}