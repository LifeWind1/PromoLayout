using System;
using RedPanda.Project.Services.Interfaces;
using UnityEngine;

namespace RedPanda.Project.Services
{
    public class GemService : IGemService
    {
        public event Action<int> GemsValueChanged;
        public int Gems { get; private set; }

        public GemService()
        {
            Gems = 500;
        }
        
        public bool TryBuy(int price)
        {
            if (price > Gems)
            {
                Debug.LogError("Недостаточно кристаллов");
                return false;
            }

            Gems -= price;
            GemsValueChanged?.Invoke(Gems);
            return true;
        }
    }
}