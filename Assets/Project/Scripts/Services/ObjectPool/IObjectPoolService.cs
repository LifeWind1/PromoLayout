using UnityEngine;

namespace RedPanda.Project.Services.ObjectPool
{
    public interface IObjectPoolService
    {
        RectTransform UiObjectHolder { get; }
        Transform ObjectHolder { get; }
        ObjectPool PromoSectionPool { get; }
        ObjectPool PromoCardPool { get; }
    }
}