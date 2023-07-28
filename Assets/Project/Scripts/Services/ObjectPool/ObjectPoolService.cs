using System;
using UnityEngine;

namespace RedPanda.Project.Services.ObjectPool
{
    public class ObjectPoolService : MonoBehaviour, IObjectPoolService
    {
        [SerializeField] private RectTransform _uiObjectHolder;
        [SerializeField] private Transform _objectHolder;
    
        public RectTransform UiObjectHolder => _uiObjectHolder;
        public Transform ObjectHolder => _objectHolder;
        public ObjectPool PromoSectionPool => PromoSection;
        public ObjectPool PromoCardPool => PromoCard;

        public ObjectPool PromoSection = new();
        public ObjectPool PromoCard = new();
    }
}