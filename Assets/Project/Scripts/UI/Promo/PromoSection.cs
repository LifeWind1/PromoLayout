using System.Collections.Generic;
using Grace.DependencyInjection;
using RedPanda.Project.Interfaces;
using RedPanda.Project.Services.ObjectPool;
using RedPanda.Project.UI.Scroll;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RedPanda.Project.UI.Promo
{
    public class PromoSection : MonoBehaviour, IObjectPool
    {
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private Transform _cardsTransform;
        [SerializeField] private ScrollRectNested _scrollRectNested;
        
        private List<PromoCard> _promoCards = new();

        public void Init(IExportLocatorScope container, List<IPromoModel> data, IObjectPoolService objectPoolService, 
            ScrollRect parentScroll)
        {
            _title.SetText(data[0].Type.ToString());
            _scrollRectNested.SetParentScroll(parentScroll);
            
            foreach (var promoModel in data)
            {
                var card = objectPoolService.PromoCardPool.Get<PromoCard>(_cardsTransform);
                card.Init(container, promoModel);
                _promoCards.Add(card);
            }
        }
    }
}