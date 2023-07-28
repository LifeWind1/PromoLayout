using DG.Tweening;
using Grace.DependencyInjection;
using Grace.DependencyInjection.Attributes;
using RedPanda.Project.Interfaces;
using RedPanda.Project.Services.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RedPanda.Project.UI.Promo
{
    public class PromoCard : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _price;
        [SerializeField] private Image _background;
        [SerializeField] private Image _icon;
        [SerializeField] private Button _buyButton;

        private const float _animationDuration = 0.23f;
        private const float _scale = 1.1f;

        private IUserService _userService;
        private IPromoModel _data;
        private Tween _onBuyTween;
        
        private void Start()
        {
            _buyButton.onClick.AddListener(OnBuyClick);
        }

        public void Init(IExportLocatorScope container, IPromoModel data)
        {
            _userService = container.Locate<IUserService>();
            _data = data;
            _title.SetText(data.Title);
            _price.SetText(data.Cost.ToString());
            _background.sprite = Resources.Load<Sprite>(data.GetBackground());
            _icon.sprite = Resources.Load<Sprite>(data.GetIcon());
        }

        private void OnBuyClick()
        {
            _onBuyTween?.Kill();
            _onBuyTween = transform.DOScale(_scale, _animationDuration).From(1).OnComplete(() =>
            {
                _onBuyTween = transform.DOScale(1, _animationDuration);
            });
                
            if (_userService.HasCurrency(_data.Cost))
            {
                _userService.ReduceCurrency(_data.Cost);
                Debug.Log($"Успешная покупка {_data.Title}");
            }
            else
            {
                Debug.LogError("Недостаточно кристаллов");
            }
        }

        private void OnDestroy()
        {
            _buyButton.onClick.RemoveListener(OnBuyClick);
        }
    }
}