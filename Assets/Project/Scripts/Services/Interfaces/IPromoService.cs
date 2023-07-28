using System.Collections.Generic;
using RedPanda.Project.Interfaces;

namespace RedPanda.Project.Services.Interfaces
{
    public interface IPromoService
    {
        public IReadOnlyList<IPromoModel> GetPromos();
    }
}