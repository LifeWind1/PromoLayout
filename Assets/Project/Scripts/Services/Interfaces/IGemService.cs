using System;

namespace RedPanda.Project.Services.Interfaces
{
    public interface IGemService
    {
        event Action<int> GemsValueChanged;
        int Gems { get; }
        bool TryBuy(int price);
    }
}