using Grace.DependencyInjection;
using Grace.DependencyInjection.Attributes;
using RedPanda.Project.Services.Interfaces;
using UnityEngine;

namespace RedPanda.Project.UI
{
    public abstract class View : MonoBehaviour
    {
        protected IUIService UIService { get; private set; }
        protected IExportLocatorScope Container { get; private set; }
        
        [Import]
        public void Inject(IExportLocatorScope container, IUIService uiService)
        {
            UIService = uiService;
            Container = container;
        }
    }
}