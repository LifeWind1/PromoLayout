using System;
using System.Collections.Generic;
using System.Linq;
using RedPanda.Project.Data;
using RedPanda.Project.Interfaces;
using RedPanda.Project.Services.Interfaces;
using RedPanda.Project.Services.ObjectPool;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RedPanda.Project.UI.Promo
{
    public class PromoView : View
    {
        [SerializeField] private TextMeshProUGUI _gemsValue;
        [SerializeField] private Transform _sectionsTransform;
        [SerializeField] private ScrollRect _scrollRect;
        
        private IPromoService _promoService;
        private IUserService _userService;
        private IObjectPoolService _objectPoolService;
        private List<PromoSection> _promoSections = new();
        
        private void Start()
        {
            _promoService = Container.Locate<IPromoService>();
            _userService = Container.Locate<IUserService>();
            _objectPoolService = Container.Locate<IObjectPoolService>();
            _userService.OnCurrencyValueChanged += OnCurrencyValueChanged;
            Init();
        }

        private void Init()
        {
            _gemsValue.SetText(_userService.Currency.ToString());
            var promos = _promoService.GetPromos();
            var chestPromos = promos.Where(x => x.Type == PromoType.Chest).OrderByDescending(x => x.Rarity).ToList();
            var specialPromos = promos.Where(x => x.Type == PromoType.Special).OrderByDescending(x => x.Rarity).ToList();
            var inAppPromos = promos.Where(x => x.Type == PromoType.InApp).OrderByDescending(x => x.Rarity).ToList();
            
            CreateSectionWithCards(chestPromos);
            CreateSectionWithCards(specialPromos);
            CreateSectionWithCards(inAppPromos);
        }

        private void CreateSectionWithCards(List<IPromoModel> data)
        {
            if (data.Count > 0)
            {
                var section = _objectPoolService.PromoSectionPool.Get<PromoSection>(_sectionsTransform);
                section.Init(Container, data, _objectPoolService, _scrollRect);
                _promoSections.Add(section);
            }
        }
        
        private void OnCurrencyValueChanged(int gems)
        {
            _gemsValue.SetText(gems.ToString());
        }

        private void OnDestroy()
        {
            _userService.OnCurrencyValueChanged -= OnCurrencyValueChanged;
        }
    }
}